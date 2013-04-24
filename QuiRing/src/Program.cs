using System;
using System.Threading;
using System.IO.Pipes;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

namespace QuiRing
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			string ipc = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "quiring", "quiring.ipc");
			bool createdNew = true;
			using (Mutex mutex = new Mutex(true, "QuiRing", out createdNew))
			{
				if (createdNew)
				{
					if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(ipc))) System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ipc));
					if (!System.IO.File.Exists(ipc)) System.IO.File.Create(ipc);
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Application.Run(new QuiRingForm(args.Contains("wipe")));
				}
				else
				{
					Process current = Process.GetCurrentProcess();
					var original = Process.GetProcessesByName(current.ProcessName).Where(p => p.Id != current.Id).FirstOrDefault();
					if (original != null)
					{
						new System.IO.FileInfo(ipc).LastWriteTime = System.DateTime.Now;
					}
				}
			}
		}
	}
}
