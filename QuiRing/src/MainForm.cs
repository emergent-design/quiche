using System;
using System.IO;
using System.IO.Ports;
using System.Data.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using Quiche;
using Quiche.Data;
using Quiche.Proxcard;
using Quiche.Providers;

using System.Linq;
using System.Xml.Serialization;

namespace QuiRing
{

	public partial class QuiRingForm : Form
	{

		protected ListViewColumnSorter sorter = new ListViewColumnSorter();
	
		protected Dictionary<Guid, DateTime> connectedUnits = new Dictionary<Guid, DateTime>();
		protected bool refreshUnitView = true;
		protected bool reallyClose = false;
		protected string ipc;
		protected DateTime lastTouch;
		protected DateTime lastWarning = DateTime.Now;
		protected System.Windows.Forms.Timer statusTimer = null;
		protected Thread signallerThread = null;
				


		protected bool displayCardData = false;

		public QuiRingForm(bool wipe)
		{
			this.Cursor = Cursors.WaitCursor;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			QuicheProvider.Initialise<SqliteBackedPerceptionProx>();
			QuicheProvider.Instance.CardProgress += this.OnCardProgress;
			QuicheProvider.Instance.CardOperation += this.OnCardOperation;

			QuicheProvider.Instance.CardRead += this.cardsTab.OnCardRead;
			QuicheProvider.Instance.CardRead += this.usersTab.OnCardRead;

			QuicheProvider.Instance.ProxConnectionChanged += this.OnProxConnection;

			QuicheProvider.Instance.Client.ConnectionChanged += this.logsTab.OnQuiConnection;
			QuicheProvider.Instance.Client.ConnectionChanged += this.OnQuiConnection;

			QuicheProvider.Instance.Client.LogReceived += this.logsTab.OnLog;
			QuicheProvider.Instance.Client.LogReceived += this.usersTab.OnLog;

			QuicheProvider.Instance.Initialise("quiring", wipe);

			this.logsTab.Initialise();
			this.unitsView.ListViewItemSorter = this.sorter;

			this.ipc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "quiring", "quiring.ipc");

			this.lastTouch = new FileInfo(this.ipc).LastWriteTime;
			this.signallerThread = new Thread(new ThreadStart(this.QuiRingWatcher));
			this.signallerThread.Start();

			this.statusTimer = new System.Windows.Forms.Timer();
			this.statusTimer.Interval = 2000;
			this.statusTimer.Tick += this.StatusTick;
			this.statusTimer.Enabled = true;

			this.ResetControls();
			this.Cursor = Cursors.Default;			
		}

		protected void UpdateConnections(List<Terminal> terminals, bool connected)
		{
			if(!this.InvokeRequired)
			{
				this.unitsView.BeginUpdate();
				IEnumerable<ListViewItem> lv = this.unitsView.Items.Cast<ListViewItem>();
				foreach (Terminal t in terminals)
				{
					var found = lv.Where(l => l.SubItems[2].Text == t.Id.ToString());
					if (found.Count()==0)
					{
						this.unitsView.Items.Add(new ListViewItem(new[]{t.Name, t.QuiNode, t.Id, "Connected"}));
					}
					else
					{
						foreach (var i in found)
						{
							if (i.SubItems[0].Text!= t.Name) i.SubItems[0].Text = t.Name;
							if (i.SubItems[1].Text!= t.QuiNode) i.SubItems[1].Text = t.QuiNode;
							if (i.SubItems[3].Text!= (connected ? "Connected" : "Disconnected")) i.SubItems[3].Text = connected ? "Connected" : "Disconnected";
						}
					}
				}
				this.unitsView.EndUpdate();
			}
			else this.BeginInvoke(new Action<List<Terminal>, bool>(this.UpdateConnections), terminals, connected);
		}

		protected void StatusTick(object sender, EventArgs e)
		{
			this.UpdateConnections(QuicheProvider.Instance.Cache.ConnectedTerminals, true);
			this.UpdateConnections(QuicheProvider.Instance.Cache.DisconnectedTerminals, false);
		}


