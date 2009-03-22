using System;
using System.Windows.Forms;

namespace OnTheFlyCompiler.Examples
{
	class Test
	{
		public static void Main()
		{
			var example = new Example();
			example.HelloWorld();
		}

		public static void Debug()
		{
			var example = new Example();
			example.Debug();
		}
	}

	class Example : ThirdPartlyLib.I3rdParty
	{
		public void HelloWorld()
		{
			string hello = "hello";
			string world = "WORLD";

			var c3rdParty = new ThirdPartlyLib.c3rdParty();
			MessageBox.Show(String.Format("{0} {1}", Foo(hello), c3rdParty.Foo(world)));
		}

		public void Debug()
		{
			var sb = new System.Text.StringBuilder();
			foreach (var asm in System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies())
			{
				sb.Append(String.Format("{0}, ", asm.FullName));
			}
			MessageBox.Show(sb.ToString());
		}

		public string Foo(string value)
		{
			return value.ToUpper();
		}
	}
}