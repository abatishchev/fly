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

		#region Public Methods
		#region Error
		internal void AddError(string value, int verbose)
		{
			Add(new OutputItem(OutputItemType.Error, value), verbose);
		}

		internal void AddError(int verbose)
		{
			Add(new OutputItem(OutputItemType.Error, String.Empty), verbose);
		}
		#endregion

		#region Information
		internal void AddInformation(string value, int verbose)
		{
			Add(new OutputItem(OutputItemType.Information, value), verbose);
		}

		internal void AddInformation(int verbose)
		{
			Add(new OutputItem(OutputItemType.Information, String.Empty), verbose);
		}
		#endregion

		#region Warning
		internal void AddWarning(string value, int verbose)
		{
			Add(new OutputItem(OutputItemType.Warning, value), verbose);
		}

		internal void AddWarning(int verbose)
		{
			Add(new OutputItem(OutputItemType.Warning, String.Empty), verbose);
		}
		#endregion

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

		#region Private Methods
		private void Add(int verbose)
		{
			Add(String.Empty, verbose);
		}

		private void Add(OutputItem item, int verbose)
		{
			if (this.compiler.Settings.VerboseLevel >= verbose)
			{
				this.container.Add(item);
			}
		}

		private void Add(string value, int verbose)
		{
			Add(new OutputItem(value), verbose);
		}

		private void Add(OutputItemType type, string value, int verbose)
		{
			Add(new OutputItem(type, value), verbose);
		}
		#endregion
	}
}
