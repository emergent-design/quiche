using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

using Quiche.Data;
using Quiche.Proxcard;
using Quiche.Providers;

namespace QuiRing
{
	public partial class UsersTab : QuiRingControl
	{
		protected List<Zone> zones;
		protected List<Terminal> units;
		
		public UsersTab()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.userLogView.ListViewItemSorter = this.sorter;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void FindUserButtonClick(object sender, EventArgs e)
		{
			FindUserForm finder = new FindUserForm( QuicheProvider.Instance.Client.GetAll<User>() );
			if (finder.ShowDialog()== DialogResult.OK) 
			{
				this.cardIdBox.Text = finder.selected.Id.ToString();
				this.SelectUser(finder.selected);
				this.saveUserButton.Enabled = true;
				this.nameBox.Enabled = true;
			}
		}
		
		void ClearButtonClick(object sender, EventArgs e)
		{
			if (readButton.Text=="Clear")
			{
				this.cardIdBox.Text = string.Empty;
				this.nameBox.Text = string.Empty;
				this.issuedBox.Text = string.Empty;
				this.userLogView.Items.Clear();
				this.saveUserButton.Enabled = false;
				this.createReplicaButton.Enabled = false;
				this.nameBox.Enabled = false;
				this.authorisedTerminalList.Items.Clear();
				this.authorisedZoneList.Items.Clear();
				this.zonesList.Items.Clear();
				this.terminalsList.Items.Clear();
				this.readButton.Text = "Read Card";
				this.readButton.Enabled = QuicheProvider.Instance.ProxConnected;
				return;
			}
			if (readButton.Text=="Read Card")
			{
				QuicheProvider.Instance.CancelProx();
				this.findUserButton.Enabled = false;
				this.saveUserButton.Enabled = false;
				this.createReplicaButton.Enabled = false;
				readButton.Text = "Cancel Read";				
				QuicheProvider.Instance.ReadCard();
				return;
			}
			if (readButton.Text=="Cancel Read")
			{
				this.findUserButton.Enabled = true;
				QuicheProvider.Instance.CancelProx();
				this.readButton.Text = "Clear";
				return;
			}
		}
		
		protected void UpdateLogView()
		{
			this.userLogView.Items.Clear();
			uint userID;
			if ( uint.TryParse(this.cardIdBox.Text, out userID))
			{
				this.userLogView.BeginUpdate();
				foreach (Log log in QuicheProvider.Instance.Cache.Logs.Where(l => l.UserId == userID.ToString()))
				{
					Terminal u = log.TerminalId != null ? QuicheProvider.Instance.Client.Get<Terminal>(Guid.Parse(log.TerminalId).ToString()) : null;
					this.userLogView.Items.Add(new ListViewItem(new []{ log.Timestamp.ToString(), log.Event, u!=null ? (u.Name!= "" ? u.Name : u.Id.ToString()) : "",  log.Result }));
				}
				this.userLogView.EndUpdate();
			}
		}
		
		protected void SelectUser(User user)
		{
			if (user!=null)
			{
				this.nameBox.Text = user.Name;
				this.issuedBox.Text = user.LastIssued.ToString();
				var remote = QuicheProvider.Instance.Client.Get<User>(user.Id);
				user.TerminalWhiteList = remote.TerminalWhiteList;
				user.ZoneWhiteList = remote.ZoneWhiteList;
			}
			this.UpdateLogView();
			this.saveUserButton.Enabled = true;
			this.createReplicaButton.Enabled = true;
			this.readButton.Text = "Clear";
			this.readButton.Enabled = true;
			this.UpdatePermissionsList(user);
			
		}
		
		public void ResetControls()
		{
			this.findUserButton.Enabled = true;
			this.createReplicaButton.Text = "Write Card";
			this.createReplicaButton.Enabled = QuicheProvider.Instance.ProxConnected;
			this.saveUserButton.Enabled = true;
			this.readButton.Text = "Read Card";
			this.readButton.Enabled = QuicheProvider.Instance.ProxConnected;
		}

		
		protected void UpdatePermissionsList(User user)
		{
			if(!this.InvokeRequired)
			{
				var zones = QuicheProvider.Instance.Client.GetAll<Zone>();
				var terminals = QuicheProvider.Instance.Client.GetAll<Terminal>();
				this.zonesList.BeginUpdate();

				this.zonesList.Items.Clear();
				this.terminalsList.Items.Clear();

				foreach (Zone z in zones) this.zonesList.Items.Add(z, false);
				foreach (Terminal u in terminals) this.terminalsList.Items.Add(u, false);
				
				this.zonesList.EndUpdate();

				this.authorisedTerminalList.SuspendLayout();
				this.authorisedZoneList.SuspendLayout();


				this.authorisedTerminalList.Items.Clear();
				this.authorisedZoneList.Items.Clear();

				if (user != null && user.TerminalWhiteList!=null && user.TerminalWhiteList.Count > 0)
				{	
					foreach(ushort t in user.TerminalWhiteList)
					{
						var term = terminals.Where( x => x.ReferenceId == t).SingleOrDefault();
						if (term != null) this.authorisedTerminalList.Items.Add(term);
					}
				}
				
				if (user != null && user.ZoneWhiteList!=null && user.ZoneWhiteList.Count > 0)
				{
					foreach(ushort t in user.ZoneWhiteList)
					{
						var term = zones.Where( x => x.Id == t.ToString()).SingleOrDefault();
						if (term != null) this.authorisedZoneList.Items.Add(term);
					}
				}
				this.authorisedTerminalList.ResumeLayout();
				this.authorisedZoneList.ResumeLayout();
			}
			else this.BeginInvoke(new Action<User>(this.UpdatePermissionsList), new object[]{ user });
		}
		
