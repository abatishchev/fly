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

		#region Constructors
		public Compiler(CompilerSettings settings)
		{
			this.Output = new CompilerOutput(this);
			try
			{
				this.Settings = settings;
				if (!CodeDomProvider.IsDefinedLanguage(settings.Language))
				{
					throw new LanguageNotSupportedException(settings.Language);
				}
				this.provider = CodeDomProvider.CreateProvider(settings.Language);
			}
			catch (Exception ex)
			{
				this.Output.AddError(ex.Message, 0);
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
		public CompilerSettings Settings { get; private set; }

		public CompilerOutput Output { get; private set; }

		public Assembly ResultAssembly { get; private set; }

		public object ResultObject { get; private set; }
		#endregion

		#region Methods
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				// free managed resources
				this.Settings.TempFiles.Delete();
			}
			// free native resources
		}

		public void Execute()
		{
			this.Output.AddInformation(String.Format("Executing {0}.{1}", this.Settings.MethodPath, this.Settings.MethodName), 1);
			try
			{
				var flags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;
				var type = this.ResultAssembly.GetType(this.Settings.MethodPath);
				this.ResultObject = type.InvokeMember(this.Settings.MethodName, flags, null, null, null, CultureInfo.CurrentCulture);
			}
			catch (Exception ex)
			{
				throw new ExecutionFailureException(ex);
			}
		}

		public void Compile()
		{
			Compile(this.Settings.Sources);
		}

		public void Compile(System.Collections.ICollection sources)
		{
			if (this.Settings == null || this.provider == null)
			{
				OnBuildFailure(new BuildFailureEventArgs(this.Output));
				throw new BuildFailureException(new ArgumentNullException());
			}

			this.Output.AddInformation(String.Format(CultureInfo.CurrentCulture, "Build started at {0}", DateTime.Now), 1);
			var buildStart = new BuildStartEventArgs();
			OnBuildStart(buildStart);
			if (buildStart.Cancel)
			{
				this.Output.AddInformation(String.Format(CultureInfo.CurrentCulture, "Build canceled at {0}", DateTime.Now), 1);
				throw new BuildCanceledException();
			}

			var result = this.provider.CompileAssemblyFromSource(this.Settings, (string[])(new System.Collections.ArrayList(sources).ToArray(typeof(string))));
			if (!result.Errors.HasErrors)
			{
				this.ResultAssembly = result.CompiledAssembly;
				this.Output.AddInformation(String.Format(CultureInfo.CurrentCulture, "Build succeeded at {0}", DateTime.Now), 1);
				OnBuildSuccess(new BuildSuccessEventArgs(result.CompiledAssembly));
			}
			else
			{
				if (this.Settings.VerboseLevel == 2)
				{
					foreach (CompilerError err in result.Errors)
					{
						this.Output.AddError(String.Format(CultureInfo.CurrentCulture, "{0}({1},{2}): Error {3}: {4}", err.FileName, err.Line, err.Column, err.ErrorNumber, err.ErrorText), 2);
					}
				}
				this.Output.AddInformation(String.Format(CultureInfo.CurrentCulture, "Build failed at {0} -- {1} errors", DateTime.Now, result.Errors.Count), 1);
				OnBuildFailure(new BuildFailureEventArgs(this.Output, result.Errors.Count));
				throw new BuildFailureException(); // or do not throw?
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
