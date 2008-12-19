// Copyright (C) 2007 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace OnTheFlyCompiler
{
	public class OnTheFlyCompiler
	{
		CodeDomProvider provider;
		CompilerParameters options = new CompilerParameters();
		CompilerResults result;

		bool isCompiled, isInitialized, isLanguageSet;
		int errorsCount;

		CompilerLanguage eLanguage;

		List<string> listError;
		List<string> referencesList = new List<string>();

		#region Constructor
		public OnTheFlyCompiler(CompilerLanguage language)
		{
			SetLanguage(language);
			InitializeComplier();
		}
		#endregion

		#region Enums
		public enum CompilerLanguage
		{
			CSharp,
			VisualBasic
		}
		#endregion

		#region Properties
		public int ErrorsCount
		{
			get
			{
				return this.errorsCount;
			}
		}

		public List<string> ErrorsList
		{
			get
			{
				return this.listError;
			}
		}

		public bool IsInitialized
		{
			get
			{
				return this.isInitialized;
			}
		}

		public bool IsCompiled
		{
			get
			{
				return this.isCompiled;
			}
		}

		public CompilerLanguage Language
		{
			get
			{
				return eLanguage;
			}
		}
		#endregion

		#region Methods
		public void InitializeComplier()
		{
			try
			{
				if (!isLanguageSet)
				{
					throw new OnTheFlyCompilerException("Compiler language was not provided", this);
				}

				options.GenerateInMemory = true;
				options.TreatWarningsAsErrors = false;
				options.WarningLevel = 4;
				isInitialized = true;
			}
			catch (Exception ex)
			{
				ReturnErrorMessage(ex);
				return;
			}
		}

		public Assembly CompileFile(string FilePath)
		{
			StreamReader reader = new StreamReader(FilePath);
			return Compile(reader.ReadToEnd());
		}

		public Assembly Compile(string Source)
		{
			string strSource = Source;
			try
			{
				if (strSource == "")
				{
					throw new OnTheFlyCompilerException("Source code was not provided", this);
				}
				if (!isInitialized)
				{
					throw new OnTheFlyCompilerException("Compiler was not initialized correctly", this);
				}
				foreach (string reference in referencesList)
				{
					options.ReferencedAssemblies.Add(reference);
				}
				result = provider.CompileAssemblyFromSource(options, strSource);
				errorsCount = result.Errors.Count;
				if (errorsCount > 0)
				{
					listError = new List<string>();
					foreach (CompilerError err in result.Errors)
					{
						listError.Add(String.Format("Line {0}, Col {1}: Error {2} - {3}", err.Line, err.Column, err.ErrorNumber, err.ErrorText));
					}
				}
				else
				{
					isCompiled = true;
					return result.CompiledAssembly;
				}
			}
			catch (Exception ex)
			{
				ReturnErrorMessage(ex);
			}
			return null;
		}

		private void SetLanguage(CompilerLanguage language)
		{
			referencesList.Add("System.dll");
			switch (language)
			{
				case CompilerLanguage.CSharp:
					{
						provider = new Microsoft.CSharp.CSharpCodeProvider();
						break;
					}
				case CompilerLanguage.VisualBasic:
					{
						referencesList.Add("Microsoft.VisualBasic.dll");
						provider = new Microsoft.VisualBasic.VBCodeProvider();
						break;
					}
			}
			eLanguage = language;
			isLanguageSet = true;
		}

		public object Run(Assembly Assembly, string MethodPath, string Method, object[] args)
		{
			return RunAssembly(Assembly, MethodPath, Method, args);
		}

		public object Run(Assembly Assembly, string MethodPath, string Method)
		{
			return RunAssembly(Assembly, MethodPath, Method, null);
		}

		/*
		public object Run(Assembly Assembly, string MethodPath, string Method, NCHLOE.Core.CCHLOECore Core)
		{
			object[] args = { Core };
			return RunAssembly(Assembly, MethodPath, Method, args);
		}
		*/

		object RunAssembly(Assembly Assembly, string MethodPath, string Method, object[] args)
		{
			if (!isCompiled)
			{
				throw new OnTheFlyCompilerException("No compiled assembly was provided.", this);
			}
			Type type = Assembly.GetType(MethodPath);
			object obj = null;
			if (type != null)
			{
				obj = type.InvokeMember(
					Method,
					BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
					null,
					null,
					args
				);
			}
			if (obj != null)
			{
				return obj;
			}
			return null;
		}

		void ReturnErrorMessage(Exception ex)
		{
			listError = new List<string>();
			listError.Add(ex.ToString());
		}
		#endregion
	}
}