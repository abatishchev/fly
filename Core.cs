// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Reflection;
using System.Windows.Forms;

using OnTheFlyCompiler.Settings;

namespace OnTheFlyCompiler
{
	static class Core
	{
		static string copyright, title;
		static Compiler compiler;

		#region Properties
		public static string ApplicationCopyright
		{
			get
			{
				if (String.IsNullOrEmpty(copyright))
				{
					object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						copyright = ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
					}
				}
				return copyright;
			}
		}

		public static string ApplicationName
		{
			get
			{
				return Application.ProductName;
			}
		}

		public static string ApplicationTitle
		{
			get
			{
				if (String.IsNullOrEmpty(title))
				{
					object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						title = ((AssemblyTitleAttribute)customAttributes[0]).Title;
					}
				}
				return title;
			}
		}

		public static string ApplicationVersion
		{
			get
			{
				return Application.ProductVersion;
			}
		}

		internal static Compiler Compiler
		{
			get
			{
				return compiler;
			}
		}
		#endregion

		#region Methods
		internal static void Init(CompilerSettings settings)
		{
			compiler = new Compiler(settings);
		}
		#endregion
	}
}
