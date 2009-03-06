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
		public OutputItem(string value)
		{
			this.Value = value;
		}
		#endregion

		#region Properties
		public string Value { get; private set; }
		#endregion

		#region Methods
		#endregion
	}
}
