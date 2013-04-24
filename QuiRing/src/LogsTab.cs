using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

using Quiche;
using Quiche.Data;
using Quiche.Providers;

namespace QuiRing
{
	/// <summary>
	/// Description of LogsTab.
	/// </summary>
	public partial class LogsTab : QuiRingControl
	{

		private class LogItem
		{
			public DateTime	Timestamp	{ get; private set; }
			public string	Event 		{ get; private set; }
			public string 	User		{ get; private set; }
			public string 	Terminal	{ get; private set; }
			public string 	Result		{ get; private set; }
	
			public LogItem (Log log)
			{
				Terminal source = QuicheProvider.Instance.Client.Get<Terminal>(log.TerminalId);
				User user = QuicheProvider.Instance.Client.Get<User>(log.UserId);
				this.User = user != null ? user.Name : log.UserId; 
				this.Terminal = source != null ? (source.Name!= "" && source.Name != null ? source.Name : source.Id) : log.TerminalId;
				this.Result = log.Result;
				this.Event = log.Event;
				this.Timestamp = log.Timestamp;
			}
		}

		private BindingList<LogItem> logs = new BindingList<LogItem>();

		public LogsTab()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			foreach(DataGridViewTextBoxColumn c in this.logView.Columns)
			{
				c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			}

			this.logs.RaiseListChangedEvents = false;
			this.logView.DataSource = this.logs;
		}

		public void Initialise()
		{
			if (QuicheProvider.Instance.Cache!= null) 
			{
				foreach(Log log in QuicheProvider.Instance.Cache.Logs.OrderBy(l=>l.Timestamp)) this.OnLog(log);
			}
			this.logs.RaiseListChangedEvents = true;
			this.addressBox.Text = QuicheProvider.Instance.Client.Address;
			this.portNumberSelection.Value = (decimal)QuicheProvider.Instance.Client.Port;
			string logLocation = QuicheProvider.Instance.LogPath;
			if(logLocation!=null && logLocation!="") this.logButton.Text = string.Format("Logging to: {0}", logLocation);
			else this.logButton.Text = "Set log path";
		}

		public void UpdateConnectionStatus(bool connection, bool tryingToConnect)
		{
			if(!InvokeRequired)
			{
				this.connectButton.Text = connection ? "Disconnect" : tryingToConnect ? "Cancel Connection" : "Connect";
				this.connectButton.Enabled = true;
				this.addressBox.Enabled = !tryingToConnect;
				this.portNumberSelection.Enabled = !tryingToConnect;
			}
			else this.BeginInvoke(new Action<bool, bool>(this.UpdateConnectionStatus), connection, tryingToConnect);
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


		private void RemoveExtraRows()
		{
			if (!InvokeRequired)
			{
				while (this.logView.Rows.Count > this.logs.Count)
				{
					this.logView.Rows.RemoveAt(this.logView.Rows.Count-1);
				}
			}
			else this.BeginInvoke (new Action(this.RemoveExtraRows));
		}

		public void OnLog(Log log)
		{
			this.logs.RaiseListChangedEvents = false;
			while (this.logs.Count >= 1024) this.logs.RemoveAt(0);
			this.RemoveExtraRows();
			this.logs.RaiseListChangedEvents = true;
			this.logs.Add(new LogItem(log));
		}

		void ConnectButtonClick(object sender, EventArgs e)
		{
			lock(this.connectButton)
			{
				if(this.connectButton.Enabled)
				{
					this.connectButton.Enabled = false;
					this.addressBox.Enabled = false;
					this.portNumberSelection.Enabled = false;
					if(!QuicheProvider.Instance.Client.Connect)
					{
						while (QuicheProvider.Instance.Client.Connected)
						{
							if (QuicheProvider.Instance.Client.Connect!=false) QuicheProvider.Instance.Client.Connect = false;
							System.Threading.Thread.Sleep(5);
						}

						QuicheProvider.Instance.Client.Address = this.addressBox.Text;
						QuicheProvider.Instance.Client.Port = (int)this.portNumberSelection.Value;
						QuicheProvider.Instance.Client.Connect = true;
						this.connectButton.Text = "Cancel";
					}
					else
					{
						this.connectButton.Text="Disconnecting...";
						QuicheProvider.Instance.Client.Connect = false;
					}
				}
			}
		}

		public void Disconnect()
		{
			QuicheProvider.Instance.Client.Connect = false;
		}

		void LogButtonClick(object sender, EventArgs e)
		{
			FolderBrowserDialog locationPicker = new FolderBrowserDialog();
		    if (locationPicker.ShowDialog() == DialogResult.OK) 
		    {
		        this.logButton.Text = string.Format("Logging to: {0}", locationPicker.SelectedPath);
		       QuicheProvider.Instance.LogPath = locationPicker.SelectedPath;
		    }			
		}
		
		void ImportButtonClick(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog() {  RestoreDirectory = true, AddExtension = true, DefaultExt = "txt", Filter="XML Text(*.xml)|*.xml", Title = "QuiRing: Export Logs", CheckFileExists = true };
			if (open.ShowDialog() == DialogResult.OK && File.Exists(open.FileName))
			{
				QuicheProvider.Instance.Cache.Import(open.FileName);
			}
		}
		
		void ExportButtonClick(object sender, EventArgs e)
		{
			SaveFileDialog save = new SaveFileDialog() { RestoreDirectory = true, AddExtension = true, DefaultExt = "xml", Filter="XML Text (*.xml)|*.xml", Title = "QuiRing: Export Database", OverwritePrompt = true };
			if (save.ShowDialog() == DialogResult.OK)
			{
				QuicheProvider.Instance.Cache.Export(save.FileName);
			}
		}

		public void OnQuiConnection(ConnectionEvent type, string address, int port)
		{
			this.addressBox.Text = address;
			this.portNumberSelection.Value = (decimal)port;
			if (!QuicheProvider.Instance.Client.Connect && !QuicheProvider.Instance.Client.Connected)
			{
				this.connectButton.Text = "Connect";
				this.addressBox.Enabled = true;
				this.portNumberSelection.Enabled = true;
			}
			else this.connectButton.Text = type == ConnectionEvent.Connected ? "Disconnect" : type == ConnectionEvent.Error ? "Cancel" : "Connect";
			this.connectButton.Enabled = true;
		}

	}
}
