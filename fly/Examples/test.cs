using System;
using System.Windows.Forms;

namespace OnTheFlyCompiler.Examples
{
	public class Test
	{
		public static void Main()
		{
			new Example().HelloWorld();
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

		public string Foo(string value)
		{
			return value.ToUpper();
		}
	}
}