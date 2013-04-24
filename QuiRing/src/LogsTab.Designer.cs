namespace QuiRing
{
	partial class LogsTab
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
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.logView = new System.Windows.Forms.DataGridView();
			this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label5 = new System.Windows.Forms.Label();
			this.addressBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.portNumberSelection = new System.Windows.Forms.NumericUpDown();
			this.connectButton = new System.Windows.Forms.Button();
			this.exportButton = new System.Windows.Forms.Button();
			this.importButton = new System.Windows.Forms.Button();
			this.logButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel10.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.portNumberSelection)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.ColumnCount = 4;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel10.Controls.Add(this.logView, 0, 2);
			this.tableLayoutPanel10.Controls.Add(this.label5, 0, 0);
			this.tableLayoutPanel10.Controls.Add(this.addressBox, 1, 0);
			this.tableLayoutPanel10.Controls.Add(this.label6, 0, 1);
			this.tableLayoutPanel10.Controls.Add(this.portNumberSelection, 1, 1);
			this.tableLayoutPanel10.Controls.Add(this.connectButton, 3, 0);
			this.tableLayoutPanel10.Controls.Add(this.exportButton, 2, 3);
			this.tableLayoutPanel10.Controls.Add(this.importButton, 3, 3);
			this.tableLayoutPanel10.Controls.Add(this.logButton, 0, 3);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 4;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(150, 150);
			this.tableLayoutPanel10.TabIndex = 1;
			// 
			// logView
			// 
			this.logView.AutoGenerateColumns = false;
			this.logView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
			this.logView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
			this.logView.ReadOnly = true;
			this.logView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.Time,
									this.Event,
									this.User,
									this.Unit,
									this.Result});
			this.tableLayoutPanel10.SetColumnSpan(this.logView, 4);
			this.logView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logView.Location = new System.Drawing.Point(3, 53);
			this.logView.Name = "logView";
			this.logView.Size = new System.Drawing.Size(144, 59);
			this.logView.TabIndex = 5;
			//this.logView.UseCompatibleStateImageBehavior = false;
			//this.logView.View = System.Windows.Forms.View.Details;
			//this.logView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.LogViewColumnClick);
			// 
			// Time
			// 
			this.Time.HeaderText = "Time";
			this.Time.DataPropertyName = "Timestamp";
			this.Time.Width = 134;
			// 
			// Event
			// 
			this.Event.HeaderText = "Event";
			this.Event.DataPropertyName = "Event";
			this.Event.Width = 127;
			// 
			// User
			// 
			this.User.HeaderText = "User";
			this.User.DataPropertyName = "User";
			this.User.Width = 133;
			// 
			// Unit
			// 
			this.Unit.HeaderText = "Terminal";
			this.Unit.DataPropertyName = "Terminal";
			this.Unit.Width = 126;
			// 
			// Result
			// 
			this.Result.HeaderText = "Result";
			this.Result.DataPropertyName = "Result";
			this.Result.Width = 155;
			// 
			// label5
			// 
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(3, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(31, 25);
			this.label5.TabIndex = 0;
			this.label5.Text = "Qui Watchman Address";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// addressBox
			// 
			this.tableLayoutPanel10.SetColumnSpan(this.addressBox, 2);
			this.addressBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.addressBox.Location = new System.Drawing.Point(40, 3);
			this.addressBox.Name = "addressBox";
			this.addressBox.Size = new System.Drawing.Size(68, 20);
			this.addressBox.TabIndex = 2;
			// 
			// label6
			// 
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(3, 25);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(31, 25);
			this.label6.TabIndex = 1;
			this.label6.Text = "Qui Watchman Port";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// portNumberSelection
			// 
			this.tableLayoutPanel10.SetColumnSpan(this.portNumberSelection, 2);
			this.portNumberSelection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.portNumberSelection.Location = new System.Drawing.Point(40, 28);
			this.portNumberSelection.Maximum = new decimal(new int[] {
									10000,
									0,
									0,
									0});
			this.portNumberSelection.Name = "portNumberSelection";
			this.portNumberSelection.Size = new System.Drawing.Size(68, 20);
			this.portNumberSelection.TabIndex = 3;
			this.portNumberSelection.Value = new decimal(new int[] {
									8181,
									0,
									0,
									0});
			// 
			// connectButton
			// 
			this.connectButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.connectButton.Location = new System.Drawing.Point(114, 3);
			this.connectButton.Name = "connectButton";
			this.tableLayoutPanel10.SetRowSpan(this.connectButton, 2);
			this.connectButton.Size = new System.Drawing.Size(33, 44);
			this.connectButton.TabIndex = 6;
			this.connectButton.Text = "Connect";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.ConnectButtonClick);
			// 
			// exportButton
			// 
			this.exportButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.exportButton.Location = new System.Drawing.Point(77, 118);
			this.exportButton.Name = "exportButton";
			this.exportButton.Size = new System.Drawing.Size(31, 29);
			this.exportButton.TabIndex = 8;
			this.exportButton.Text = "Export Database";
			this.exportButton.UseVisualStyleBackColor = true;
			this.exportButton.Click += new System.EventHandler(this.ExportButtonClick);
			// 
			// importButton
			// 
			this.importButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.importButton.Location = new System.Drawing.Point(114, 118);
			this.importButton.Name = "importButton";
			this.importButton.Size = new System.Drawing.Size(33, 29);
			this.importButton.TabIndex = 9;
			this.importButton.Text = "Import Database";
			this.importButton.UseVisualStyleBackColor = true;
			this.importButton.Click += new System.EventHandler(this.ImportButtonClick);
			// 
			// logButton
			// 
			this.tableLayoutPanel10.SetColumnSpan(this.logButton, 2);
			this.logButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logButton.Location = new System.Drawing.Point(3, 118);
			this.logButton.Name = "logButton";
			this.logButton.Size = new System.Drawing.Size(68, 29);
			this.logButton.TabIndex = 10;
			this.logButton.Text = "Set log path";
			this.logButton.UseVisualStyleBackColor = true;
			this.logButton.Click += new System.EventHandler(this.LogButtonClick);
			// 
			// LogsTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel10);
			this.Name = "LogsTab";
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.portNumberSelection)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button logButton;
		private System.Windows.Forms.Button importButton;
		private System.Windows.Forms.Button exportButton;
		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.NumericUpDown portNumberSelection;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox addressBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DataGridViewTextBoxColumn Result;
		private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
		private System.Windows.Forms.DataGridViewTextBoxColumn User;
		private System.Windows.Forms.DataGridViewTextBoxColumn Event;
		private System.Windows.Forms.DataGridViewTextBoxColumn Time;
		private System.Windows.Forms.DataGridView logView;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
	}
}
