namespace QuiRing
{
	partial class QuiRingForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
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
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.usersPage = new System.Windows.Forms.TabPage();
			this.usersTab = new QuiRing.UsersTab();
			this.controlCardPage = new System.Windows.Forms.TabPage();
			this.cardsTab = new QuiRing.CardsTab();
			this.settingsPage = new System.Windows.Forms.TabPage();
			this.logsTab = new QuiRing.LogsTab();
			this.unitsPage = new System.Windows.Forms.TabPage();
			this.unitsView = new System.Windows.Forms.ListView();
			this.nameColumn = new System.Windows.Forms.ColumnHeader();
			this.nodeColumn = new System.Windows.Forms.ColumnHeader();
			this.serialColumn = new System.Windows.Forms.ColumnHeader();
			this.statusColumn = new System.Windows.Forms.ColumnHeader();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			this.mainTabControl.SuspendLayout();
			this.usersPage.SuspendLayout();
			this.controlCardPage.SuspendLayout();
			this.settingsPage.SuspendLayout();
			this.unitsPage.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.notifyMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.mainTabControl, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(723, 457);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// mainTabControl
			// 
			this.mainTabControl.Controls.Add(this.usersPage);
			this.mainTabControl.Controls.Add(this.controlCardPage);
			this.mainTabControl.Controls.Add(this.settingsPage);
			this.mainTabControl.Controls.Add(this.unitsPage);
			this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTabControl.Location = new System.Drawing.Point(3, 3);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(717, 429);
			this.mainTabControl.TabIndex = 1;
			this.mainTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.MainTabControlSelecting);
			// 
			// usersPage
			// 
			this.usersPage.Controls.Add(this.usersTab);
			this.usersPage.Location = new System.Drawing.Point(4, 22);
			this.usersPage.Name = "usersPage";
			this.usersPage.Padding = new System.Windows.Forms.Padding(3);
			this.usersPage.Size = new System.Drawing.Size(709, 403);
			this.usersPage.TabIndex = 0;
			this.usersPage.Text = "Users";
			this.usersPage.UseVisualStyleBackColor = true;
			// 
			// usersTab
			// 
			this.usersTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.usersTab.Location = new System.Drawing.Point(3, 3);
			this.usersTab.Name = "usersTab";
			this.usersTab.Size = new System.Drawing.Size(703, 397);
			this.usersTab.TabIndex = 0;
			// 
			// controlCardPage
			// 
			this.controlCardPage.Controls.Add(this.cardsTab);
			this.controlCardPage.Location = new System.Drawing.Point(4, 22);
			this.controlCardPage.Name = "controlCardPage";
			this.controlCardPage.Padding = new System.Windows.Forms.Padding(3);
			this.controlCardPage.Size = new System.Drawing.Size(709, 403);
			this.controlCardPage.TabIndex = 1;
			this.controlCardPage.Text = "Control Cards";
			this.controlCardPage.UseVisualStyleBackColor = true;
			// 
			// cardsTab
			// 
			this.cardsTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cardsTab.Location = new System.Drawing.Point(3, 3);
			this.cardsTab.Name = "cardsTab";
			this.cardsTab.Size = new System.Drawing.Size(743, 180);
			this.cardsTab.TabIndex = 0;
			// 
			// settingsPage
			// 
			this.settingsPage.Controls.Add(this.logsTab);
			this.settingsPage.Location = new System.Drawing.Point(4, 22);
			this.settingsPage.Name = "settingsPage";
			this.settingsPage.Padding = new System.Windows.Forms.Padding(3);
			this.settingsPage.Size = new System.Drawing.Size(709, 403);
			this.settingsPage.TabIndex = 2;
			this.settingsPage.Text = "Logs";
			this.settingsPage.UseVisualStyleBackColor = true;
			// 
			// logsTab
			// 
			this.logsTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logsTab.Location = new System.Drawing.Point(3, 3);
			this.logsTab.Name = "logsTab";
			this.logsTab.Size = new System.Drawing.Size(743, 180);
			this.logsTab.TabIndex = 0;
			// 
			// unitsPage
			// 
			this.unitsPage.Controls.Add(this.unitsView);
			this.unitsPage.Location = new System.Drawing.Point(4, 22);
			this.unitsPage.Name = "unitsPage";
			this.unitsPage.Padding = new System.Windows.Forms.Padding(3);
			this.unitsPage.Size = new System.Drawing.Size(709, 403);
			this.unitsPage.TabIndex = 3;
			this.unitsPage.Text = "Units";
			this.unitsPage.UseVisualStyleBackColor = true;
			// 
			// unitsView
			// 
			this.unitsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.nameColumn,
									this.nodeColumn,
									this.serialColumn,
									this.statusColumn});
			this.unitsView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.unitsView.LabelEdit = true;
			this.unitsView.Location = new System.Drawing.Point(3, 3);
			this.unitsView.MultiSelect = false;
			this.unitsView.Name = "unitsView";
			this.unitsView.Size = new System.Drawing.Size(703, 397);
			this.unitsView.TabIndex = 0;
			this.unitsView.UseCompatibleStateImageBehavior = false;
			this.unitsView.View = System.Windows.Forms.View.Details;
			this.unitsView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.LogViewColumnClick);
			// 
			// nameColumn
			// 
			this.nameColumn.DisplayIndex = 2;
			this.nameColumn.Text = "Psiloc Name";
			this.nameColumn.Width = 235;
			// 
			// nodeColumn
			// 
			this.nodeColumn.DisplayIndex = 0;
			this.nodeColumn.Text = "Qui Node";
			this.nodeColumn.Width = 96;
			// 
			// serialColumn
			// 
			this.serialColumn.DisplayIndex = 1;
			this.serialColumn.Text = "Psiloc Serial";
			this.serialColumn.Width = 216;
			// 
			// statusColumn
			// 
			this.statusColumn.Text = "Status";
			this.statusColumn.Width = 133;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStripStatusLabel1,
									this.toolStripStatusLabel2,
									this.toolStripDropDownButton1,
									this.toolStripStatusLabel3,
									this.toolStripProgressBar1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 435);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(723, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(314, 17);
			this.toolStripStatusLabel1.Spring = true;
			this.toolStripStatusLabel1.Text = "Card Reader:";
			this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(92, 20);
			this.toolStripDropDownButton1.Text = "Disconnected";
			this.toolStripDropDownButton1.DropDownOpening += new System.EventHandler(this.ToolStripDropDownButton1DropDownOpening);
			this.toolStripDropDownButton1.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStripDropDownButton1DropDownItemClicked);
			// 
			// toolStripStatusLabel3
			// 
			this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
			this.toolStripStatusLabel3.Size = new System.Drawing.Size(50, 17);
			this.toolStripStatusLabel3.Text = "Activity:";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripProgressBar1.MarqueeAnimationSpeed = 10;
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(250, 16);
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenuStrip = this.notifyMenu;
			this.notifyIcon1.Icon = global::QuiRing.Icons.Icons_qui;
			this.notifyIcon1.Text = "QuiRing (Disconnected)";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1MouseClick);
			// 
			// notifyMenu
			// 
			this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.exitToolStripMenuItem,
									this.restoreToolStripMenuItem});
			this.notifyMenu.Name = "notifyMenu";
			this.notifyMenu.Size = new System.Drawing.Size(93, 26);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
			// 
			// restoreToolStripMenuItem
			// 
			this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
			this.restoreToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
			this.restoreToolStripMenuItem.Text = "Show";
			this.restoreToolStripMenuItem.Click += new System.EventHandler(this.RestoreToolStripMenuItemClick);
			// 
			// QuiRingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(723, 457);
			this.ContextMenuStrip = this.notifyMenu;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = global::QuiRing.Icons.Icons_qui;
			this.MinimumSize = new System.Drawing.Size(739, 245);
			this.Name = "QuiRingForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "QuiRing";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
			this.Resize += new System.EventHandler(this.MainFormResize);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.mainTabControl.ResumeLayout(false);
			this.usersPage.ResumeLayout(false);
			this.controlCardPage.ResumeLayout(false);
			this.settingsPage.ResumeLayout(false);
			this.unitsPage.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.notifyMenu.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private QuiRing.LogsTab logsTab;
		private QuiRing.CardsTab cardsTab;
		private QuiRing.UsersTab usersTab;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip notifyMenu;
		private System.Windows.Forms.ColumnHeader statusColumn;
		private System.Windows.Forms.ColumnHeader nameColumn;
		private System.Windows.Forms.ColumnHeader serialColumn;
		private System.Windows.Forms.ColumnHeader nodeColumn;
		private System.Windows.Forms.ListView unitsView;
		private System.Windows.Forms.TabPage unitsPage;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.TabPage settingsPage;
		private System.Windows.Forms.TabPage controlCardPage;
		private System.Windows.Forms.TabPage usersPage;
		private System.Windows.Forms.TabControl mainTabControl;

	}
}
