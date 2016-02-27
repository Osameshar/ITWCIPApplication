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
        //This section here is just an example use for the Recycler View to get it working. Overtime, we will switch this out for events.
        List<string> mystringlist;
        StringListAdapter mystringlistadapter;

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

            //Adding strings to mystringlist, which is an example use for the recycler view
            mystringlist = new List<string>();
            mystringlist.Add("Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("How About Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");
            mystringlist.Add("And Another One");

            //Plug in the linear layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            //Plug in my adapter
            mystringlistadapter = new StringListAdapter(mystringlist);
            mRecyclerView.SetAdapter(mystringlistadapter);

        }

        //creates an options menu
        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.navigationview_menu, menu);
        //    return true;
        //}

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

    public class StringViewHolder : RecyclerView.ViewHolder
    {
        public TextView Caption { get; private set; }

        public StringViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            Caption = itemView.FindViewById<TextView>(Resource.Id.textView);
        }
    }

    public class StringListAdapter : RecyclerView.Adapter
    {
        public List<string> adapterstringlist;

        public StringListAdapter(List<string> mystringlist)
        {
           adapterstringlist = mystringlist;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.StringCardView, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            StringViewHolder vh = new StringViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StringViewHolder vh = holder as StringViewHolder;

            // Load the photo caption from the photo album:
            vh.Caption.Text = adapterstringlist.ElementAt(position);

        }

        public override int ItemCount
        {
            get { return adapterstringlist.Count; }
        }
    }
}


