// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Reflection;

namespace OnTheFly.Events
{
	public class BuildFailureEventArgs : EventArgs
	{
		CompilerOutput output;
		int count;

		public BuildFailureEventArgs(CompilerOutput output) : this(output, 0)
		{
			//
		}

		public BuildFailureEventArgs(CompilerOutput output, int count)
		{
			this.output = output;
			this.count = count;
		}

		public CompilerOutput CompilerOutput
		{
			get
			{
				return output;
			}
		}

		public int ErrorsCount
		{
			get
			{
				return this.count;
			}
		}
	}

	public class BuildStartEventArgs : EventArgs
	{
		bool cancel;

		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}
	}

	public class BuildSuccessEventArgs : EventArgs
	{
		Assembly resultAsm;

		public BuildSuccessEventArgs(Assembly resultAsm)
		{
			this.resultAsm = resultAsm;
		}

		public Assembly ResultAssembly
		{
			get
			{
				return this.resultAsm;
			}
		}
	}
}
