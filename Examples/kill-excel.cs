using System;
using System.Diagnostics;

class Test
{
	public static void Main()
	{
		foreach (Process p in Process.GetProcesses())
		{
			if (p.ProcessName.StartsWith("excel", StringComparison.OrdinalIgnoreCase))
			{
				p.Kill();
			}
		}
	}
}