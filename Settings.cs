// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.Specialized;
using System.Reflection;

namespace OnTheFlyCompiler.Settings
{
	public class CompilerSettings : System.CodeDom.Compiler.CompilerParameters
	{
		private int verboseLevel;

		#region Constructors
		public CompilerSettings()
		{
			this.Sources = new StringCollection();

			// default values
			this.Execute = false;
			this.GenerateExecutable = true;
			this.GenerateInMemory = true;
			this.VerboseLevel = CompilerSettings.MaxVerboseLevel;
			this.WarningLevel = 3;
		}
		#endregion

		#region Fields
		public static int MaxVerboseLevel = 2;
		#endregion

		#region Properties
		public BindingFlags BindingFlag { get; set; }

		public bool Execute { get; set; }

		public string Language { get; set; }

		public string MethodName { get; set; }

		public string MethodPath { get; set; }

		public StringCollection Sources { get; set; }

		public int VerboseLevel
		{
			get
			{
				return this.verboseLevel;
			}
			set
			{
				if (value < 0 || value > CompilerSettings.MaxVerboseLevel)
				{
					throw new Errors.ParameterOutOfRangeException("verbose", value.ToString(System.Globalization.CultureInfo.CurrentCulture));
				}
				else
				{
					this.verboseLevel = value;
				}
			}
		}
		#endregion
	}
}
