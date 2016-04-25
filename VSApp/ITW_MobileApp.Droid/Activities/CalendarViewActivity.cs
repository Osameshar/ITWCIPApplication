using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V4.View;
using System.Threading.Tasks;

namespace ITW_MobileApp.Droid
{
    [Activity(Theme = "@style/MyTheme")]
    public class CalendarViewActivity : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar _supporttoolbar;
        DrawerLayout _drawer;
        NavigationView _navigationview;

        EventItemAdapter eventItemAdapter;
        RecipientListItemAdapter recipientListItemAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            switch (IoC.UserInfo.Employee.PrivledgeLevel)
            {
                case "Admin":
                    {
                        SetContentView(Resource.Layout.RecentEvents_Admin);
                        eventItemAdapter = new EventItemAdapter(this, Resource.Layout.RecentEvents_Admin);
                        recipientListItemAdapter = new RecipientListItemAdapter(this, Resource.Layout.RecentEvents_Admin);
                        break;
                    }
                case "Moderator":
                    {
                        SetContentView(Resource.Layout.RecentEvents_Moderator);
                        eventItemAdapter = new EventItemAdapter(this, Resource.Layout.RecentEvents_Moderator);
                        recipientListItemAdapter = new RecipientListItemAdapter(this, Resource.Layout.RecentEvents_Moderator);
                        break;
                    }
                default:
                    {
                        SetContentView(Resource.Layout.RecentEvents_User);
                        eventItemAdapter = new EventItemAdapter(this, Resource.Layout.RecentEvents_User);
                        recipientListItemAdapter = new RecipientListItemAdapter(this, Resource.Layout.RecentEvents_User);
                        break;
                    }
            }

            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _drawer = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            _navigationview = FindViewById<NavigationView>(Resource.Id.nav_view);
            ToolbarCreator toolbarCreator = new ToolbarCreator();
            toolbarCreator.setupToolbar(_supporttoolbar, _drawer, _navigationview, Resource.String.create_event, this);

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
                var intent = new Intent(this, typeof(RecentEventsActivity));
                StartActivity(intent);
            }
        }
        public async Task RefreshView()
        {
            await IoC.Dbconnect.SyncAsync(pullData: true);
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(eventItemAdapter);
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(recipientListItemAdapter);
        }
    }
}