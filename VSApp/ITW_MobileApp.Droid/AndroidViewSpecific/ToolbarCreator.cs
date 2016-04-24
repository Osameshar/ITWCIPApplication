using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Webkit;
using System.Collections.Generic;

namespace ITW_MobileApp.Droid
{
    class ToolbarCreator
    {
        public AppCompatActivity ViewContext;
        public List<int> selectedItems = new List<int>();
        public string[] items = { "Meeting", "Company Event", "Machine Maintenance" };

        public void setupToolbar(Toolbar _supporttoolbar, DrawerLayout _drawer, NavigationView _navigationview, int titleID, AppCompatActivity context)
        {
            ViewContext = context;
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
                    case Resource.Id.nav_createEmployee:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            error.CreateAndShowDialog("Create Employee not yet implemented.", "Work In Progress");
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
                    case Resource.Id.nav_filter:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            spawnFilterDialog();
                            //error.CreateAndShowDialog("Settings not yet implemented.", "Work In Progress");
                            //switch to settings view
                            //var intent = new Intent(this, typeof(RecentEventsActivity));
                            //StartActivity(intent);
                        }
                        break;
                    case Resource.Id.logoutitem:
                        {
                            _drawer.CloseDrawer(GravityCompat.Start);
                            OnLogoutClicked();
                            var intent = new Intent(context, typeof(LoginActivity));
                            context.StartActivity(intent);
                        }
                        break;

                }
            };
        }

        private void spawnFilterDialog()
        {
            selectedItems.Add(0);
            selectedItems.Add(1);
            selectedItems.Add(2);
            var builder = new AlertDialog.Builder(ViewContext)
                .SetTitle("Filter Events")
                .SetMultiChoiceItems(items, new bool[] {true,true,true}, MultiListClicked);
            builder.SetPositiveButton("Ok", OkClicked);
            builder.SetNegativeButton("Cancel", CancelClicked);
            builder.Create();
            builder.Show();
        }
        private void OkClicked(object sender, DialogClickEventArgs e)
        {
            List<string> filteredItems = new List<string>();
            foreach (int index in selectedItems)
            {
                filteredItems.Add(items[index]);
            }
            IoC.ViewRefresher.FilterStringList = filteredItems;
            var intent = new Intent(ViewContext, typeof(RecentEventsActivity));
            ViewContext.StartActivity(intent);
        }

        private void CancelClicked(object sender, DialogClickEventArgs e)
        {
        }
        private void MultiListClicked(object sender, DialogMultiChoiceClickEventArgs e)
        {
            

            if (e.IsChecked)
            {
                // If the user checked the item, add it to the selected items
                selectedItems.Add(e.Which);
            }
            else if (selectedItems.Contains(e.Which))
            {
                // Else, if the item is already in the array, remove it
                selectedItems.Remove(e.Which);
            }

        }
        async void OnLogoutClicked()
        {
            CookieManager.Instance.RemoveAllCookie();
            await IoC.Dbconnect.getClient().LogoutAsync();
        }
    }
}