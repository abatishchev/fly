﻿// Copyright (C) 2008-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Reflection;
using System.Windows.Forms;

using OnTheFlyCompiler.Settings;

namespace OnTheFlyCompiler
{
	static class Core
	{
		private static string copyright, title;
		private static Compiler compiler;

		#region Properties
		internal static string ApplicationCopyright
		{
			get
			{
				if (copyright == null)
				{
					try
					{
						var customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
						if (customAttributes != null && customAttributes.Length > 0)
						{
							copyright = ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
						}
					}
					catch
					{
						copyright = String.Empty;
					}
				}
				return copyright;
			}
		}

		internal static string ApplicationTitle
		{
			get
			{
				if (title == null)
				{
					try
					{
						var customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
						if (customAttributes != null && customAttributes.Length > 0)
						{
							title = ((AssemblyTitleAttribute)customAttributes[0]).Title;
						}
					}
					catch
					{
						title = String.Empty;
					}
				}
				return title;
			}
		}

		internal static string ApplicationVersion
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
