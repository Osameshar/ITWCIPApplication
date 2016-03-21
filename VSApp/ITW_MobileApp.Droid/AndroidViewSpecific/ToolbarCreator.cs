using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V4.View;

namespace ITW_MobileApp.Droid
{
    class ToolbarCreator
    {
        public void setupToolbar(Toolbar _supporttoolbar, DrawerLayout _drawer, NavigationView _navigationview, int titleID, AppCompatActivity context)
        {

            _supporttoolbar.SetTitle(titleID);
            context.SetSupportActionBar(_supporttoolbar);
            _supporttoolbar.SetNavigationIcon(Resource.Drawable.ic_menu_white_24dp);

            context.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            ErrorHandler error = new ErrorHandler(context);
            _navigationview.NavigationItemSelected += (sender, e) =>
            
            {
                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_recentEvents:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            var intent = new Intent(context, typeof(RecentEventsActivity));
                            context.StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_createEvent:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            var intent = new Intent(context, typeof(EventCreationActivity));
                            context.StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_deleteEvent:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            //switch to calendar view
                            var intent = new Intent(context, typeof(EventDeletionActivity));
                            context.StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_calendar:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            error.CreateAndShowDialog("Calender not yet implemented.", "Work In Progress");
                            //switch to calendar view
                            //var intent = new Intent(context, typeof(EventDeletionActivity));
                            //context.StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_overtime:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            error.CreateAndShowDialog("Overtime not yet implemented.", "Work In Progress");
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
                            var intent = new Intent(context, typeof(LoginActivity));
                            context.StartActivity(intent);
                        }
                        break;

                }
            };
        }
    }
}