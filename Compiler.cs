// Copyright (C) 2007-2009 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;

using OnTheFlyCompiler.Errors;
using OnTheFlyCompiler.Events;
using OnTheFlyCompiler.Settings;

namespace OnTheFlyCompiler
{
	public class Compiler : IDisposable
	{
		private CodeDomProvider provider;
		private CompilerSettings settings;
		private CompilerOutput output;

		private Assembly resultAssembly;
		private object resultObj;

		#region Constructors
		public Compiler(CompilerSettings settings)
		{
			this.output = new CompilerOutput(this);
			try
			{
				this.settings = settings;
				if (!CodeDomProvider.IsDefinedLanguage(settings.Language))
				{
					throw new LanguageNotSupportedException(settings.Language);
				}
				this.provider = CodeDomProvider.CreateProvider(settings.Language);
			}
			catch (Exception ex)
			{
				this.output.Add(ex.Message, 0);
				return;
			}
		}
		#endregion

		#region Events
		public event EventHandler<BuildFailureEventArgs> BuildFailure;
		public event EventHandler<BuildStartEventArgs> BuildStart;
		public event EventHandler<BuildSuccessEventArgs> BuildSuccess;
		#endregion

		#region Properties
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
				return this.resultAssembly;
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
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			// free managed resources
			this.settings.TempFiles.Delete();
			if (disposing)
			{
				// free native resources
			}
		}

		public void Execute()
		{
			this.output.Add(String.Format("Executing {0}.{1}..", settings.MethodPath, settings.MethodName), 1);
			var flags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;

			try
			{
				var type = this.ResultAssembly.GetType(settings.MethodPath);
				this.resultObj = type.InvokeMember(settings.MethodName, flags, null, null, null, CultureInfo.CurrentCulture);
			}
			catch
			{
				throw new CompilerException(); // TODO
			}
		}

		public void Compile()
		{
			Compile(this.settings.Sources);
		}

		public void Compile(System.Collections.Specialized.StringCollection sources)
		{
			if (this.settings == null || this.provider == null)
			{
				// TODO: this.output.Add(
				OnBuildFailure(new BuildFailureEventArgs(this.Output));
				throw new CompilerException(); // TODO
			}

			this.output.Add(String.Format(CultureInfo.CurrentCulture, "Build started at {0}", DateTime.Now), 1);
			var buildStart = new BuildStartEventArgs();
			OnBuildStart(buildStart);
			if (buildStart.Cancel)
			{
				this.output.Add(String.Format(CultureInfo.CurrentCulture, "Build canceled at {0}", DateTime.Now), 1);
				throw new CompilerException(); // TODO
			}

			var result = this.provider.CompileAssemblyFromSource(this.settings, (string[])new System.Collections.ArrayList(sources).ToArray());
			if (!result.Errors.HasErrors)
			{
				this.resultAssembly = result.CompiledAssembly;
				this.output.Add(String.Format(CultureInfo.CurrentCulture, "Build succeeded at {0}", DateTime.Now), 1);
				OnBuildSuccess(new BuildSuccessEventArgs(result.CompiledAssembly));
			}
			else
			{
				foreach (CompilerError err in result.Errors)
				{
					this.output.Add(String.Format(CultureInfo.CurrentCulture, "{0}({1}.{2}): Error {3} - {4}", err.FileName, err.Line, err.Column, err.ErrorNumber, err.ErrorText), 2);
				}
				this.output.Add(String.Format(CultureInfo.CurrentCulture, "Build failed at {0} -- {1} errors", DateTime.Now, result.Errors.Count), 1);
				OnBuildFailure(new BuildFailureEventArgs(this.output, result.Errors.Count));
				throw new CompilerException(); // TODO
			}
		}

		private void OnBuildFailure(BuildFailureEventArgs e)
		{
			if (this.BuildFailure != null)
			{
				this.BuildFailure(this, e);
			}
		}

		private void OnBuildStart(BuildStartEventArgs e)
		{
			if (this.BuildStart != null)
			{
				this.BuildStart(this, e);
			}
		}

		private void OnBuildSuccess(BuildSuccessEventArgs e)
		{
			if (this.BuildSuccess != null)
			{
				this.BuildSuccess(this, e);
			}
		}
		#endregion
	}
}
