// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.Specialized;

namespace OnTheFlyCompiler.Settings
{
	public class CompilerSettings : System.CodeDom.Compiler.CompilerParameters
	{
		#region Constructors
		public CompilerSettings()
		{
			this.Sources = new StringCollection();
		}
		#endregion

		#region Properties
		public bool Execute { get; set; }

		public string Language { get; set; }

		public string MethodName { get; set; }

		public string MethodPath { get; set; }

		public StringCollection Sources { get; set; }

		public int Verbose { get; set; }
		#endregion
	}
}
