namespace QuiRing
{
	partial class UsersTab
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
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.saveUserButton = new System.Windows.Forms.Button();
			this.createReplicaButton = new System.Windows.Forms.Button();
			this.findUserButton = new System.Windows.Forms.Button();
			this.readButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.cardIdBox = new System.Windows.Forms.TextBox();
			this.issuedBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.authorisedZoneList = new System.Windows.Forms.ListBox();
			this.authorisedTerminalList = new System.Windows.Forms.ListBox();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.terminalsList = new System.Windows.Forms.CheckedListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.userLogView = new System.Windows.Forms.ListView();
			this.timeColumn = new System.Windows.Forms.ColumnHeader();
			this.eventColumn = new System.Windows.Forms.ColumnHeader();
			this.unitColumn = new System.Windows.Forms.ColumnHeader();
			this.resultColumn = new System.Windows.Forms.ColumnHeader();
			this.zonesList = new System.Windows.Forms.CheckedListBox();
			this.tableLayoutPanel4.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.63158F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.36842F));
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 1, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(808, 459);
			this.tableLayoutPanel4.TabIndex = 3;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 1;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 2;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.4F));
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(289, 453);
			this.tableLayoutPanel5.TabIndex = 0;
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.ColumnCount = 2;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel7.Controls.Add(this.saveUserButton, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.createReplicaButton, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.findUserButton, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.readButton, 0, 0);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 370);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 2;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel7.Size = new System.Drawing.Size(283, 80);
			this.tableLayoutPanel7.TabIndex = 0;
			// 
			// saveUserButton
			// 
			this.saveUserButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.saveUserButton.Enabled = false;
			this.saveUserButton.Location = new System.Drawing.Point(3, 43);
			this.saveUserButton.Name = "saveUserButton";
			this.saveUserButton.Size = new System.Drawing.Size(135, 34);
			this.saveUserButton.TabIndex = 4;
			this.saveUserButton.Text = "Save User";
			this.saveUserButton.UseVisualStyleBackColor = true;
			this.saveUserButton.Click += new System.EventHandler(this.SaveUserButtonClick);
			// 
			// createReplicaButton
			// 
			this.createReplicaButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.createReplicaButton.Enabled = false;
			this.createReplicaButton.Location = new System.Drawing.Point(144, 43);
			this.createReplicaButton.Name = "createReplicaButton";
			this.createReplicaButton.Size = new System.Drawing.Size(136, 34);
			this.createReplicaButton.TabIndex = 3;
			this.createReplicaButton.Text = "Write Card";
			this.createReplicaButton.UseVisualStyleBackColor = true;
			this.createReplicaButton.Click += new System.EventHandler(this.CreateReplicaButtonClick);
			// 
			// findUserButton
			// 
			this.findUserButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.findUserButton.Location = new System.Drawing.Point(3, 3);
			this.findUserButton.Name = "findUserButton";
			this.findUserButton.Size = new System.Drawing.Size(135, 34);
			this.findUserButton.TabIndex = 2;
			this.findUserButton.Text = "Find User";
			this.findUserButton.UseVisualStyleBackColor = true;
			this.findUserButton.Click += new System.EventHandler(this.FindUserButtonClick);
			// 
			// readButton
			// 
			this.readButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.readButton.Location = new System.Drawing.Point(144, 3);
			this.readButton.Name = "readButton";
			this.readButton.Size = new System.Drawing.Size(136, 34);
			this.readButton.TabIndex = 1;
			this.readButton.Text = "Read Card";
			this.readButton.UseVisualStyleBackColor = true;
			this.readButton.Click += new System.EventHandler(this.ClearButtonClick);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.7551F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.2449F));
			this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.label3, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.nameBox, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.cardIdBox, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.issuedBox, 1, 2);
			this.tableLayoutPanel3.Controls.Add(this.label4, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.label7, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.label8, 0, 4);
			this.tableLayoutPanel3.Controls.Add(this.authorisedZoneList, 1, 3);
			this.tableLayoutPanel3.Controls.Add(this.authorisedTerminalList, 1, 4);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 5;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(283, 361);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 26);
			this.label2.TabIndex = 0;
			this.label2.Text = "User Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 26);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 26);
			this.label3.TabIndex = 1;
			this.label3.Text = "Card ID";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nameBox
			// 
			this.nameBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nameBox.Enabled = false;
			this.nameBox.Location = new System.Drawing.Point(81, 3);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(199, 20);
			this.nameBox.TabIndex = 2;
			// 
			// cardIdBox
			// 
			this.cardIdBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cardIdBox.Location = new System.Drawing.Point(81, 29);
			this.cardIdBox.Name = "cardIdBox";
			this.cardIdBox.ReadOnly = true;
			this.cardIdBox.Size = new System.Drawing.Size(199, 20);
			this.cardIdBox.TabIndex = 3;
			// 
			// issuedBox
			// 
			this.issuedBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.issuedBox.Location = new System.Drawing.Point(81, 55);
			this.issuedBox.Name = "issuedBox";
			this.issuedBox.ReadOnly = true;
			this.issuedBox.Size = new System.Drawing.Size(199, 20);
			this.issuedBox.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(3, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 26);
			this.label4.TabIndex = 5;
			this.label4.Text = "Last Issued";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label7.Location = new System.Drawing.Point(3, 78);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 141);
			this.label7.TabIndex = 6;
			this.label7.Text = "Authorised Zones";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label8
			// 
			this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label8.Location = new System.Drawing.Point(3, 219);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(72, 142);
			this.label8.TabIndex = 7;
			this.label8.Text = "Authorised Terminals";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// authorisedZoneList
			// 
			this.authorisedZoneList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.authorisedZoneList.FormattingEnabled = true;
			this.authorisedZoneList.Location = new System.Drawing.Point(81, 81);
			this.authorisedZoneList.Name = "authorisedZoneList";
			this.authorisedZoneList.Size = new System.Drawing.Size(199, 135);
			this.authorisedZoneList.TabIndex = 8;
			// 
			// authorisedTerminalList
			// 
			this.authorisedTerminalList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.authorisedTerminalList.FormattingEnabled = true;
			this.authorisedTerminalList.Location = new System.Drawing.Point(81, 222);
			this.authorisedTerminalList.Name = "authorisedTerminalList";
			this.authorisedTerminalList.Size = new System.Drawing.Size(199, 136);
			this.authorisedTerminalList.TabIndex = 9;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.ColumnCount = 2;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.Controls.Add(this.terminalsList, 1, 3);
			this.tableLayoutPanel6.Controls.Add(this.label6, 1, 2);
			this.tableLayoutPanel6.Controls.Add(this.label5, 0, 2);
			this.tableLayoutPanel6.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.userLogView, 0, 1);
			this.tableLayoutPanel6.Controls.Add(this.zonesList, 0, 3);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(298, 3);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 4;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(507, 453);
			this.tableLayoutPanel6.TabIndex = 1;
			// 
			// terminalsList
			// 
			this.terminalsList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.terminalsList.FormattingEnabled = true;
			this.terminalsList.Location = new System.Drawing.Point(256, 248);
			this.terminalsList.Name = "terminalsList";
			this.terminalsList.Size = new System.Drawing.Size(248, 202);
			this.terminalsList.TabIndex = 5;
			// 
			// label6
			// 
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(256, 225);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(248, 20);
			this.label6.TabIndex = 3;
			this.label6.Text = "Terminals To Authorise";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(3, 225);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(247, 20);
			this.label5.TabIndex = 2;
			this.label5.Text = "Zones To Authorise";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.tableLayoutPanel6.SetColumnSpan(this.label1, 2);
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(501, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Logged Events";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// userLogView
			// 
			this.userLogView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.timeColumn,
									this.eventColumn,
									this.unitColumn,
									this.resultColumn});
			this.tableLayoutPanel6.SetColumnSpan(this.userLogView, 2);
			this.userLogView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userLogView.Location = new System.Drawing.Point(3, 20);
			this.userLogView.Name = "userLogView";
			this.userLogView.Size = new System.Drawing.Size(501, 202);
			this.userLogView.TabIndex = 0;
			this.userLogView.UseCompatibleStateImageBehavior = false;
			this.userLogView.View = System.Windows.Forms.View.Details;
			this.userLogView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.LogViewColumnClick);
			// 
			// timeColumn
			// 
			this.timeColumn.Text = "Time";
			this.timeColumn.Width = 141;
			// 
			// eventColumn
			// 
			this.eventColumn.Text = "Event";
			this.eventColumn.Width = 120;
			// 
			// unitColumn
			// 
			this.unitColumn.Text = "Unit";
			// 
			// resultColumn
			// 
			this.resultColumn.Text = "Result";
			this.resultColumn.Width = 133;
			// 
			// zonesList
			// 
			this.zonesList.CheckOnClick = true;
			this.zonesList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zonesList.FormattingEnabled = true;
			this.zonesList.Location = new System.Drawing.Point(3, 248);
			this.zonesList.MultiColumn = true;
			this.zonesList.Name = "zonesList";
			this.zonesList.Size = new System.Drawing.Size(247, 202);
			this.zonesList.TabIndex = 4;
			// 
			// UsersTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel4);
			this.Name = "UsersTab";
			this.Size = new System.Drawing.Size(808, 459);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ListBox authorisedTerminalList;
		private System.Windows.Forms.ListBox authorisedZoneList;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckedListBox zonesList;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckedListBox terminalsList;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader resultColumn;
		private System.Windows.Forms.ColumnHeader unitColumn;
		private System.Windows.Forms.ColumnHeader eventColumn;
		private System.Windows.Forms.ColumnHeader timeColumn;
		private System.Windows.Forms.ListView userLogView;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox issuedBox;
		private System.Windows.Forms.TextBox cardIdBox;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Button readButton;
		private System.Windows.Forms.Button findUserButton;
		private System.Windows.Forms.Button createReplicaButton;
		private System.Windows.Forms.Button saveUserButton;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
	}
}
