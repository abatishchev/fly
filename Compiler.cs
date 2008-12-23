// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;

using Microsoft.CSharp;

using OnTheFly.Errors;
using OnTheFly.Events;
using OnTheFly.Settings;

namespace OnTheFly
{
	public class Compiler
	{
		CodeDomProvider provider;
		CompilerSettings settings;
		CompilerOutput output;

		CompilerResults result;
		object resultObj;

		bool isReady = false;

		#region Constructor
		public Compiler(CompilerSettings settings)
		{
			this.output = new CompilerOutput(this);
			try
			{
				settings.Check();
				this.settings = settings;
				if (!CodeDomProvider.IsDefinedLanguage(settings.Language))
				{
					throw new LanguageNotSupportedException(settings.Language);
				}
				this.provider = CodeDomProvider.CreateProvider(settings.Language);
				this.isReady = true;
			}
			catch (Exception ex)
			{
				this.output.Add(ex.Message, 0);
				return;
			}
		}
		#endregion

		#region Events
		public event EventHandler<BuildFailureEventArgs> OnBuildFailure;
		public event EventHandler<BuildStartEventArgs> OnBuildStart;
		public event EventHandler<BuildSuccessEventArgs> OnBuildSuccess;
		#endregion

		#region Properties
		public bool IsReady
		{
			get
			{
				return this.isReady;
			}
		}

		public CompilerSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		public CompilerOutput Output
		{
			get
			{
				return this.output;
			}
		}

		public Assembly ResultAssembly
		{
			get
			{
				return this.result.CompiledAssembly;
			}
		}

		public object ResultObject
		{
			get
			{
				return this.resultObj;
			}
		}
		#endregion

		#region Methods
		public void Execute()
		{
			BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;
			Type type = this.ResultAssembly.GetType(settings.MethodPath);
			this.resultObj = type.InvokeMember(settings.MethodName, flags, null, null, null, CultureInfo.CurrentCulture);
		}

		public void Clean()
		{
			this.result.TempFiles.Delete();
		}

		public void Compile()
		{
			string[] sources = new string[settings.Sources.Count];
			for (int i = 0; i < settings.Sources.Count; i++)
			{
				sources[i] = settings.Sources[i];
			}
			Compile(sources);
		}

		public void Compile(string[] sources)
		{
			if (!isReady)
			{
				output.Add(new CompilerNotReadyException().Message, 1);
				if (this.OnBuildFailure != null)
				{
					this.OnBuildFailure(this, new BuildFailureEventArgs(this.output));
				}
				return;
			}

			output.Add(String.Format(CultureInfo.CurrentCulture, "Build started at {0}", DateTime.Now), 1);
			if (this.OnBuildStart != null)
			{
				BuildStartEventArgs compilationStartArgs = new BuildStartEventArgs();
				this.OnBuildStart(this, compilationStartArgs);
				if (compilationStartArgs.Cancel)
				{
					this.output.Add(String.Format(CultureInfo.CurrentCulture, "Build canceled at {0}", DateTime.Now), 1);
					return;
				}
			}
			this.result = this.provider.CompileAssemblyFromSource(this.settings, sources);
			if (result.Errors.HasErrors)
			{
				foreach (CompilerError err in result.Errors)
				{
					this.output.Add(String.Format(CultureInfo.CurrentCulture, "{0}({1}.{2}): Error {3} - {4}", err.FileName, err.Line, err.Column, err.ErrorNumber, err.ErrorText), 2);
				}
				this.output.Add(String.Format(CultureInfo.CurrentCulture, "Build failed at {0} -- {1} errors", DateTime.Now, result.Errors.Count), 1);

				if (this.OnBuildFailure != null)
				{
					this.OnBuildFailure(this, new BuildFailureEventArgs(output, result.Errors.Count));
				}
				return;
			}
			else
			{
				this.output.Add(String.Format(CultureInfo.CurrentCulture, "Build succeeded at {0}", DateTime.Now), 1);
			}
			if (this.OnBuildSuccess != null)
			{
				this.OnBuildSuccess(this, new BuildSuccessEventArgs(this.ResultAssembly));
			}
		}
		#endregion
	}
}
