namespace QuiRing
{
	partial class FindUserForm
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.resultsTable = new System.Windows.Forms.ListView();
			this.User = new System.Windows.Forms.ColumnHeader();
			this.Id = new System.Windows.Forms.ColumnHeader();
			this.IssueTime = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.nameSearchBox = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
			this.tableLayoutPanel1.Controls.Add(this.cancelButton, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.okButton, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.resultsTable, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.nameSearchBox, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(539, 311);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cancelButton.Location = new System.Drawing.Point(363, 284);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(173, 24);
			this.cancelButton.TabIndex = 8;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
			// 
			// okButton
			// 
			this.okButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.okButton.Location = new System.Drawing.Point(180, 284);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(177, 24);
			this.okButton.TabIndex = 7;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.OkButtonClick);
			// 
			// resultsTable
			// 
			this.resultsTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.User,
									this.Id,
									this.IssueTime});
			this.tableLayoutPanel1.SetColumnSpan(this.resultsTable, 3);
			this.resultsTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultsTable.FullRowSelect = true;
			this.resultsTable.Location = new System.Drawing.Point(3, 28);
			this.resultsTable.MultiSelect = false;
			this.resultsTable.Name = "resultsTable";
			this.resultsTable.Size = new System.Drawing.Size(533, 250);
			this.resultsTable.TabIndex = 5;
			this.resultsTable.UseCompatibleStateImageBehavior = false;
			this.resultsTable.View = System.Windows.Forms.View.Details;
			this.resultsTable.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ResultsTableColumnClick);
			this.resultsTable.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ResultsTableKeyPress);
			this.resultsTable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ResultsTableMouseDoubleClick);
			// 
			// User
			// 
			this.User.Text = "User";
			this.User.Width = 170;
			// 
			// Id
			// 
			this.Id.Text = "Id";
			this.Id.Width = 136;
			// 
			// IssueTime
			// 
			this.IssueTime.Text = "Issue Time";
			this.IssueTime.Width = 207;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(171, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nameSearchBox
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.nameSearchBox, 2);
			this.nameSearchBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nameSearchBox.Location = new System.Drawing.Point(180, 3);
			this.nameSearchBox.Name = "nameSearchBox";
			this.nameSearchBox.Size = new System.Drawing.Size(356, 20);
			this.nameSearchBox.TabIndex = 1;
			this.nameSearchBox.TextChanged += new System.EventHandler(this.NameSearchBoxTextChanged);
			this.nameSearchBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NameSearchBoxKeyPress);
			// 
			// FindUserForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(539, 311);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(555, 349);
			this.Name = "FindUserForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FindUserForm";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ColumnHeader IssueTime;
		private System.Windows.Forms.ColumnHeader Id;
		private System.Windows.Forms.ColumnHeader User;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ListView resultsTable;
		private System.Windows.Forms.TextBox nameSearchBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
