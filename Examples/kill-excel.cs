using System;

namespace OnTheFlyCompiler.Examples
{
	public class Test
	{
		public static void Main()
		{
			foreach (var p in System.Diagnostics.Process.GetProcesses())
			{
				if (p.ProcessName.StartsWith("excel", StringComparison.OrdinalIgnoreCase))
				{
					p.Kill();
				}
			}
		}
	}
}