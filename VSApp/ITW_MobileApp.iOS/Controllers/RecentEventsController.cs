using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace ITW_MobileApp.iOS
{
    partial class RecentEventsController : UITableViewController
    {
        private UITableView table;
        private List<EventItem> myEventList;

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

            await IoC.UserInfo.setEmployee();
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
            sortByDate(myEventList);




            table = new UITableView(View.Bounds); // defaults to Plain style
            table.Source = new RecentEventTableSource(myEventList,this);
            table.ContentInset = new UIEdgeInsets(65, 0, 0, 0);
            Add(table);
            
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

        void OnItemClick(object sender, int position)
        {

        }

        //public bool IsPlayServicesAvailable()
        //{
        //    int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
        //    if (resultCode != ConnectionResult.Success)
        //    {
        //        if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
        //            error.CreateAndShowDialog(GoogleApiAvailability.Instance.GetErrorString(resultCode), "GoogleAPI");
        //        else
        //        {
        //            error.CreateAndShowDialog("Sorry, this device is not supported", "Unsupported device");
        //            Finish();
        //        }
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}
