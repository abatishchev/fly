// Copyright (C) 2008-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

using OnTheFlyCompiler.Errors;
using OnTheFlyCompiler.Settings;

namespace OnTheFlyCompiler
{
	abstract class CompilerSettingsParser
	{
		internal static CompilerSettings Parse(string[] args)
		{
			var settings = new CompilerSettings();
			for (int i = 0; i < args.Length; i++)
			{
				string name = args[i];
				try
				{
					switch (name)
					{
						case "--appd":
						case "--appdomain":
							{
								settings.AppDomainCreation = true;
								break;
							}
						case "--debug":
							{
								settings.IncludeDebugInformation = true;
								break;
							}
						case "--disk":
							{
								settings.GenerateInMemory = false;
								break;
							}
						case "--dll":
							{
								settings.GenerateExecutable = false;
								break;
							}
						case "--exe":
							{
								settings.GenerateExecutable = true;
								break;
							}
						case "--exec":
							{
								settings.Execute = true;
								break;
							}
						case "-a":
							{
								break;
							}
						case "-f":
							{
								var arr = args[++i].Split(';');
								foreach (var str in arr)
								{
									settings.Sources.Add(File.ReadAllText(str));
								}
								break;
							}
						case "-l":
							{
								settings.Language = args[++i];
								break;
							}
						case "--memory":
							{
								settings.GenerateInMemory = true;
								break;
							}
						case "-n":
							{
								settings.MethodName = args[++i];
								break;
							}
						case "-p":
							{
								settings.MethodPath = args[++i];
								break;
							}
						case "-r":
							{
								var arr = args[++i].Split(';');
								settings.ReferencedAssemblies.AddRange(arr);
								break;
							}
						case "-s":
							{
								settings.Sources.Add(args[++i]);
								break;
							}
						case "-t":
							{
								try
								{
									settings.BindingFlag = BindingFlags.InvokeMethod | BindingFlags.Public;
									settings.GenerateExecutable = false;
									settings.GenerateInMemory = false;
									settings.MethodPath = "OnTheFlyCompiler.Templates.Test";
									settings.ReferencedAssemblies.Add(System.Reflection.Assembly.GetExecutingAssembly().Location);

									var templateFile = args[++i];
									switch (templateFile)
									{

										case "test":
											{
												settings.Language = "c#";
												settings.MethodName = "Main";
												string source = File.ReadAllText(Path.Combine(Properties.Resources.TemplateDirectory, "Test.cs"));
												try
												{
													settings.Sources[0] = source.Replace(Properties.Resources.TemplateMarkerSharp, settings.Sources[0]);
												}
												catch (IndexOutOfRangeException)
												{
													throw new TemplateException("No source code was loaded");
												}
												break;
											}
										default:
											{
												throw new ParameterOutOfRangeException("template", templateFile);
											}
									}
								}
								catch (CompilerException)
								{
									// skip this argument
								}
								break;
							}
						case "--threat":
							{
								settings.TreatWarningsAsErrors = true;
								break;
							}
						case "-v":
							{
								var verbose = Convert.ToInt32(args[++i], CultureInfo.CurrentCulture);
								if (verbose > 2)
								{
									throw new ParameterOutOfRangeException(name, verbose.ToString(CultureInfo.CurrentCulture));
								}
								else
								{
									settings.VerboseLevel = verbose;
								}
								break;
							}
						case "-w":
							{
								try
								{
									var warning = Convert.ToInt32(args[++i], CultureInfo.CurrentCulture);
									if (warning > 4)
									{
										throw new ParameterOutOfRangeException(name, warning.ToString(CultureInfo.CurrentCulture));
									}
									else
									{
										settings.WarningLevel = warning;
									}
								}
								catch
								{
									throw new ParameterNotSetException(name);
								}
								break;
							}
						case "-x":
							{
								try
								{
									var xml = String.Empty;
									try
									{
										xml = args[++i];
										var doc = new XmlDocument();
										doc.Load(xml);
										settings = Parse(doc); // TODO +=
									}
									catch (IndexOutOfRangeException)
									{
										throw new ParameterNotSetException(name);
									}
									catch (FileNotFoundException)
									{
										throw;
									}
									catch (XmlException ex)
									{
										throw new Exception(String.Format(CultureInfo.CurrentCulture, "Error parsing XmlConfiguration: {0}", ex.Message));
									}
									catch (Exception)
									{
										throw new ParameterOutOfRangeException(name, xml);
									}
								}
								catch (NullReferenceException ex)
								{
									throw new XmlDescriptionException(ex);
								}
								break;
							}
						default:
							{
								throw new UnknownParameterException(name);
							}
					}
				}
				catch (IndexOutOfRangeException)
				{
					throw new ParameterNotSetException(name);
				}
			}
			return settings;
		}

