// Copyright (C) 2008-2010 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Reflection;
using System.Windows.Forms;

using OnTheFlyCompiler.Settings;

namespace OnTheFlyCompiler
{
	static class Core
	{
		#region Properties
		private static string copyright;
		public static string ApplicationCopyright
		{
			get
			{
				if (copyright == null)
				{
					try
					{
						copyright = ((AssemblyCopyrightAttribute)Assembly
							.GetEntryAssembly()
							.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0])
								.Copyright;
					}
					catch
					{
						copyright = String.Empty;
					}
				}
				return copyright;
			}
		}

		public static string ApplicationName
		{
			get
			{
				return System.Windows.Forms.Application.ProductName;
			}
		}

		public static string ApplicationVersion
		{
			get
			{
				return System.Windows.Forms.Application.ProductVersion;
			}
		}

		private static string title;
		public static string ApplicationTitle
		{
			get
			{
				if (title == null)
				{
					try
					{
						title = ((AssemblyTitleAttribute)Assembly
							.GetEntryAssembly()
							.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0])
								.Title;
					}
					catch
					{
						title = String.Empty;
					}
				}
				return title;
			}
		}

		internal static Compiler Compiler { get; private set; }
		#endregion

		#region Methods
		public static void Init(CompilerSettings settings)
		{
			Compiler = new Compiler(settings);
		}
		#endregion
	}
}
