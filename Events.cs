// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Reflection;

namespace OnTheFlyCompiler.Events
{
	public class BuildFailureEventArgs : EventArgs
	{
		public BuildFailureEventArgs(CompilerOutput output) : this(output, 0)
		{
			//
		}

		public BuildFailureEventArgs(CompilerOutput output, int count)
		{
			this.CompilerOutput = output;
			this.ErrorsCount = count;
		}

		public CompilerOutput CompilerOutput { get; private set;  }

		public int ErrorsCount { get; private set;  }
	}

	public class BuildStartEventArgs : EventArgs
	{
		public bool Cancel { get; set; }
	}

	public class BuildSuccessEventArgs : EventArgs
	{
		public BuildSuccessEventArgs(Assembly resultAsm)
		{
			this.ResultAssembly = resultAsm;
		}

		public Assembly ResultAssembly { get; private set; }
	}
}
