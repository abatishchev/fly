// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;

namespace OnTheFlyCompiler
{
	[Flags]
	public enum OutputItemType
	{
		Information = 1 << 1,
		Warning = 1 << 2,
		Error = 1 << 3,

		All = Information | Warning | Error
	}

	public class OutputItem
	{
		#region Constructors
		public OutputItem(string value) : this(OutputItemType.Information, value) { }

		public OutputItem(OutputItemType type, string value)
		{
			this.Type = type;
			this.Value = value;
		}
		#endregion

		#region Properties
		public OutputItemType Type { get; private set; }

		public string Value { get; private set; }
		#endregion

		#region Methods
		#endregion
	}
}
