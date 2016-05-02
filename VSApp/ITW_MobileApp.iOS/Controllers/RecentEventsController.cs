using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace ITW_MobileApp.iOS
{
    partial class RecentEventsController : UITableViewController
    {
        private UITableView table;
        private List<EventItem> myEventList;
        private List<EventItem> notificationEventList;
        EventItemAdapter eventItemAdapter;
        RecipientListItemAdapter recipientListItemAdapter;

        public RecentEventsController (IntPtr handle) : base (handle)
		{
            UIImage hamburgericon = UIImage.FromFile("Menu Filled-20");
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(hamburgericon, UIBarButtonItemStyle.Plain, delegate
            {
                ParentController.getNavigationMenu().ToggleMenu();
            });
            eventItemAdapter = new EventItemAdapter();
            recipientListItemAdapter = new RecipientListItemAdapter();
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            
            //if (IsPlayServicesAvailable())
            //{
            //    var intentRegistration = new Intent(this, typeof(RegistrationIntentService));
            //    StartService(intentRegistration);
            //}

            var bounds = UIScreen.MainScreen.Bounds;
            LoadingOverlay loadingOverlay = new LoadingOverlay(bounds, "Loading events...");
            View.Add(loadingOverlay);

            await RefreshView();

            loadingOverlay.Hide();

            myEventList = recipientListItemAdapter.getEventsByEmployeeID(IoC.UserInfo.EmployeeID, eventItemAdapter);
            myEventList = filterEvents();
            sortByDate(myEventList);

            table = new UITableView(View.Bounds); // defaults to Plain style
            table.Source = new RecentEventTableSource(myEventList,this);
            table.ContentInset = new UIEdgeInsets(65, 0, 0, 0);
            Add(table);
            
        }
        public override async void ViewDidAppear(bool animated)
        {
            var bounds = UIScreen.MainScreen.Bounds;
            LoadingOverlay loadingOverlay = new LoadingOverlay(bounds, "Loading events...");
            View.Add(loadingOverlay);

            await RefreshView();

            loadingOverlay.Hide();

            myEventList = recipientListItemAdapter.getEventsByEmployeeID(IoC.UserInfo.EmployeeID, eventItemAdapter);
            notificationEventList = filterNotificationEvents(myEventList);
            
            //ThreadStart myThreadDelegate = new ThreadStart(createNotifications);
            //Thread setupNotifications = new Thread(myThreadDelegate);
            // setupNotifications.Start();
            createNotifications();
            myEventList = filterEvents();
            sortByDate(myEventList);
            base.ViewDidAppear(animated);
            table = new UITableView(View.Bounds); // defaults to Plain style
            table.Source = new RecentEventTableSource(myEventList, this);
            table.ContentInset = new UIEdgeInsets(65, 0, 0, 0);
            Add(table);
        }

        public void createNotifications()
        {
            if (notificationEventList.Count != UIApplication.SharedApplication.ScheduledLocalNotifications.Length) {
                foreach (EventItem item in notificationEventList)
                {

                    UILocalNotification notification = new UILocalNotification();
                    double seconds = (item.EventDate - DateTime.Now).TotalSeconds - 1800;
                    notification.FireDate = NSDate.FromTimeIntervalSinceNow(seconds);
                    notification.SoundName = UILocalNotification.DefaultSoundName;
                    notification.ApplicationIconBadgeNumber = UIApplication.SharedApplication.ApplicationIconBadgeNumber++;
                    notification.AlertAction = item.Category;
                    notification.AlertBody = item.Name + ": scheduled within 30 minutes.";
                    UIApplication.SharedApplication.ScheduleLocalNotification(notification);
                   
                }
            }
        }

        public void sortByDate(List<EventItem> eventList)
        {
            eventList.Sort((x, y) => DateTime.Compare(x.EventDate, y.EventDate));
        }

        public async Task RefreshView()
        {
            await IoC.Dbconnect.SyncAsync(pullData: true);
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(eventItemAdapter);
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(recipientListItemAdapter);
        }
        private List<EventItem> filterNotificationEvents(List<EventItem> list)
        {
            List<EventItem> newList = new List<EventItem>();

            foreach (EventItem item in list)
            {
                if ((item.EventDate > DateTime.Now) && item.IsDeleted == false)
                {
                    newList.Add(item);
                }
            }
            return newList;
        }
        private List<EventItem> filterEvents()
        {
            List<EventItem> filteredList = new List<EventItem>();
            foreach (EventItem eventitem in myEventList)
            {
                if (eventitem.EventDate.DayOfYear >= DateTime.Now.DayOfYear)
                {
                    foreach (string filter in IoC.ViewRefresher.FilterStringList)
                    {
                        if (eventitem.Category == filter)
                        {
                            filteredList.Add(eventitem);
                        }
                    }
                    if (eventitem.Category == "Emergency")
                    {
                        filteredList.Add(eventitem);
                    }
                }
            }
            return filteredList;
        }
    }
}
