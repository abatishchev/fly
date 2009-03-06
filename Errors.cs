// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Globalization;

namespace OnTheFlyCompiler.Errors
{
	#region Compiler
	[Serializable]
	public abstract class CompilerException : Exception
	{
		public CompilerException(CompilerOutput output, string message)
			: base(message)
		{
			this.CompilerOutput = output;
		}

		public CompilerOutput CompilerOutput { get; private set; }

		//TODO: serialization
	}

	[Serializable]
	public class BuildCanceledException : CompilerException
	{
		public BuildCanceledException(CompilerOutput output)
			: base(output, "Build canceled") { }
	}

	[Serializable]
	public class BuildFailureException : CompilerException
	{
		public BuildFailureException(CompilerOutput output)
			: base(output, "Build failed") { }
	}

	[Serializable]
	public class ExecutionFailureException : CompilerException
	{
		public ExecutionFailureException(CompilerOutput output)
			: base(output, "Execution failed") { }
	}

	[Serializable]
	public class LanguageNotSupportedException : Exception
	{
		public LanguageNotSupportedException(string language)
			: base(String.Format(CultureInfo.CurrentCulture, "Specified language '{0}' is not supported", language))
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
			: base(String.Format(CultureInfo.CurrentCulture, "Error while reading Xml-description: {0}", ex.Message), ex) { }
	}
	#endregion

	#region Parameters
	[Serializable]
	public class ParameterMissedException : ArgumentNullException
	{
		public ParameterMissedException(string name)
			: base(String.Format(CultureInfo.CurrentCulture, "Required parameter '{0}' was not specified", name), name) { }
	}

	[Serializable]
	public class ParameterNotSetException : ArgumentNullException
	{
		public ParameterNotSetException(string name)
			: base(String.Format(CultureInfo.CurrentCulture, "Required parameter '{0}' must have a value", name), name) { }
	}

	[Serializable]
	public class ParameterOutOfRangeException : ArgumentOutOfRangeException
	{
		public ParameterOutOfRangeException(string name, string value)
			: base(name, value, String.Format(CultureInfo.CurrentCulture, "Wrong value for parameter '{0}': '{1}'", name, value)) { }
	}

	[Serializable]
	public class UnknownParameterException : ArgumentException
	{
		public UnknownParameterException(string name)
			: base(String.Format(CultureInfo.CurrentCulture, "Unknown parameter was specified: '{0}'", name), name) { }
	}
	#endregion
}
