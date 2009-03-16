// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;

namespace OnTheFlyCompiler
{
	static class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			//OutputItemType all = OutputItemType.All;
			//OutputItemType err = OutputItemType.Error;
			//OutputItemType info = OutputItemType.Information;
			//OutputItemType warn = OutputItemType.Warning;

			//OutputItemType r = all & warn;

			Console.WriteLine(String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0} version {1}", Core.ApplicationTitle, Core.ApplicationVersion));
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
					Console.WriteLine(Core.Compiler.Output);
				}
			}
			catch (OnTheFlyCompiler.Errors.CompilerException ex)
			{
				Console.WriteLine("Compiler error:");
				Console.WriteLine(ex.CompilerOutput);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error! {0}", ex.Message);
				return;
			}
		}

		private static void PrintUsage()
		{
			Console.WriteLine("Usage: fly -l <language> -f <path1;path2;..> -r <reference1;reference2;..> -p <MethodPath> -n <MethodName> [--debug] [--exe] [--memory] [-v <0-2>] [-w <0-4>] [--treat] | -x <XmlConfiguration>");
		}
	}
}
