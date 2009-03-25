// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;

namespace OnTheFlyCompiler
{
	static class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Console.WriteLine("{0} version {1}", Core.ApplicationTitle, Core.ApplicationVersion);
			Console.WriteLine(Core.ApplicationCopyright);
			Console.WriteLine();

			if (args.Length == 0)
			{
				PrintUsage();
				return;
			}
			try
			{
				Core.Init(CompilerSettingsHelper.Parse(args));
				using (Core.Compiler)
				{
					Core.Compiler.Compile();
					if (Core.Compiler.Settings.Execute)
					{
						Core.Compiler.Execute();
					}
				}
			}
			catch (OnTheFlyCompiler.Errors.CompilerException ex)
			{
				Console.WriteLine("Compiler error:");
				Console.WriteLine(ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:");
				Console.WriteLine(ex.Message);
				return;
			}
			finally
			{
				Console.WriteLine(Core.Compiler.Output);
			}
		}

		private static void PrintUsage()
		{
			Console.WriteLine("Usage: fly -l <language> -f <path1;path2;..> -r <reference1;reference2;..> -p <MethodPath> -n <MethodName> [--debug] [--exe | dll] [--memory | disk] [-v <0-2>] [-w <0-4>] [--treat] | -x <XmlConfiguration>");
		}
	}
}
