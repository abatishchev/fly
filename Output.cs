// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.ObjectModel;
using System.Text;

namespace OnTheFlyCompiler
{
	public class CompilerOutput
	{
		private Compiler compiler;
		private Collection<OutputItem> container = new Collection<OutputItem>();

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

		internal void Add(OutputItem item, int verbose)
		{
			if (this.compiler.Settings.VerboseLevel >= verbose)
			{
				this.container.Add(item);
			}
		}

		internal void Add(string value, int verbose)
		{
			if (this.compiler.Settings.VerboseLevel >= verbose)
			{
				this.container.Add(new OutputItem(value));
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder(this.container.Count);
			foreach (OutputItem item in this.container)
			{
				sb.AppendLine(item.Value);
			}
			return sb.ToString();
		}
		#endregion
	}
}
