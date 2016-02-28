using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
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

        private EmployeeItemAdapter employeeItemAdapter;
        private EventItemAdapter eventItemAdapter;
        private RecipientListItemAdapter recipientListItemAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            employeeItemAdapter = new EmployeeItemAdapter(this, Resource.Layout.Row_List_To_Do);
            eventItemAdapter = new EventItemAdapter(this, Resource.Layout.Row_List_To_Do);
            recipientListItemAdapter = new RecipientListItemAdapter(this, Resource.Layout.Row_List_To_Do);

            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _supporttoolbar.SetTitle(Resource.String.recent_events);
            SetSupportActionBar(_supporttoolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _drawer = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);

            _navigationview = FindViewById<NavigationView>(Resource.Id.nav_view);

            _navigationview.NavigationItemSelected += (sender, e) =>

            {
                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_haveRead:
                        Console.WriteLine("Have Read");
                        break;
                    case Resource.Id.nav_readingNow:
                        Console.WriteLine("Reading Now");
                        break;

                }
            };

            //Here is where we do the Recyler View
            //Starting it off
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            //Initialize the list of events
            myEventList = new List<EventItem>();

            EventItem myevent = new EventItem();
            myevent.Name = "My Event";
            myevent.EventRecipients = "Bob, Same, and Marley";
            myevent.EventDate = new DateTime(2016, 3, 3);
            myevent.EventTime = "Noon";
            myevent.Location = "Nashville";
            myevent.Category = "Company Event";
            myevent.EventPriority = "Now";
            myevent.EventDescription = "PARTY AT MARLEY'S";
            myevent.EventID = 1;
            myevent.EmployeeID = 0078982;
            myevent.deleted = false;

            myEventList.Add(myevent);

            //Plug in the linear layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            //Plug in my adapter
            myEventListAdapter = new EventListAdapter(myEventList);
            mRecyclerView.SetAdapter(myEventListAdapter);

        }

        //This has nothing to do with this Recycler View. Instead, this deals with the seletion of the Navigation Drawer button
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _drawer.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }

    public class EventViewHolder: RecyclerView.ViewHolder
    {
        public TextView Name { get; private set; }
        public TextView Date { get; private set; }
        public TextView Time { get; private set; }

        public EventViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            Name = itemView.FindViewById<TextView>(Resource.Id.Name);
            Date = itemView.FindViewById<TextView>(Resource.Id.Date);
            Time = itemView.FindViewById<TextView>(Resource.Id.Time);
        }
    }

    public class EventListAdapter : RecyclerView.Adapter
    {
        public List<EventItem> adaptereventlist;

        public EventListAdapter(List<EventItem> myEventList)
        {
            adaptereventlist = myEventList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                           //TODO: Use if statements to figure out a different EventCardView to inflate for each Category
                        Inflate(Resource.Layout.EventCardView, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            EventViewHolder vh = new EventViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            EventViewHolder vh = holder as EventViewHolder;

            // Load the photo caption from the photo album:
            vh.Name.Text = adaptereventlist.ElementAt(position).Name;
            vh.Date.Text = adaptereventlist.ElementAt(position).EventDate.ToString("MMMM dd, yyyy");
            vh.Time.Text = adaptereventlist.ElementAt(position).EventTime;
        }

        public override int ItemCount
        {
            get { return adaptereventlist.Count; }
        }
    }
}


