// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Globalization;
using System.Xml;

using OnTheFlyCompiler.Errors;
using OnTheFlyCompiler.Settings;

namespace OnTheFlyCompiler
{
	abstract class CompilerSettingsHelper
	{
		internal static CompilerSettings Parse(string[] args)
		{
			CompilerSettings settings = new CompilerSettings();
			for (int i = 0; i < args.Length; i++)
			{
				string name = args[i];
				try
				{
					switch (name)
					{
						case "--debug":
							{
								settings.IncludeDebugInformation = true;
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
						case "-f":
							{
								string[] arr = args[++i].Split(';');
								foreach (string str in arr)
								{
									settings.Sources.Add(System.IO.File.ReadAllText(str));
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
								string[] arr = args[++i].Split(';');
								settings.ReferencedAssemblies.AddRange(arr);
								break;
							}
						case "-s":
							{
								string[] arr = args[++i].Split(';');
								settings.Sources.AddRange(arr);
								break;
							}
						case "--threat":
							{
								settings.TreatWarningsAsErrors = true;
								break;
							}
						case "-v":
							{
								int verbose = Convert.ToInt32(args[++i], CultureInfo.CurrentCulture);
								if (verbose > 2)
								{
									throw new ParameterOutOfRangeException(name, verbose.ToString(CultureInfo.CurrentCulture));
								}
								else
								{
									settings.Verbose = verbose;
								}
								break;
							}
						case "-w":
							{
								try
								{
									int warning = Convert.ToInt32(args[++i], CultureInfo.CurrentCulture);
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
									string xml = String.Empty;
									try
									{
										xml = args[++i];
										XmlDocument doc = new XmlDocument();
										doc.Load(xml);
										settings = Parse(doc);
									}
									catch (IndexOutOfRangeException)
									{
										throw new ParameterNotSetException(name);
									}
									catch (System.IO.FileNotFoundException)
									{
										throw;
									}
									catch (XmlException ex)
									{
										throw new Exception(String.Format(CultureInfo.CurrentCulture, "Error parsing XmlConfiguration: {0}", ex.Message));
									}
									catch (Exception ex)
									{
										throw new ParameterOutOfRangeException(name, xml);
									}
								}
								catch (NullReferenceException ex)
								{
									throw new ReadingXmlDescriptionException(ex);
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
			CompilerSettings settings = new CompilerSettings();

			XmlNode nodeSettings = doc.SelectSingleNode("//settings");
			XmlNode nodeCompiler = nodeSettings.SelectSingleNode("compiler");
			XmlAttribute atrLanguage = nodeCompiler.Attributes["language"];
			if (atrLanguage != null)
			{
				settings.Language = atrLanguage.Value;
			}
			else
			{
				throw new ReadingXmlDescriptionException(new XmlException("Required attribute 'Language' is missed"));
			}

			XmlAttribute atrWarning = nodeCompiler.Attributes["warning"];
			if (atrWarning != null)
			{
				settings.WarningLevel = Convert.ToInt32(atrWarning.Value);
			}

			XmlNode nodeExecutable = nodeSettings.SelectSingleNode("executable");
			XmlAttribute atrType = nodeExecutable.Attributes["type"];
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

			XmlAttribute atrMemory = nodeExecutable.Attributes["memory"];
			if (atrMemory != null)
			{
				settings.GenerateInMemory = Boolean.Parse(atrMemory.Value);
			}

			XmlNode nodeMethod = nodeSettings.SelectSingleNode("method");
			XmlAttribute atrPath = nodeMethod.Attributes["path"];
			if (atrPath != null)
			{
				settings.MethodPath = atrPath.Value;
			}

			XmlAttribute atrName = nodeMethod.Attributes["name"];
			if (atrName != null)
			{
				settings.MethodName = atrName.Value;
			}

			foreach (XmlNode nodeRef in nodeSettings.SelectNodes("references/item"))
			{
				settings.ReferencedAssemblies.Add(nodeRef.InnerText);
			}

			foreach (XmlNode nodeFile in nodeSettings.SelectNodes("files/item"))
			{
				settings.Sources.Add(System.IO.File.ReadAllText(nodeFile.InnerText));
			}

			return settings;
		}
	}
}
