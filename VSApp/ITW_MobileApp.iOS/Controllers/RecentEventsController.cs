using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;

namespace ITW_MobileApp.iOS
{
    partial class RecentEventsController : UITableViewController
    {
        private UITableView table;
        private List<EventItem> eventList;

        public RecentEventsController (IntPtr handle) : base (handle)
		{
            UIImage hamburgericon = UIImage.FromFile("Menu Filled-20");
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(hamburgericon, UIBarButtonItemStyle.Plain, delegate
            {
                ParentController.getNavigationMenu().ToggleMenu();
            });

            eventList = ParentController.getEventList();

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style
            table.Source = new RecentEventTableSource(eventList, this);
            Add(table);
        }
    }
}