		internal static CompilerSettings Parse(XmlDocument doc)
		{
			var settings = new CompilerSettings();

			var nodeSettings = doc.SelectSingleNode("//settings");
			var nodeCompiler = nodeSettings.SelectSingleNode("compiler");

			var atrLanguage = nodeCompiler.Attributes["language"];
			if (atrLanguage != null)
			{
				settings.Language = atrLanguage.Value;
			}
			else
			{
				throw new XmlDescriptionException(new XmlException("Required attribute 'Language' is missed"));
			}

			var atrVerbose = nodeCompiler.Attributes["verbose"];
			if (atrVerbose != null)
			{
				settings.VerboseLevel = Convert.ToInt32(atrVerbose.Value);
			}

			var atrWarning = nodeCompiler.Attributes["warning"];
			if (atrWarning != null)
			{
				settings.WarningLevel = Convert.ToInt32(atrWarning.Value);
			}

			var nodeMethod = nodeSettings.SelectSingleNode("method");

			var atrPath = nodeMethod.Attributes["path"];
			if (atrPath != null)
			{
				settings.MethodPath = atrPath.Value;
			}

			var atrName = nodeMethod.Attributes["name"];
			if (atrName != null)
			{
				settings.MethodName = atrName.Value;
			}

			var nodeOutput = nodeSettings.SelectSingleNode("output");

			var atrType = nodeOutput.Attributes["type"];
			if (atrType != null)
			{
				switch (atrType.Value)
				{
					case "exe":
						{
							settings.GenerateExecutable = true;
							break;
						}
					case "dll":
						{
							settings.GenerateExecutable = false;
							break;
						}
				}
			}

			var atrMemory = nodeOutput.Attributes["memory"];
			if (atrMemory != null)
			{
				settings.GenerateInMemory = Boolean.Parse(atrMemory.Value);
			}

			var atrExecute = nodeOutput.Attributes["execute"];
			if (atrExecute != null)
			{
				settings.Execute = Boolean.Parse(atrExecute.Value);
			}

			var nodeAppDomain = nodeSettings.SelectSingleNode("appDomain");

			bool create = false;

			var atrAppDomainCreation = nodeAppDomain.Attributes["create"];
			if (atrAppDomainCreation != null)
			{
				Boolean.TryParse(atrAppDomainCreation.Value, out create);
				settings.AppDomainCreation = create;
			}

			if (create)
			{
				var atrAppDomainName = nodeAppDomain.Attributes["name"];
				if (atrAppDomainName != null)
				{
					settings.AppDomainName = atrAppDomainName.Value;
				}
			}

			foreach (XmlNode nodeRef in nodeSettings.SelectNodes("references/item"))
			{
				settings.ReferencedAssemblies.Add(nodeRef.InnerText);
			}

			foreach (XmlNode nodeFile in nodeSettings.SelectNodes("files/item"))
			{
				settings.Sources.Add(File.ReadAllText(nodeFile.InnerText));
			}

			return settings;
		}
	}
}
