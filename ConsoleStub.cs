// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Globalization;
using System.IO;
using System.Xml;

using OnTheFlyCompiler.Errors;
using OnTheFlyCompiler.Settings;

namespace OnTheFlyCompiler
{
	static class ConsoleStub
	{
		[STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine(String.Format(CultureInfo.CurrentCulture, "{0} version {1}", Core.ApplicationTitle, Core.ApplicationVersion));
			Console.WriteLine(Core.ApplicationCopyright);
			Console.WriteLine();

			if (args.Length == 0)
			{
				PrintUsage();
				return;
			}
			try
			{
				Core.Init(Parse(args));
				Core.Compiler.Compile();
				Console.WriteLine(Core.Compiler.Output);
				if (Core.Compiler.Settings.Execute)
				{
					Core.Compiler.Execute();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error! {0}", ex.Message);
				return;
			}
		}

		public static CompilerSettings Parse(string[] args)
		{
			CompilerSettings settings = new CompilerSettings();
			for (int i = 0; i < args.Length; i++)
			{
				string name = args[i];
				object value = null;

				try
				{
					switch (name)
					{
						case "--debug":
							{
								value = true;
								settings.IncludeDebugInformation = true;
								break;
							}
						case "--exe":
							{
								value = true;
								settings.GenerateExecutable = true;
								break;
							}
						case "--exec":
							{
								value = true;
								settings.Execute = true;
								break;
							}
						case "-f":
							{
								string[] arr = args[++i].Split(';');
								foreach (string str in arr)
								{
									settings.Sources.Add(File.ReadAllText(str));
								}
								value = arr;
								break;
							}
						case "-l":
							{
								string str = args[++i];
								settings.Language = str;
								value = str;
								break;
							}
						case "--memory":
							{
								value = true;
								settings.GenerateInMemory = true;
								break;
							}
						case "-n":
							{
								string str = args[++i];
								value = str;
								settings.MethodName = str;
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
								value = arr;
								settings.ReferencedAssemblies.AddRange(arr);
								break;
							}
						case "-s":
							{
								string[] arr = args[++i].Split(';');
								settings.Sources.AddRange(arr);
								value = arr;
								break;
							}
						case "--threat":
							{
								value = true;
								settings.TreatWarningsAsErrors = true;
								break;
							}
						case "-v":
							{
								int level = Convert.ToInt32(args[++i], CultureInfo.CurrentCulture);
								if (level > 2)
								{
									throw new ParameterOutOfRangeException(name, level.ToString(CultureInfo.CurrentCulture));
								}
								else
								{
									settings.Verbose = level;
									value = level;
								}
								break;
							}
						case "-w":
							{
								try
								{
									int level = Convert.ToInt32(args[++i], CultureInfo.CurrentCulture);
									if (level > 4)
									{
										throw new ParameterOutOfRangeException(name, level.ToString(CultureInfo.CurrentCulture));
									}
									else
									{
										settings.WarningLevel = level;
										value = level;
									}
								}
								catch
								{
									throw new ParameterOutOfRangeException(name, (string)value);
								}
								break;
							}
						case "-x":
							{
								try
								{
									string xml = String.Empty; ;
									XmlDocument doc = new XmlDocument();
									try
									{
										xml = args[++i];
										doc.Load(xml);
										settings = CompilerSettings.Parse(doc);
									}
									catch
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
					if (value != null)
					{
						try
						{
							settings.SettingsContainer.Add(name, value);
						}
						catch (ArgumentNullException)
						{
							throw new ParameterNotSetException(name);
						}
						catch (ArgumentException)
						{
							throw new ParameterAlreadySetException(name);
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

		static void PrintUsage()
		{
			Console.WriteLine("Usage: fly -l <language> -f <path1;path2;..> -r <reference1;reference2;..> -p <MethodPath> -n <MethodName> [--debug] [--exe] [--memory] [-v <0-2>] [-w <0-4>] [--treat]");
		}
	}
}
