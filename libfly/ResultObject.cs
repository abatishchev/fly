// Copyright (C) 2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;

namespace OnTheFlyCompiler
{
	public class CompilerResultObject
	{
		#region Constructors
		public CompilerResultObject(object instance)
		{
			this.Instance = instance;
		}
		#endregion

		#region Properties
		public object Instance { get; private set; }
		#endregion
	}
}
