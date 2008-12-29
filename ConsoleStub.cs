// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Globalization;

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
				Core.Compiler.Dispose();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error! {0}", ex.Message);
				return;
			}
		}

		static CompilerSettings Parse(string[] args)
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
									System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
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
