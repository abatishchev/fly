// Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Test
{
	public partial class frmMain : Form
	{
		OnTheFly.Compiler compiler = null;

		public frmMain()
		{
			InitializeComponent();
		}

		private void cmdFly_Click(object sender, EventArgs e)
		{
			string str = "-p Test -n Main -l vb -r System.Windows.Forms.dll";
			OnTheFly.Settings.CompilerSettings settings = OnTheFly.Settings.CompilerSettings.Parse(str.Split(' '));

			compiler = new OnTheFly.Compiler(settings);

			compiler.OnBuildFailure += new EventHandler<OnTheFly.Events.BuildFailureEventArgs>(Compiler_OnBuildFailure);
			compiler.OnBuildStart += new EventHandler<OnTheFly.Events.BuildStartEventArgs>(Compiler_OnBuildStart);
			compiler.OnBuildSuccess += new EventHandler<OnTheFly.Events.BuildSuccessEventArgs>(Compiler_OnBuildSuccess);

			string[] sources = new string[1];
			sources[0] = txtSourceCode.Text;

			compiler.Compile(sources);
		}

		void Compiler_OnBuildFailure(object semder, OnTheFly.Events.BuildFailureEventArgs e)
		{
			MessageBox.Show(compiler.Output.ToString());
		}

		void Compiler_OnBuildStart(object semder, OnTheFly.Events.BuildStartEventArgs e)
		{
			e.Cancel = false;
		}

		void Compiler_OnBuildSuccess(object semder, OnTheFly.Events.BuildSuccessEventArgs e)
		{
			compiler.Execute();
			compiler.Clean();
		}

		private void cmdExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
