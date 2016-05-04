using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Android.Support.V4.View;
using System.Linq;

namespace ITW_MobileApp.Droid
{
    [Activity(Theme = "@style/MyTheme")]
    public class CalendarListActivity : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar _supporttoolbar;
        DrawerLayout _drawer;
        NavigationView _navigationview;

        ListView calendarListView;
        List<EventItem> myEventList;
        EventItemAdapter eventItemAdapter;
        CalendarAdapter myCalendarAdapter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            switch (IoC.UserInfo.Employee.PrivledgeLevel)
            {
                case "Admin":
                    {
                        SetContentView(Resource.Layout.CalendarList_Admin);
                        eventItemAdapter = new EventItemAdapter(this, Resource.Layout.CalendarList_Admin);
                        break;
                    }
                case "Moderator":
                    {
                        SetContentView(Resource.Layout.CalendarList_Moderator);
                        eventItemAdapter = new EventItemAdapter(this, Resource.Layout.CalendarList_Moderator);
                        break;
                    }
                default:
                    {
                        SetContentView(Resource.Layout.CalendarList_User);
                        eventItemAdapter = new EventItemAdapter(this, Resource.Layout.CalendarList_User);
                        break;
                    }
            }

            await RefreshView();
            FindViewById(Resource.Id.loadingPanel).Visibility = ViewStates.Gone;

            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _drawer = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            _navigationview = FindViewById<NavigationView>(Resource.Id.nav_view);
            ToolbarCreator toolbarCreator = new ToolbarCreator();
            toolbarCreator.setupToolbar(_supporttoolbar, _drawer, _navigationview, Resource.String.event_list, this);


            myEventList = eventItemAdapter.getEventsByEmployeeID();
            DateTime date = IoC.selectedDate;
            myEventList = filterByDate(date);
            calendarListView = FindViewById<ListView>(Resource.Id.listCalendar);           
            myCalendarAdapter = new CalendarAdapter(this, myEventList);
            calendarListView.Adapter = myCalendarAdapter;
            RegisterForContextMenu(calendarListView);


        }
        private List<EventItem> filterByDate(DateTime date)
        {
            List<EventItem> filteredList = new List<EventItem>();
            foreach (EventItem eventitem in myEventList)
            {
                if (eventitem.EventDate.Day == date.Day)
                {
                    filteredList.Add(eventitem);
                }
            }
            return filteredList;
        }
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.listCalendar)
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
                var intent = new Intent(this, typeof(CalendarViewActivity));
                StartActivity(intent);
            }
        }
    }
}