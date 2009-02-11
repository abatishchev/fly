using System;
using System.Diagnostics;

class Test
{
	public static void Main(string[] args)
	{
		foreach (Process p in Process.GetProcesses())
		{
			if (p.ProcessName.StartsWith("EXCEL"))
			{
				p.Kill();
			}
		}
	}
}