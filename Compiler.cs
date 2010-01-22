// Copyright (C) 2007-2010 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

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

		public CompilerResultObject ResultObject { get; private set; }
		#endregion

		#region Methods
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// free managed resources
				this.Settings.TempFiles.Delete();
				this.provider.Dispose();
			}
			// free native resources
		}

		public object ExecuteStatic()
		{
			var fullPath = String.Format(CultureInfo.CurrentCulture, "{0}.{1}", this.Settings.MethodPath, this.Settings.MethodName);
			this.Output.AddInformation(String.Format("Executing {0}", fullPath));
			try
			{
				Type type = null;
				if (Settings.AppDomainCreation)
				{
					var appDomain = AppDomain.CreateDomain(this.Settings.AppDomainName != null ? this.Settings.AppDomainName : "OnTheFlyCompiler", null, null);
					this.ResultObject = appDomain.CreateInstanceFromAndUnwrap(this.ResultAssembly.Location, fullPath) as CompilerResultObject;
					if (this.ResultObject != null)
					{
						type = this.ResultObject.GetType();
					}
				}
				else
				{
					type = this.ResultAssembly.GetType(this.Settings.MethodPath);
				}
				if (type != null)
				{
					if (this.Settings.BindingFlag == BindingFlags.Default)
					{
						this.Settings.BindingFlag = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;
					}
					return type.InvokeMember(this.Settings.MethodName, this.Settings.BindingFlag, null, null, null, CultureInfo.CurrentCulture);
				}
				else
				{
					throw new ExecutionFailureException();
				}
			}
			catch (Exception ex)
			{
				this.Output.AddInformation(new ExecutionFailureException(ex).Message);
				this.Output.AddError(String.Format(CultureInfo.CurrentCulture, "Error: {0}", ex.Message));
				return null;
			}
		}

		public object ExecuteStatic<T>() where T : class
		{
			return ExecuteStatic() as T;
		}

		public Assembly Compile()
		{
			return Compile(this.Settings.Sources);
		}

		public Assembly Compile(System.Collections.ICollection sources)
		{
			if (this.Settings == null)
			{
				OnBuildFailure(new BuildFailureEventArgs());
				throw new BuildFailureException(new ArgumentNullException("source"));
			}
			else if (this.provider == null)
			{
				OnBuildFailure(new BuildFailureEventArgs());
				throw new BuildFailureException(new ArgumentNullException("provider"));
			}

			this.Output.AddInformation(String.Format(CultureInfo.CurrentCulture, "Build started at {0}", DateTime.Now));
			var buildStart = new BuildStartEventArgs();
			OnBuildStart(buildStart);
			if (buildStart.Cancel)
			{
				this.Output.AddInformation(String.Format(CultureInfo.CurrentCulture, "Build canceled at {0}", DateTime.Now));
				throw new BuildCanceledException();
			}

			var result = this.provider.CompileAssemblyFromSource(this.Settings, (string[])(new System.Collections.ArrayList(sources).ToArray(typeof(string))));
			if (!result.Errors.HasErrors)
			{
				this.ResultAssembly = result.CompiledAssembly;
				this.Output.AddInformation(String.Format(CultureInfo.CurrentCulture, "Build succeeded at {0}", DateTime.Now));
				OnBuildSuccess(new BuildSuccessEventArgs(result.CompiledAssembly));
				return this.ResultAssembly;
			}
			else
			{
				foreach (CompilerError err in result.Errors)
				{
					this.Output.AddError(String.Format(CultureInfo.CurrentCulture, "{0}({1},{2}): Error {3}: {4}", err.FileName, err.Line, err.Column, err.ErrorNumber, err.ErrorText), 1);
				}
				this.Output.AddInformation(String.Format(CultureInfo.CurrentCulture, "Build failed at {0} -- {1} errors", DateTime.Now, result.Errors.Count));
				OnBuildFailure(new BuildFailureEventArgs(result.Errors.Count));
				return null;
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
