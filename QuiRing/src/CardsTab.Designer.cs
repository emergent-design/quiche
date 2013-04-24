namespace QuiRing
{
	partial class CardsTab
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.createVerifyCard = new System.Windows.Forms.Button();
			this.createEnrolButton = new System.Windows.Forms.Button();
			this.createRevokeButton = new System.Windows.Forms.Button();
			this.createPermissionsCard = new System.Windows.Forms.Button();
			this.wipeButton = new System.Windows.Forms.Button();
			this.detailsButton = new System.Windows.Forms.Button();
			this.createAccessCard = new System.Windows.Forms.Button();
			this.tableLayoutPanel8.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.ColumnCount = 3;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
			this.tableLayoutPanel8.Controls.Add(this.createVerifyCard, 2, 0);
			this.tableLayoutPanel8.Controls.Add(this.createEnrolButton, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.createRevokeButton, 1, 0);
			this.tableLayoutPanel8.Controls.Add(this.createPermissionsCard, 0, 1);
			this.tableLayoutPanel8.Controls.Add(this.wipeButton, 2, 1);
			this.tableLayoutPanel8.Controls.Add(this.detailsButton, 1, 2);
			this.tableLayoutPanel8.Controls.Add(this.createAccessCard, 1, 1);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 3;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(207, 150);
			this.tableLayoutPanel8.TabIndex = 1;
			// 
			// createVerifyCard
			// 
			this.createVerifyCard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.createVerifyCard.Location = new System.Drawing.Point(140, 3);
			this.createVerifyCard.Name = "createVerifyCard";
			this.createVerifyCard.Size = new System.Drawing.Size(64, 44);
			this.createVerifyCard.TabIndex = 3;
			this.createVerifyCard.Text = "Create Verify Card";
			this.createVerifyCard.UseVisualStyleBackColor = true;
			this.createVerifyCard.Click += new System.EventHandler(this.CardButtonClick);
			// 
			// createEnrolButton
			// 
			this.createEnrolButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.createEnrolButton.ForeColor = System.Drawing.Color.ForestGreen;
			this.createEnrolButton.Location = new System.Drawing.Point(3, 3);
			this.createEnrolButton.Name = "createEnrolButton";
			this.createEnrolButton.Size = new System.Drawing.Size(62, 44);
			this.createEnrolButton.TabIndex = 0;
			this.createEnrolButton.Text = "Create Enrol Card";
			this.createEnrolButton.UseVisualStyleBackColor = true;
			this.createEnrolButton.Click += new System.EventHandler(this.CardButtonClick);
			// 
			// createRevokeButton
			// 
			this.createRevokeButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.createRevokeButton.ForeColor = System.Drawing.Color.DarkRed;
			this.createRevokeButton.Location = new System.Drawing.Point(71, 3);
			this.createRevokeButton.Name = "createRevokeButton";
			this.createRevokeButton.Size = new System.Drawing.Size(63, 44);
			this.createRevokeButton.TabIndex = 1;
			this.createRevokeButton.Text = "Create Revoke Card";
			this.createRevokeButton.UseVisualStyleBackColor = true;
			this.createRevokeButton.Click += new System.EventHandler(this.CardButtonClick);
			// 
			// createPermissionsCard
			// 
			this.createPermissionsCard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.createPermissionsCard.ForeColor = System.Drawing.Color.RoyalBlue;
			this.createPermissionsCard.Location = new System.Drawing.Point(3, 53);
			this.createPermissionsCard.Name = "createPermissionsCard";
			this.createPermissionsCard.Size = new System.Drawing.Size(62, 44);
			this.createPermissionsCard.TabIndex = 2;
			this.createPermissionsCard.Text = "Create Admin Permissions Card";
			this.createPermissionsCard.UseVisualStyleBackColor = true;
			this.createPermissionsCard.Click += new System.EventHandler(this.CardButtonClick);
			// 
			// wipeButton
			// 
			this.wipeButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wipeButton.Location = new System.Drawing.Point(140, 53);
			this.wipeButton.Name = "wipeButton";
			this.wipeButton.Size = new System.Drawing.Size(64, 44);
			this.wipeButton.TabIndex = 4;
			this.wipeButton.Text = "Wipe Card";
			this.wipeButton.UseVisualStyleBackColor = true;
			this.wipeButton.Click += new System.EventHandler(this.CardButtonClick);
			// 
			// detailsButton
			// 
			this.detailsButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.detailsButton.Location = new System.Drawing.Point(71, 103);
			this.detailsButton.Name = "detailsButton";
			this.detailsButton.Size = new System.Drawing.Size(63, 44);
			this.detailsButton.TabIndex = 5;
			this.detailsButton.Text = "Read Card Details";
			this.detailsButton.UseVisualStyleBackColor = true;
			this.detailsButton.Click += new System.EventHandler(this.CardButtonClick);
			// 
			// createAccessCard
			// 
			this.createAccessCard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.createAccessCard.ForeColor = System.Drawing.Color.RoyalBlue;
			this.createAccessCard.Location = new System.Drawing.Point(71, 53);
			this.createAccessCard.Name = "createAccessCard";
			this.createAccessCard.Size = new System.Drawing.Size(63, 44);
			this.createAccessCard.TabIndex = 6;
			this.createAccessCard.Text = "Create Access Permissions Card";
			this.createAccessCard.UseVisualStyleBackColor = true;
			this.createAccessCard.Click += new System.EventHandler(this.CardButtonClick);
			// 
			// CardsTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel8);
			this.Name = "CardsTab";
			this.Size = new System.Drawing.Size(207, 150);
			this.Click += new System.EventHandler(this.CardButtonClick);
			this.tableLayoutPanel8.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button createAccessCard;
		private System.Windows.Forms.Button detailsButton;
		private System.Windows.Forms.Button wipeButton;
		private System.Windows.Forms.Button createPermissionsCard;
		private System.Windows.Forms.Button createRevokeButton;
		private System.Windows.Forms.Button createEnrolButton;
		private System.Windows.Forms.Button createVerifyCard;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
	}
}
