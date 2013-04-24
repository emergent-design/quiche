using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.Linq;


using Quiche.Data;

namespace QuiRing
{
	/// <summary>
	/// Description of FindUserForm.
	/// </summary>
	public partial class FindUserForm : Form
	{
		public User selected;
		protected List<User> candidates = new List<User>();
		protected ListViewColumnSorter sorter = new ListViewColumnSorter();


		public FindUserForm(List<User> candidates)
		{
			InitializeComponent();
			this.resultsTable.ListViewItemSorter = sorter;
			this.candidates = candidates;

			this.resultsTable.BeginUpdate();
			foreach(User user in this.candidates)
			{
				this.resultsTable.Items.Add(new ListViewItem(new []{user.Name, user.Id.ToString(), user.LastIssued.ToString()}));
			}
			this.resultsTable.EndUpdate();
		}


		void CancelButtonClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}


		void OkButtonClick(object sender, EventArgs e)
		{
			if (this.resultsTable.SelectedItems.Count>0)
			{
				this.selected = this.candidates[this.resultsTable.SelectedIndices[0]];
				this.DialogResult = DialogResult.OK;
			}
			else this.DialogResult = DialogResult.Cancel;
		}


		void NameSearchBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return)
	        {
				if(this.candidates.Count > 0)
				{
					this.resultsTable.Focus();
					this.resultsTable.Items[0].Selected = true;
					this.selected = this.candidates[0];
				}
	        }
		}
		
		void ResultsTableKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return)
	        	this.DialogResult = this.SelectUser() ? DialogResult.OK : DialogResult.Cancel;
		}
		
		protected bool SelectUser()
		{
			if (this.resultsTable.SelectedItems.Count>0)
			{
				this.selected = this.candidates.Find(c => c.Id == this.resultsTable.SelectedItems[0].SubItems[1].Text);
				return this.selected!=null ? true: false;
			}
			return false;
		}
		
		void ResultsTableMouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.DialogResult = this.SelectUser() ? DialogResult.OK : DialogResult.Cancel;
		}
		
		void NameSearchBoxTextChanged(object sender, EventArgs e)
		{
			List<User> shortlist = this.candidates.FindAll(candidates => candidates.Name.IndexOf(this.nameSearchBox.Text, StringComparison.InvariantCultureIgnoreCase) >= 0);
			this.resultsTable.BeginUpdate();
			this.resultsTable.Items.Clear();
			foreach(User user in shortlist)
			{
				this.resultsTable.Items.Add(new ListViewItem(new []{user.Name, user.Id.ToString(), user.LastIssued.ToString()}));
			}
			this.resultsTable.EndUpdate();
		}
		
		void ResultsTableColumnClick(object sender, ColumnClickEventArgs e)
		{			
			if ( e.Column == sorter.SortColumn ) sorter.Order = sorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
			else
			{
				sorter.SortColumn = e.Column;
				sorter.Order = SortOrder.Ascending;
			}
			this.resultsTable.Sort();			
		}
	}
}
