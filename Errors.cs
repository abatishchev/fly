// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Globalization;

namespace OnTheFlyCompiler.Errors
{
	#region Compiler
	[Serializable]
	public abstract class CompilerException : Exception
	{
		public CompilerException(string message)
			: base(message) { }

		public CompilerOutput CompilerOutput { get; protected set; }
	}

	[Serializable]
	public class BuildCanceledException : CompilerException
	{
		public BuildCanceledException(CompilerOutput output)
			: base("Build was canceled")
		{
			this.CompilerOutput = output;
		}
	}

	[Serializable]
	public class BuildFailureException : CompilerException
	{
		public BuildFailureException(CompilerOutput output)
			: base("Build failed")
		{
			this.CompilerOutput = output;
		}
	}

	[Serializable]
	public class ExecutionFailureException : CompilerException
	{
		public ExecutionFailureException(CompilerOutput output)
			: base("Execution failed")
		{
			this.CompilerOutput = output;
		}
	}

	[Serializable]
	public class LanguageNotSupportedException : Exception
	{
		public LanguageNotSupportedException(string language)
			: base(String.Format(CultureInfo.CurrentCulture, "Specified language is not supported: '{0}'", language)) { }
	}

	[Serializable]
	public class XmlDescriptionException : Exception
	{
		public XmlDescriptionException(Exception ex)
			: base(String.Format(CultureInfo.CurrentCulture, "Error while reading Xml-description: {0}", ex.Message), ex) { }
	}
	#endregion

	#region Parameters
	[Serializable]
	public abstract class ParameterException : Exception
	{
		#region Constructors
		protected ParameterException(string message, string name)
			: base(message)
		{
			this.ParameterName = name;
		}

		protected ParameterException(string message, string name, string value)
			: base(message)
		{
			this.ParameterName = name;
			this.ParameterValue = value;
		}
		#endregion

		#region Properties
		public string ParameterName { get; protected set; }

		public string ParameterValue { get; protected set; }
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
}
