// Copyright (C) 2007 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.Generic;
using System.Text;

namespace OnTheFlyCompiler
{
	public class OnTheFlyCompilerException : Exception
	{
		OnTheFlyCompiler fly;
		string strMessage;

		public OnTheFlyCompilerException(string msg, OnTheFlyCompiler parrentInstance)
		{
			strMessage = msg;
			fly = parrentInstance;
		}

		public override string Message
		{
			get
			{
				return strMessage;
			}
		}

		public int ErrorsCount
		{
			get
			{
				return fly.ErrorsCount;
			}
		}

		public string BuildResult
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				foreach (string str in fly.ErrorsList)
				{
					builder.Append(str);
				}
				return builder.ToString();
			}
		}
	}
}
