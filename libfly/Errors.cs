// Copyright (C) 2008-2010 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

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

		public CompilerException(string message, Exception innerException)
			: base(message, innerException) { }
	}

	[Serializable]
	public class BuildCanceledException : CompilerException
	{
		public BuildCanceledException()
			: base("Build canceled") { }
	}

	[Serializable]
	public class BuildFailureException : CompilerException
	{
		public BuildFailureException()
			: this(null) { }

		public BuildFailureException(Exception exception)
			: base("Build failed", exception) { }
	}

	[Serializable]
	public class ExecutionFailureException : CompilerException
	{
		public ExecutionFailureException()
			: this(null) { }

		public ExecutionFailureException(Exception exception)
			: base("Execution failed") { }
	}

	[Serializable]
	public class LanguageNotSupportedException : Exception
	{
		public LanguageNotSupportedException(string language)
			: base(String.Format(CultureInfo.InvariantCulture, "Specified language '{0}' is not supported", language))
		{
			this.Language = language;
		}

		public string Language { get; private set; }

		//TODO: serialization
	}

	[Serializable]
	public class XmlDescriptionException : Exception
	{
		public XmlDescriptionException(Exception ex)
			: base(String.Format(CultureInfo.InvariantCulture, "Error while reading Xml-description: {0}", ex.Message), ex) { }
	}
	#endregion

	#region Parameters
	[Serializable]
	public class ParameterMissedException : ArgumentNullException
	{
		public ParameterMissedException(string name)
			: base(String.Format(CultureInfo.InvariantCulture, "Required parameter '{0}' was not specified", name), name) { }
	}

	[Serializable]
	public class ParameterNotSetException : ArgumentNullException
	{
		public ParameterNotSetException(string name)
			: base(String.Format(CultureInfo.InvariantCulture, "Required parameter '{0}' must have a value", name), name) { }
	}

	[Serializable]
	public class ParameterOutOfRangeException : ArgumentOutOfRangeException
	{
		public ParameterOutOfRangeException(string name, string value)
			: base(name, value, String.Format(CultureInfo.InvariantCulture, "Wrong value for parameter '{0}': '{1}'", name, value)) { }
	}

	[Serializable]
	public class UnknownParameterException : ArgumentException
	{
		public UnknownParameterException(string name)
			: base(String.Format(CultureInfo.InvariantCulture, "Unknown parameter was specified: '{0}'", name), name) { }
	}
	#endregion

	[Serializable]
	public class TemplateException : Exception
	{
		public TemplateException(string message) : base(message) { }
	}
}
