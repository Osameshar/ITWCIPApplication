using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
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
            setupToolbar();

            Name = FindViewById<TextView>(Resource.Id.NameText);
            Date = FindViewById<TextView>(Resource.Id.DateText);
            Time = FindViewById<TextView>(Resource.Id.TimeText);
            Location = FindViewById<TextView>(Resource.Id.LocationText);
            Category = FindViewById<TextView>(Resource.Id.Category);
            Description = FindViewById<TextView>(Resource.Id.DescriptionText);

            Name.Text = Intent.GetStringExtra("Name") ?? "Data not available";
            Date.Text = Intent.GetStringExtra("Date") ?? "Data not available";
            Time.Text = Intent.GetStringExtra("Time") ?? "Data not available";
            Location.Text = Intent.GetStringExtra("Location") ?? "Data not available";
            Category.Text = Intent.GetStringExtra("Category") ?? "Data not available";
            Description.Text = Intent.GetStringExtra("Description") ?? "Data not available";

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
        public void setupToolbar()
        {
            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _supporttoolbar.SetTitle(Resource.String.event_details);
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