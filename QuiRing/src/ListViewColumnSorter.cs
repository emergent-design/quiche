using System;
using System.Collections;	
using System.Windows.Forms;

namespace QuiRing
{
	public class ListViewColumnSorter : IComparer
	{
		public int SortColumn 	{ get; set; }
		public SortOrder Order	{ get; set; }
		private CaseInsensitiveComparer ObjectCompare;
	
		public ListViewColumnSorter()
		{
			this.SortColumn = 0;
			this.Order = SortOrder.None;
			this.ObjectCompare = new CaseInsensitiveComparer();
		}
	
		public int Compare(object x, object y)
		{
			int compareResult = ObjectCompare.Compare(((ListViewItem)x).SubItems[SortColumn].Text, ((ListViewItem)y).SubItems[SortColumn].Text);
			return Order == SortOrder.Ascending ? compareResult : Order == SortOrder.Descending ? -compareResult : 0;		
		}
	}
}