		protected void BringBackWindow()
		{
			if (!this.InvokeRequired)
			{
				this.WindowState = FormWindowState.Minimized;
				this.Activate();
				this.BringToFront();
				this.Show();
				
				this.WindowState = FormWindowState.Normal;
			}
			else this.BeginInvoke(new Action(this.BringBackWindow));
		}
		
		protected void QuiRingWatcher()
		{
			while(!this.reallyClose)
			{
				if (this.lastTouch != new FileInfo(this.ipc).LastWriteTime)
				{
					this.BringBackWindow();
					new FileInfo(this.ipc).LastWriteTime = this.lastTouch = DateTime.Now;
				}
				else Thread.Sleep(100);
			}
		}


		void ToolStripDropDownButton1DropDownOpening(object sender, EventArgs e)
		{
	
			this.toolStripDropDownButton1.DropDownItems.Clear();
			this.toolStripDropDownButton1.DropDownItems.Add("Disconnect");
			foreach (string port in SerialPort.GetPortNames())
			{
				using (SerialPort check = new SerialPort(port))
				{
					try	{ check.Open(); }
					catch{}	
					finally
					{
						if (check.IsOpen)
						{
							check.Close();
							this.toolStripDropDownButton1.DropDownItems.Add(port);
						}
					}
				}
			}
		}


