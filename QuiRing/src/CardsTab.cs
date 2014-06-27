using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Quiche.Data;
using Quiche.Proxcard;
using Quiche.Providers;

namespace QuiRing
{
	/// <summary>
	/// Description of CardsTab.
	/// </summary>
	public partial class CardsTab : QuiRingControl
	{
		public CardsTab()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		protected bool displayCardData = false;

		public void ResetControls()
		{
			this.createEnrolButton.Text = "Create Enrol Card";
			this.createRevokeButton.Text = "Create Revoke Card";
			this.createVerifyCard.Text = "Create Verify Card"; 
			this.createPermissionsCard.Text = "Create Admin Permissions Card";
			this.detailsButton.Text = "Read Card Details";
			this.wipeButton.Text = "Wipe Card";
			this.createAccessCard.Text = "Create Access Permissions Card";
			this.displayCardData = false;
			this.createEnrolButton.Enabled = QuicheProvider.Instance.ProxConnected;
			this.createPermissionsCard.Enabled = QuicheProvider.Instance.ProxConnected;
			this.createRevokeButton.Enabled = QuicheProvider.Instance.ProxConnected;
			this.createVerifyCard.Enabled = QuicheProvider.Instance.ProxConnected;
			this.wipeButton.Enabled = QuicheProvider.Instance.ProxConnected;
			this.detailsButton.Enabled = QuicheProvider.Instance.ProxConnected;
			this.createAccessCard.Enabled = QuicheProvider.Instance.ProxConnected;

		}
		
		public void DisableControls()
		{
			this.createEnrolButton.Enabled = false;
			this.createPermissionsCard.Enabled = false;
			this.createRevokeButton.Enabled = false;
			this.createVerifyCard.Enabled = false;
			this.wipeButton.Enabled = false;
			this.detailsButton.Enabled = false;
			this.createAccessCard.Enabled = false;
		}
		
		
		void CardButtonClick(object sender, System.EventArgs e)
		{
			if ((sender as Button).Text == "Cancel")
			{
				(sender as Button).Enabled = false;
				QuicheProvider.Instance.CancelProx();
			}
			else
			{
				if (sender == this.detailsButton)
				{
					this.displayCardData = true;
					QuicheProvider.Instance.ReadCard();
				}
				else
				{
					TokenType type  = (sender == this.wipeButton) ? TokenType.User : 
					(sender == this.createEnrolButton) ? TokenType.Enrol :
					(sender == this.createRevokeButton) ? TokenType.Revoke :
					(sender == this.createPermissionsCard) ? TokenType.AdminToggle :
					(sender == this.createVerifyCard) ? TokenType.Verify :
					(sender == this.createAccessCard) ? TokenType.Access : TokenType.None;
					QuicheProvider.Instance.WriteCard(type, 0, null, null);
				}
				this.DisableControls();
				(sender as Button).Text = "Cancel";
				(sender as Button).Enabled = true;
			}
		}

		public void OnCardRead(TokenType type, uint pin, List<ushort> terminals, List<ushort> zones)
		{
			if(!this.InvokeRequired)
			{
				if(this.displayCardData)
				{
					string tokenType = "User Card";
					this.displayCardData = false;
					switch (type)
					{
						case TokenType.Proxy: tokenType = "User Proxy Card"; break;
						case TokenType.AdminToggle: tokenType = "Control Card (Admin Permissions Toggle)"; break;
						case TokenType.Enrol: tokenType = "Control Card (Enrol)"; break;
						case TokenType.Revoke: tokenType = "Control Card (Revoke)"; break;
						case TokenType.Verify: tokenType = "Control Card (Verify)"; break;
						case TokenType.Access: tokenType = "Control Card (Access Permissions Set)"; break;
						default: break;
					}
					
					string text = string.Format("Card information: \nType: {0}\nCard ID: {1}\nZones: {2}\nTerminals :{3}", tokenType, pin.ToString(), zones!= null && zones.Count >  0 ? string.Join(", ", zones.ToArray()) : "None", terminals != null && terminals.Count > 0 ? string.Join(", ", terminals.ToArray()) : "None");
					User u = QuicheProvider.Instance.Client.Get<User>(pin.ToString());
					if (u!=null && u.Name!= null && u.Name!="") text = string.Format("{0}\nUser: {1}", text, u.Name);
					MessageBox.Show(text, "QuiRing: Card Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			else this.BeginInvoke(new Action<TokenType, uint, List<ushort>, List<ushort>>(this.OnCardRead), type, pin, terminals, zones);
		}
	}
}
