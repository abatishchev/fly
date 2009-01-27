// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.Specialized;

namespace OnTheFlyCompiler.Settings
{
	public class CompilerSettings : System.CodeDom.Compiler.CompilerParameters
	{
		string language, methodName, methodPath;
		int verbose = 2;
		bool execute;
		StringCollection sources = new StringCollection();

		#region Constructors
		#endregion

		#region Properties
		public bool Execute
		{
			get
			{
				return this.execute;
			}
			set
			{
				this.execute = value;
			}
		}
		public string Language
		{
			get
			{
				return this.language;
			}
			set
			{
				this.language = value;
			}
		}

		public string MethodName
		{
			get
			{
				return this.methodName;
			}
			set
			{
				this.methodName = value;
			}
		}


		public string MethodPath
		{
			get
			{
				return this.methodPath;
			}
			set
			{
				this.methodPath = value;
			}
		}

		public StringCollection Sources
		{
			get
			{
				return this.sources;
			}
			set
			{
				this.sources = value;
			}
		}

		public int Verbose
		{
			get
			{
				return this.verbose;
			}
			set
			{
				this.verbose = value;
			}
		}
		#endregion
	}
}