		protected void SetCurrentPermissions(List<ushort> units, List<ushort> zones)
		{
			if(!this.InvokeRequired)
			{
				if (zones != null)
				{
					for(int i = 0; i < this.zonesList.Items.Count; i++)
					{
						this.zonesList.SetItemChecked(i, zones.Contains(ushort.Parse(((Zone)(this.zonesList.Items[i])).Id)));
					}
				}
				
				if (units != null)
				{
					for(int i = 0; i < this.terminalsList.Items.Count; i++)
					{
						this.terminalsList.SetItemChecked(i, units.Contains(((Terminal)(this.terminalsList.Items[i])).ReferenceId));
					}
				}
			}
			else this.BeginInvoke(new Action<List<ushort>, List<ushort>>(this.SetCurrentPermissions), new object[]{ units, zones });
		}
		
		public void OnLog(Log log)
		{
			if(!this.InvokeRequired)
			{
				if (log.UserId!=null && this.cardIdBox.Text!="" && this.cardIdBox.Text==log.UserId)
				{
					Terminal source = QuicheProvider.Instance.Client.Get<Terminal>(log.TerminalId);
					this.userLogView.Items.Add(new ListViewItem(new []{ log.Timestamp.ToString(), log.Event, source!=null ? (source.Name!= "" ? source.Name : source.Id.ToString()) : log.TerminalId,  log.Result }));
				}
			}
			else this.BeginInvoke(new Action<Log>(this.OnLog), log);
		}
		
		public void OnCardRead(TokenType type, uint pin, List<ushort> units, List<ushort> zones)
		{
			if (!this.InvokeRequired)
			{
				this.userLogView.Items.Clear();
				this.terminalsList.Items.Clear();
				this.zonesList.Items.Clear();
				this.authorisedTerminalList.Items.Clear();
				this.authorisedZoneList.Items.Clear();
				this.cardIdBox.Text = string.Empty;
				this.nameBox.Text = string.Empty;
				this.issuedBox.Text = string.Empty;
				if((type == TokenType.User) || (type == TokenType.Proxy))
				{
					this.cardIdBox.Text = pin.ToString();
					this.saveUserButton.Enabled = true;
					this.createReplicaButton.Enabled = false;
					this.nameBox.Enabled = true;
			
					this.SelectUser(QuicheProvider.Instance.Client.Get<User>(pin.ToString()));
					this.SetCurrentPermissions(units, zones);

				}
				else
				{
					this.saveUserButton.Enabled = false;
					this.createReplicaButton.Enabled = false;
				}
			}
			else this.BeginInvoke(new Action<TokenType, uint, List<ushort>, List<ushort>>(this.OnCardRead), type, pin, units, zones);
		}
		
		void SaveUserButtonClick(object sender, EventArgs e)
		{
			User u = new User() { Name = this.nameBox.Text , LastIssued = DateTime.Now, Id = this.cardIdBox.Text};
			this.issuedBox.Text = u.LastIssued.ToString();
			QuicheProvider.Instance.Cache.Store(u);
		}

		public void ResetWritingControls()
		{
			this.createReplicaButton.Text = "Write Card";
			this.findUserButton.Enabled = true;
			this.readButton.Enabled = true;
			this.createReplicaButton.Enabled = QuicheProvider.Instance.ProxConnected;
			if (this.readButton.Text == "Read Card" ) this.readButton.Enabled = QuicheProvider.Instance.ProxConnected;
		}
		
		protected List<ushort>GetChecked(CheckedListBox list)
		{
			List<ushort> result = new List<ushort>();
			foreach (var c in list.CheckedItems)
				result.Add((c is Zone) ? ushort.Parse((c as Zone).Id) : (c as Terminal).ReferenceId);
			return result;
		}
		
		public void CreateReplicaButtonClick(object sender, EventArgs e)
		{
			if (this.createReplicaButton.Text == "Cancel")
			{
				QuicheProvider.Instance.CancelProx();
			}
			else
			{
				List<ushort> terms = this.GetChecked(this.terminalsList);
				uint pin = 0;
				if (uint.TryParse(this.cardIdBox.Text, out pin))
				{
					QuicheProvider.Instance.WriteCard(TokenType.Proxy, uint.Parse(this.cardIdBox.Text), this.GetChecked(this.zonesList), terms);
					this.SaveUserButtonClick(sender, e);
					this.findUserButton.Enabled = false;
					this.readButton.Enabled = false;
					this.createReplicaButton.Text = "Cancel";
				}
			}
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

	}
}
