using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace ITW_MobileApp.iOS
{
    public class TableSource : UITableViewSource
    {

        List<EventItem> TableItems;
        string CellIdentifier = "TableCell";

        public TableSource(List<EventItem> items)
        {
            TableItems = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string tLabel  = TableItems[indexPath.Row].Name;
            string dLabel = TableItems[indexPath.Row].EventDate.ToString();

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Value1, CellIdentifier); }

            cell.TextLabel.Text = tLabel;
            cell.DetailTextLabel.Text = dLabel;

            return cell;
        }
    }
}
