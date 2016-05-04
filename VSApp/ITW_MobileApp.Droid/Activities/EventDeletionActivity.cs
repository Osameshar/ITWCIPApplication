using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.Support.V4.View;
using System.Linq;
using Android.Widget;
using System;

namespace ITW_MobileApp.Droid
{
    [Activity(Theme = "@style/MyTheme")]
    public class EventDeletionActivity : AppCompatActivity
    {

        Android.Support.V7.Widget.Toolbar _supporttoolbar;
        DrawerLayout _drawer;
        NavigationView _navigationview;

        Button DeleteEventsBtn;
        List<EventItem> checkedEvents;

        ListView deletionListView;
        //This should eventually be the list inside of the EventItemAdapter
        List<EventItem> myEventList;
        //This is different from the EventItemAdapter, as this how to deal with the RecyclerView
        CheckBoxAdapter myCheckBoxAdapter;

        EventItemAdapter eventItemAdapter;

        EventDeleter eventDeleter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            switch (IoC.UserInfo.Employee.PrivledgeLevel)
            {
                case "Admin":
                    {
                        SetContentView(Resource.Layout.EventDeletion_Admin);
                        eventItemAdapter = new EventItemAdapter(this, Resource.Layout.EventDeletion_Admin);
                        break;
                    }
                case "Moderator":
                    {
                        SetContentView(Resource.Layout.EventDeletion_Moderator);
                        eventItemAdapter = new EventItemAdapter(this, Resource.Layout.EventDeletion_Moderator);
                        break;
                    }
            }

            eventDeleter = new EventDeleter();
            deletionListView = FindViewById<ListView>(Resource.Id.listDeletion);
            DeleteEventsBtn = FindViewById<Button>(Resource.Id.DeleteEventsBtn);

            await RefreshView();
            FindViewById(Resource.Id.loadingPanel).Visibility = ViewStates.Gone;

            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _drawer = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            _navigationview = FindViewById<NavigationView>(Resource.Id.nav_view);
            ToolbarCreator toolbarCreator = new ToolbarCreator();
            toolbarCreator.setupToolbar(_supporttoolbar, _drawer, _navigationview, Resource.String.event_deletion, this);

            if (IoC.UserInfo.Employee.PrivledgeLevel == "Admin")
            {
                myEventList = eventItemAdapter.getAllEventsNotDeleted(); 
            }
            else
            {
                myEventList = eventItemAdapter.getEventsByEmployeeID();
            }

            sortByDate(myEventList);
            //Plug in my adapter
            myCheckBoxAdapter = new CheckBoxAdapter(this,myEventList);
            deletionListView.Adapter = myCheckBoxAdapter;
            RegisterForContextMenu(deletionListView);

            DeleteEventsBtn.Click += delegate
            {
                checkedEvents = ((CheckBoxAdapter)deletionListView.Adapter).getCheckedList();
                deleteEvents(deletionListView.Adapter.Count);
            };

        }
        private void sortByDate(List<EventItem> eventList)
        {
            eventList.Sort((x, y) => DateTime.Compare(x.EventDate, y.EventDate));
            eventList.Reverse();
        }
        private void deleteEvents(int numItems)
        {

            foreach (EventItem eventItem in checkedEvents)
            {
                eventDeleter.deleteEvent(eventItem.EventID);
            }
            var intent = new Intent(this, typeof(EventDeletionActivity));
            StartActivity(intent);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.listDeletion)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
                menu.SetHeaderTitle("Event Options");
                
                var menuItems = Resources.GetStringArray(Resource.Array.eventdetailsmenu);
                for (var i = 0; i < menuItems.Length; i++)
                    menu.Add(Menu.None, i, i, menuItems[i]);
                
            }
        }
        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            var menuItemIndex = item.ItemId;
            var menuItems = Resources.GetStringArray(Resource.Array.eventdetailsmenu);
            var menuItemName = menuItems[menuItemIndex];
            if (menuItemIndex == 0)
            {
                var intent = new Intent(this, typeof(EventDetailsActivity));
                intent.PutExtra("Name", myEventList.ElementAt(info.Position).Name);
                intent.PutExtra("Date", myEventList.ElementAt(info.Position).EventDate.ToString("MMMM dd, yyyy"));
                intent.PutExtra("Time", myEventList.ElementAt(info.Position).EventDate.ToString("h:mm tt"));
                intent.PutExtra("Location", myEventList.ElementAt(info.Position).Location);
                intent.PutExtra("Category", myEventList.ElementAt(info.Position).Category);
                intent.PutExtra("Description", myEventList.ElementAt(info.Position).EventDescription);
                StartActivity(intent);
            }
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _drawer.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public async Task RefreshView()
        {
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(eventItemAdapter);
        }
        public override void OnBackPressed()
        {
            if (_drawer.IsDrawerOpen(GravityCompat.Start))
            {
                _drawer.CloseDrawer(GravityCompat.Start);
            }
            else {
                var intent = new Intent(this, typeof(RecentEventsActivity));
                StartActivity(intent);
            }
        }
    }
}