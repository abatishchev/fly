// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.ObjectModel;
using System.Text;

namespace OnTheFly
{
	public class CompilerOutput : Collection<String>
	{
		Compiler compiler;

		#region Constructors
		public CompilerOutput(Compiler compiler)
		{
			this.compiler = compiler;
		}
		#endregion

		#region Methods
		public void Add(string value, int verboseLevel)
		{
			if (this.compiler.Settings.Verbose >= verboseLevel)
			{
				this.Add(value);
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(this.Count);
			foreach (string str in this)
			{
				sb.AppendLine(str);
			}
			return sb.ToString();
		}
		#endregion
	}
}
