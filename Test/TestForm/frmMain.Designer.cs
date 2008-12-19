namespace Test
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdExit = new System.Windows.Forms.Button();
			this.cmdFly = new System.Windows.Forms.Button();
			this.txtSourceCode = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cmdExit
			// 
			this.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdExit.Location = new System.Drawing.Point(270, 11);
			this.cmdExit.Name = "cmdExit";
			this.cmdExit.Size = new System.Drawing.Size(75, 23);
			this.cmdExit.TabIndex = 2;
			this.cmdExit.Text = "Exit";
			this.cmdExit.UseVisualStyleBackColor = true;
			this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
			// 
			// cmdFly
			// 
			this.cmdFly.Location = new System.Drawing.Point(13, 11);
			this.cmdFly.Name = "cmdFly";
			this.cmdFly.Size = new System.Drawing.Size(75, 23);
			this.cmdFly.TabIndex = 0;
			this.cmdFly.Text = "Fly";
			this.cmdFly.UseVisualStyleBackColor = true;
			this.cmdFly.Click += new System.EventHandler(this.cmdFly_Click);
			// 
			// txtSourceCode
			// 
			this.txtSourceCode.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtSourceCode.Location = new System.Drawing.Point(13, 49);
			this.txtSourceCode.Multiline = true;
			this.txtSourceCode.Name = "txtSourceCode";
			this.txtSourceCode.Size = new System.Drawing.Size(332, 229);
			this.txtSourceCode.TabIndex = 1;
			this.txtSourceCode.Text = "using System;\r\nusing System.Windows.Forms;\r\n\r\nclass Test\r\n{\r\n    public static vo" +
				"id Main()\r\n    {\r\n\tMessageBox.Show(\"Hello World!\");\r\n    }\r\n}";
			// 
			// frmMain
			// 
			this.AcceptButton = this.cmdFly;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdExit;
			this.ClientSize = new System.Drawing.Size(357, 290);
			this.Controls.Add(this.txtSourceCode);
			this.Controls.Add(this.cmdFly);
			this.Controls.Add(this.cmdExit);
			this.Name = "frmMain";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdExit;
		private System.Windows.Forms.Button cmdFly;
		private System.Windows.Forms.TextBox txtSourceCode;
	}
}

