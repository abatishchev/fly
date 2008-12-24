using System;
using System.Reflection;
using System.Windows.Forms;
using System.Text;

class Test
{
	public static void HelloWorld()
	{
		Example example = new Example();
		example.HelloWorld();
	}

	public static void Debug()
	{
		Example example = new Example();
		example.Debug();
	}
}

class Example : ThirdPartlyLib.I3rdParty
{
	public void HelloWorld()
	{
		string hello = "hello";
		string world = "WORLD";

		ThirdPartlyLib.c3rdParty c3rdParty = new ThirdPartlyLib.c3rdParty();
		MessageBox.Show(String.Format("{0} {1}", Foo(hello), c3rdParty.Foo(world)));
	}

	public void Debug()
	{
		StringBuilder sb = new StringBuilder();
		foreach (AssemblyName asm in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
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
