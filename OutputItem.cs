using System;

namespace OnTheFlyCompiler
{
	public enum OutputItemType
	{
		Information,
		Warning,
		Error
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
