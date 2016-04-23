using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace ITW_MobileApp.iOS
{
    public class RecentEventTableSource : UITableViewSource
    {

        List<EventItem> TableItems;
        string CellIdentifier = "TableCell";
        UITableViewController owner;

        public RecentEventTableSource(List<EventItem> items, UITableViewController owner)
        {
            TableItems = items;
            this.owner = owner;
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
            string category = TableItems[indexPath.Row].Category;

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier); }

            cell.TextLabel.Text = tLabel;
            cell.DetailTextLabel.Text = dLabel;

            if (category == "Meeting")
            {
                cell.ImageView.Image = UIImage.FromFile("Meeting");
            }

            else if (category == "Company Event")
            {
                cell.ImageView.Image = UIImage.FromFile("Company");
            }

            else if (category == "Emergency")
            {
                cell.ImageView.Image = UIImage.FromFile("Emergency");
            }

            else if (category == "Machine Maintenance")
            {
                cell.ImageView.Image = UIImage.FromFile("MachineMaintenance");
            }

            else
            {
                cell.ImageView.Image = UIImage.FromFile("Meeting");
            }

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //UIAlertController okAlertController = UIAlertController.Create("Row Selected", TableItems[indexPath.Row].Name, UIAlertControllerStyle.Alert);
            //okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            EventDetailsController eventDetails = owner.Storyboard.InstantiateViewController("EventDetailsController") as EventDetailsController;
            owner.NavigationController.PushViewController(eventDetails, true);

            tableView.DeselectRow(indexPath, true);
        }

    }

}
