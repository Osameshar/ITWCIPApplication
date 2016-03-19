using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using ITW_MobileApp.Droid;
using Android.Support.V4.View;

namespace ITW_MobileApp
{
    [Activity(Theme = "@style/MyTheme")]
    public class EventDetailsActivity : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar _supporttoolbar;
        DrawerLayout _drawer;
        NavigationView _navigationview;

        public TextView Name { get; private set; }
        public TextView Date { get; private set; }
        public TextView Time { get; private set; }
        public TextView Location { get; private set; }
        public TextView Category { get; private set; }
        public TextView Description { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EventDetails);

            //setup toolbar
            
            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _drawer = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            _navigationview = FindViewById<NavigationView>(Resource.Id.nav_view);
            ToolbarCreator toolbarCreator = new ToolbarCreator();
            toolbarCreator.setupToolbar(_supporttoolbar, _drawer, _navigationview, Resource.String.event_details, this);

            Name = FindViewById<TextView>(Resource.Id.Name);
            Date = FindViewById<TextView>(Resource.Id.Date);
            Time = FindViewById<TextView>(Resource.Id.Time);
            Location = FindViewById<TextView>(Resource.Id.Location);
            Category = FindViewById<TextView>(Resource.Id.Category);
            Description = FindViewById<TextView>(Resource.Id.Description);

            Name.Text += Intent.GetStringExtra("Name") ?? "Data not available";
            Date.Text += Intent.GetStringExtra("Date") ?? "Data not available";
            Time.Text += Intent.GetStringExtra("Time") ?? "Data not available";
            Location.Text += Intent.GetStringExtra("Location") ?? "Data not available";
            Category.Text = Intent.GetStringExtra("Category") ?? "Data not available";
            Description.Text += Intent.GetStringExtra("Description") ?? "Data not available";

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