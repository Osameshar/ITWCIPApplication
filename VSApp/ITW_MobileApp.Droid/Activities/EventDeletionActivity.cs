using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.Support.V4.View;
using System.Linq;
using Android.Widget;


namespace ITW_MobileApp.Droid
{
    [Activity(Theme = "@style/MyTheme")]
    public class EventDeletionActivity : AppCompatActivity
    {

        Android.Support.V7.Widget.Toolbar _supporttoolbar;
        DrawerLayout _drawer;
        NavigationView _navigationview;

        ListView deletionListView;
        //This should eventually be the list inside of the EventItemAdapter
        List<EventItem> myEventList;
        //This is different from the EventItemAdapter, as this how to deal with the RecyclerView
        CheckBoxAdapter myCheckBoxAdapter;

        EventItemAdapter eventItemAdapter;
        RecipientListItemAdapter recipientListItemAdapter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EventDeletion);

            eventItemAdapter = new EventItemAdapter(this, Resource.Layout.EventDeletion);
            recipientListItemAdapter = new RecipientListItemAdapter(this, Resource.Layout.EventDeletion);
            deletionListView = FindViewById<ListView>(Resource.Id.listDeletion);

            await RefreshView();

            setupToolbar();                     

            myEventList = recipientListItemAdapter.getEventsByEmployeeID(IoC.UserInfo.EmployeeID, eventItemAdapter);

            //Plug in my adapter
            myCheckBoxAdapter = new CheckBoxAdapter(this,myEventList);
            deletionListView.Adapter = myCheckBoxAdapter;
            RegisterForContextMenu(deletionListView);

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
                intent.PutExtra("Time", myEventList.ElementAt(info.Position).EventTime);
                intent.PutExtra("Location", myEventList.ElementAt(info.Position).Location);
                intent.PutExtra("Category", myEventList.ElementAt(info.Position).Category);
                intent.PutExtra("Description", myEventList.ElementAt(info.Position).EventDescription);
                StartActivity(intent);
            }
            return true;
        }

        public void setupToolbar()
        {
            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _supporttoolbar.SetTitle(Resource.String.event_deletion);
            SetSupportActionBar(_supporttoolbar);
            _supporttoolbar.SetNavigationIcon(Resource.Drawable.ic_menu_white_24dp);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);


            _drawer = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);

            _navigationview = FindViewById<NavigationView>(Resource.Id.nav_view);

            _navigationview.NavigationItemSelected += (sender, e) =>

            {
                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_recentEvents:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            var intent = new Intent(this, typeof(RecentEventsActivity));
                            StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_createEvent:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            var intent = new Intent(this, typeof(EventCreationActivity));
                            StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_calendar:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            Console.WriteLine("calendar");
                            //switch to calendar view
                            //var intent = new Intent(this, typeof(EventCreationActivity));
                            //StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_overtime:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            Console.WriteLine("overtime");
                            //switch to overtime view
                            //var intent = new Intent(this, typeof(RecentEventsActivity));
                            //StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_settings:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            Console.WriteLine("settings");
                            //switch to settings view
                            //var intent = new Intent(this, typeof(RecentEventsActivity));
                            //StartActivity(intent);
                        }
                        break;
                    case Resource.Id.logoutitem:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            Console.WriteLine("logout");
                            //logout
                            var intent = new Intent(this, typeof(LoginActivity));
                            StartActivity(intent);
                        }
                        break;

                }
            };
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
            await IoC.Dbconnect.SyncAsync(pullData: true);
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(eventItemAdapter);
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(recipientListItemAdapter);
        }
        public override void OnBackPressed()
        {
            if (_drawer.IsDrawerOpen(GravityCompat.Start))
            {
                _drawer.CloseDrawer(GravityCompat.Start);
            }
            else {
                base.OnBackPressed();
            }
        }
    }
}