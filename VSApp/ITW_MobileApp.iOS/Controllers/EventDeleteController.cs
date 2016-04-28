using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using MonoTouch.Dialog;
using CoreGraphics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITW_MobileApp.iOS
{
    public partial class EventDeleteController : UITableViewController
    {
        private UITableView table;
        private List<EventItem> eventList;
        UIBarButtonItem edit, done;
        EventItemAdapter eventItemAdapter;

        public EventDeleteController(IntPtr handle) : base(handle)
        {
            UIImage hamburgericon = UIImage.FromFile("Menu Filled-20");
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(hamburgericon, UIBarButtonItemStyle.Plain, delegate
            {
                ParentController.getNavigationMenu().ToggleMenu();
            });

            eventItemAdapter = new EventItemAdapter();


        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            await RefreshView();
            eventList = eventItemAdapter.getEventsByEmployeeID();
            sortByDate(eventList);

            table = new UITableView(View.Bounds); // defaults to Plain style
            table.Source = new DeleteEventTableSource(eventList,this);
            table.ContentInset = new UIEdgeInsets(65, 0, 0, 0);
            Add(table);
            table.SetEditing(true, true);

        }

        private void sortByDate(List<EventItem> eventList)
        {
            eventList.Sort((x, y) => DateTime.Compare(x.EventDate, y.EventDate));
            eventList.Reverse();
        }

        public async Task RefreshView()
        {
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(eventItemAdapter);
        }

    }
}