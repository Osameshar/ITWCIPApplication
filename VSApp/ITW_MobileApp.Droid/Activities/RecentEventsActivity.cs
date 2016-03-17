using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using System.Threading.Tasks;
using Android.Content;
using Android.Support.V4.View;
using System.Linq;

namespace ITW_MobileApp.Droid
{
    [Activity(Theme = "@style/MyTheme")]
    public class RecentEventsActivity : AppCompatActivity
    {

        Android.Support.V7.Widget.Toolbar _supporttoolbar;
        DrawerLayout _drawer;
        NavigationView _navigationview;

        //this section starts off objects for the recycler view
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        //This should eventually be the list inside of the EventItemAdapter
        List<EventItem> myEventList;
        //This is different from the EventItemAdapter, as this how to deal with the RecyclerView
        EventListAdapter myEventListAdapter;

        EventItemAdapter eventItemAdapter;
        RecipientListItemAdapter recipientListItemAdapter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            eventItemAdapter = new EventItemAdapter(this, Resource.Layout.Main);
            recipientListItemAdapter = new RecipientListItemAdapter(this, Resource.Layout.Main);

            await RefreshView();

            setupToolbar();
            //IoC.EventFactory.createEvent("MyEvent", "Curtis Keller", new DateTime(2016, 3, 3), "Noon", "Nashville", "Company Event", "High", "PARTY AT MARLEY'S", 1, 2);
            //IoC.EventFactory.createEvent("MyEvent2", "Curtis Keller,Alan Keller", new DateTime(2016, 3, 3), "Noon", "Nashville", "Emergency", "High", "PARTY AT MARLEY'S", 2, 2);
            //IoC.EventFactory.createEvent("MyEvent3", "Curtis Keller", new DateTime(2016, 3, 3), "Noon", "Nashville", "Meeting", "High", "PARTY AT MARLEY'S", 3, 2);
            //IoC.EventFactory.createEvent("MyEvent4", "Curtis Keller", new DateTime(2016, 3, 3), "Noon", "Nashville", "Machine Maintenance", "High", "PARTY AT MARLEY'S", 4, 2);

            //Here is where we do the Recyler View
            //Starting it off
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            await RefreshView();

            myEventList = recipientListItemAdapter.getEventsByEmployeeID(IoC.UserInfo.EmployeeID, eventItemAdapter);

            //Plug in the linear layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            //Plug in my adapter
            myEventListAdapter = new EventListAdapter(myEventList);
            myEventListAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(myEventListAdapter);

        }

        public void setupToolbar()
        {
            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _supporttoolbar.SetTitle(Resource.String.recent_events);
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
        //This has nothing to do with this Recycler View. Instead, this deals with the seletion of the Navigation Drawer button
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
        void OnItemClick(object sender, int position)
        {
            //int eventNum = position + 1;
            //Toast.MakeText(this, "This is event number " + eventNum, ToastLength.Short).Show();
            var intent = new Intent(this, typeof(EventDetailsActivity));
            intent.PutExtra("Name", myEventList.ElementAt(position).Name);
            intent.PutExtra("Date", myEventList.ElementAt(position).EventDate.ToString("MMMM dd, yyyy"));
            intent.PutExtra("Time", myEventList.ElementAt(position).EventTime);
            intent.PutExtra("Location", myEventList.ElementAt(position).Location);
            intent.PutExtra("Category", myEventList.ElementAt(position).Category);
            intent.PutExtra("Description", myEventList.ElementAt(position).EventDescription);
            StartActivity(intent);
        }
    }
}