		void ToolStripDropDownButton1DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			string commPort = e.ClickedItem.Text;
			this.toolStripDropDownButton1.Text = commPort =="Disconnect" ? "Disconnected" : commPort;
			QuicheProvider.Instance.ConnectProx(commPort);
		}

		protected void OnProxConnection(string connection, bool connected)
		{
			if (!InvokeRequired)
			{
				this.toolStripDropDownButton1.Text = connected ? connection : "Disconnected";
				this.ResetControls();
			}
			else this.BeginInvoke(new Action<string, bool>(this.OnProxConnection), connection, connected);
		}

		protected void OnCardOperation(OperationType operation)
		{
			if (!this.InvokeRequired)
			{
				switch(operation)
				{
					case OperationType.ReadStarted: 	this.toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
														this.toolStripProgressBar1.MarqueeAnimationSpeed = 50; 
														break;
					case OperationType.ReadFinished: 	this.ResetControls();	break;
					case OperationType.ReadAborted:		this.ResetControls();	break;
					case OperationType.WriteStarted: 	break;
					case OperationType.WriteFinished: 	this.ResetControls();	break;
					case OperationType.WriteAborted:	MessageBox.Show("An error occurred when writing the card");
														this.ResetControls();	break;
				}
			}
			else BeginInvoke(new Action<OperationType>(this.OnCardOperation), operation);
		}


		protected void OnCardProgress(bool error, int progress, bool readOperation)
		{
			if (!InvokeRequired)
			{
				if (readOperation)
				{
					if (error)
					{
						this.toolStripProgressBar1.Value = 0;
					}
					else
					{
						this.toolStripProgressBar1.Value = progress;
					}
				}
				else this.toolStripProgressBar1.Value = progress;
			}
			else BeginInvoke(new Action<bool, int, bool>(this.OnCardProgress), error, progress, readOperation);
		}


		protected void ResetControls()
		{
			this.cardsTab.ResetControls();
			this.usersTab.ResetControls();
			this.toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
			this.toolStripProgressBar1.Value = 0;
		}


		void MainTabControlSelecting(object sender, TabControlCancelEventArgs e)
		{
			QuicheProvider.Instance.CancelProx();
			this.ResetControls();
		}
			

		void LogViewColumnClick(object sender, ColumnClickEventArgs e)
		{
			if ( e.Column == sorter.SortColumn ) sorter.Order = sorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
			else
			{
				sorter.SortColumn = e.Column;
				sorter.Order = SortOrder.Ascending;
			}
			(sender as ListView).Sort();
		}
		
		void MainFormFormClosed(object sender, FormClosedEventArgs e)
		{
			if (this.signallerThread!=null)
			{
				this.signallerThread.Join();
			}
			QuicheProvider.Instance.Dispose();
		}
		
		void NotifyIcon1MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (this.WindowState == FormWindowState.Minimized)
				{
					this.Show();
					this.WindowState = FormWindowState.Normal;
					this.BringToFront();
					this.Activate();
					this.restoreToolStripMenuItem.Text = "Hide";
				}
				else 
				{
					this.restoreToolStripMenuItem.Text = "Show";
					this.Hide();
					this.WindowState = FormWindowState.Minimized;
				}
			}
		}

		void RestoreToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.Show();
				this.WindowState = FormWindowState.Normal;
				this.BringToFront();
				this.Activate();
				this.restoreToolStripMenuItem.Text = "Hide";
			}
			else 
			{
				this.restoreToolStripMenuItem.Text = "Show";
				this.Hide();
				this.WindowState = FormWindowState.Minimized;
			}
		}
		
		void MainFormResize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{
				this.restoreToolStripMenuItem.Text = "Show";
			    this.Hide();
			}
			else this.restoreToolStripMenuItem.Text = "Hide";

		}

		void InfoToolStripMenuItemClick (object sender, EventArgs e)
		{

			string version = new DateTime(2000,1,1).AddDays(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build).ToString("yy.MM.dd");
			MessageBox.Show(string.Format("QuiRing version {0}({1})\n\nSee the Quiche project for details\n\nhttps://github.com/emergent-design/quiche", version, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString()), "About QuiRing", 
			                MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (System.Environment.OSVersion.Platform != PlatformID.Unix)
			{
				this.notifyIcon1.Text = "QuiRing (Exiting)";
				this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
				this.notifyIcon1.BalloonTipTitle = "QuiRing is exiting";
				this.notifyIcon1.BalloonTipText = "QuiRing is exiting. Logs will no longer be recorded.";
				DateTime buhbye = DateTime.Now.AddSeconds(3.5);
				this.notifyIcon1.ShowBalloonTip(0);
				while (DateTime.Now < buhbye) Thread.Sleep(10);
			}
			else
			{
				//The notifier is a bit flaky in linux, so use a message box instead
				MessageBox.Show("QuiRing is exiting. Logs will no longer be recorded");
			}
			this.statusTimer.Stop();
			this.statusTimer.Dispose();
			this.reallyClose = true;
			this.Close();
		}


		void MainFormFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			if(!this.reallyClose && !(e.CloseReason == System.Windows.Forms.CloseReason.TaskManagerClosing))
			{
				e.Cancel = true;
				this.Hide();
				this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			}
		}

		void OnQuiConnection(ConnectionEvent type, string address, int port)
		{
			if (!InvokeRequired)
			{
				switch (type)
				{
					case ConnectionEvent.Error:		if (this.lastWarning.AddSeconds(5) < DateTime.Now)
													{
														this.lastWarning = DateTime.Now;
														this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
														this.notifyIcon1.BalloonTipTitle = "QuiRing Connection Error";
														this.notifyIcon1.BalloonTipText = string.Format("Warning: Connection to Qui on {0}:{1} has been lost.\nLogging will continue when a connection is re-established", address, port);
														this.notifyIcon1.ShowBalloonTip(2000);
														this.notifyIcon1.Text = string.Format("QuiRing (Trying to connect)");
													}
													this.notifyIcon1.Icon = global::QuiRing.Icons.Icons_quiFault;
													break;

					case ConnectionEvent.Connected:	this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
													this.notifyIcon1.BalloonTipTitle = "QuiRing Connected";
													this.notifyIcon1.BalloonTipText = string.Format("Connected to Qui on {0}:{1}", address, port);
													this.notifyIcon1.ShowBalloonTip(500);
													this.notifyIcon1.Text = string.Format("QuiRing (Connected)");
													this.notifyIcon1.Icon = global::QuiRing.Icons.Icons_quiConnected;
													break;

					case ConnectionEvent.Disconnected:	this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
														this.notifyIcon1.BalloonTipTitle = "Qui Disconnected";
														this.notifyIcon1.BalloonTipText = string.Format("Disconnected from Qui", address, port);
														this.notifyIcon1.ShowBalloonTip(500);
														this.notifyIcon1.Text = string.Format("QuiRing (Disconnected)");
														this.notifyIcon1.Icon = global::QuiRing.Icons.Icons_qui;
														break;
				}
			}
			else this.BeginInvoke(new Action<ConnectionEvent, string, int>(this.OnQuiConnection), type, address, port);
		}
	}
}
