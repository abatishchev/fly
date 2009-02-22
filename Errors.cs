// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Globalization;

namespace OnTheFlyCompiler.Errors
{
	#region Compiler
	[Serializable]
	public class CompilerException : Exception
	{
		public CompilerException()
			: base() { }

		//TODO: message
		public CompilerException(string message)
			: base(message) { }
	}

	[Serializable]
	public class LanguageNotSupportedException : Exception
	{
		public LanguageNotSupportedException(string language)
			: base(String.Format(CultureInfo.CurrentCulture, "Specified language is not supported: '{0}'", language)) { }
	}
	#endregion

	#region Parameters
	[Serializable]
	public abstract class ParameterException : Exception
	{
		private string parameterName, parameterValue;

		#region Constructors
		protected ParameterException(string message, string name)
			: base(message)
		{
			this.parameterName = name;
		}

		protected ParameterException(string message, string name, string value)
			: base(message)
		{
			this.parameterName = name;
			this.parameterValue = value;
		}
		#endregion

		#region Properties
		public string ParameterName
		{
			get
			{
				return this.parameterName;
			}
			protected set
			{
				this.parameterName = value;
			}
		}

		public string ParameterValue
		{
			get
			{
				return this.parameterValue;
			}
			protected set
			{
				this.parameterValue = value;
			}
		}
		#endregion
	}

	[Serializable]
	public class ParameterAlreadySetException : ParameterException
	{
		public ParameterAlreadySetException(string name)
			: base(String.Format(CultureInfo.CurrentCulture, "Parameter '{0}' is already set", name), name) { }
	}

	[Serializable]
	public class ParameterMissedException : ParameterException
	{
		public ParameterMissedException(string name)
			: base(String.Format(CultureInfo.CurrentCulture, "Required parameter '{0}' is missed", name), name) { }
	}

	[Serializable]
	public class ParameterNotSetException : ParameterException
	{
		public ParameterNotSetException(string name)
			: base(String.Format(CultureInfo.CurrentCulture, "Required parameter '{0}' must have a value", name), name) { }
	}

	[Serializable]
	public class ParameterOutOfRangeException : ParameterException
	{
		public ParameterOutOfRangeException(string name, string value)
			: base(String.Format(CultureInfo.CurrentCulture, "Wrong value for parameter '{0}': '{1}'", name, value), name, value) { }
	}

	[Serializable]
	public class UnknownParameterException : ParameterException
	{
		public UnknownParameterException(string name)
			: base(String.Format(CultureInfo.CurrentCulture, "Uknown parameter was specified: '{0}'", name), name) { }
	}
	#endregion

	[Serializable]
	public class XmlDescriptionException : Exception
	{
		public XmlDescriptionException(Exception ex)
			: base(String.Format(CultureInfo.CurrentCulture, "Error while reading Xml-description: {0}", ex.Message), ex) { }
	}
}
