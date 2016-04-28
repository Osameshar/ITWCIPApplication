using Foundation;
using MonoTouch.Dialog;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace ITW_MobileApp.iOS
{
    partial class CalendarListController : UITableViewController
    {
        private UITableView table;
        private List<EventItem> myEventList;

        EventItemAdapter eventItemAdapter;
        RecipientListItemAdapter recipientListItemAdapter;

        public CalendarListController(List<EventItem> eventList) : base()
        {
            UIImage hamburgericon = UIImage.FromFile("Menu Filled-20");
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, delegate
            {
                NavigationController.PopViewController(true);
            });
            eventItemAdapter = new EventItemAdapter();
            recipientListItemAdapter = new RecipientListItemAdapter();

            myEventList = eventList;
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();


            //if (IsPlayServicesAvailable())
            //{
            //    var intentRegistration = new Intent(this, typeof(RegistrationIntentService));
            //    StartService(intentRegistration);
            //}


            await RefreshView();
            
            table = new UITableView(View.Bounds); // defaults to Plain style
            table.Source = new RecentEventTableSource(myEventList, this);
            table.ContentInset = new UIEdgeInsets(65, 0, 0, 0);
            Add(table);

        }
        public override async void ViewDidAppear(bool animated)
        {

            await RefreshView();

            base.ViewDidAppear(animated);
            table = new UITableView(View.Bounds); // defaults to Plain style
            table.Source = new CalendarListTableSource(myEventList, this);
            table.ContentInset = new UIEdgeInsets(65, 0, 0, 0);
            Add(table);
        }

        public async Task RefreshView()
        {
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(eventItemAdapter);
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(recipientListItemAdapter);
        }


    }
}
