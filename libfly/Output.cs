// Copyright (C) 2008-2010 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.ObjectModel;
using System.Text;

using OnTheFlyCompiler.Settings;

namespace OnTheFlyCompiler
{
	public class CompilerOutput
	{
		#region Fields
		private Compiler compiler;
		private Collection<OutputItem> container = new Collection<OutputItem>();
		#endregion

		#region Constructors
		public CompilerOutput(Compiler compiler)
		{
			this.compiler = compiler;
		}
		#endregion

		#region Public Methods
		#region Error
		internal void AddError(string value)
		{
			Add(value, CompilerSettings.MaxVerboseLevel);
		}
		
		internal void AddError(int verbose)
		{
			Add(String.Empty, verbose);
		}

		internal void AddError(string value, int verbose)
		{
			Add(new OutputItem(OutputItemType.Error, value), verbose);
		}
		#endregion

		#region Information
		internal void AddInformation(string value)
		{
			AddInformation(value, CompilerSettings.MaxVerboseLevel);
		}

		internal void AddInformation(int verbose)
		{
			Add(String.Empty, verbose);
		}

		internal void AddInformation(string value, int verbose)
		{
			Add(new OutputItem(OutputItemType.Information, value), verbose);
		}
		#endregion

		#region Warning
		internal void AddWarning(string value)
		{
			Add(value, CompilerSettings.MaxVerboseLevel);
		}

		internal void AddWarning(int verbose)
		{
			Add(String.Empty, verbose);
		}

		internal void AddWarning(string value, int verbose)
		{
			Add(new OutputItem(OutputItemType.Warning, value), verbose);
		}
		#endregion

		public override string ToString()
		{
			return ToString(OutputItemType.All);
		}

		public string ToString(OutputItemType type)
		{
			var sb = new StringBuilder(this.container.Count);
			foreach (var item in this.container)
			{
				if ((type & item.Type) == item.Type)
				{
					sb.AppendLine(item.Value);
				}
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
