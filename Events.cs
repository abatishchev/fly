// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Reflection;

namespace OnTheFlyCompiler.Events
{
	public class BuildFailureEventArgs : EventArgs
	{
		public BuildFailureEventArgs()
			: this(0) { }

		public BuildFailureEventArgs(int count)
		{
			this.ErrorsCount = count;
		}

		public int ErrorsCount { get; private set; }
	}

	public class BuildStartEventArgs : System.ComponentModel.CancelEventArgs
	{
		public BuildStartEventArgs() { }
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
