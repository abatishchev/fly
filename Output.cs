// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.ObjectModel;
using System.Text;

namespace OnTheFlyCompiler
{
	public class CompilerOutput
	{
		private Compiler compiler;
		private Collection<String> output = new Collection<string>();

		#region Constructors
		public CompilerOutput(Compiler compiler)
		{
			this.compiler = compiler;
		}
		#endregion

		#region Methods
		internal void Add(int verbose)
		{
			Add(String.Empty, verbose);
		}

		internal void Add(string value, int verbose)
		{
			if (this.compiler.Settings.Verbose >= verbose)
			{
				this.output.Add(value);
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(this.output.Count);
			foreach (string str in this.output)
			{
				sb.AppendLine(str);
			}
			return sb.ToString();
		}
		#endregion
	}
}
