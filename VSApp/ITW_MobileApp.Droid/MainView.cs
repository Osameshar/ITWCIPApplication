using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ITW_MobileApp
{
    [Activity(Label = "CIPConnect", MainLauncher = false, Theme = "@android:style/Theme.Material")]
    public class MainView : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.MainView);

            //enable navigation mode to support tab layout
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            //adding Recent Events tab
            AddTab("Recent Events", Resource.Drawable.Icon, new RecentEventsFragment());

            //adding Calendar Tab
            AddTab("Calendar", Resource.Drawable.Icon, new CalendarFragment());

            //adding Overtime Tab
            AddTab("Overtime", Resource.Drawable.Icon, new OvertimeFragment());

        }

        /*
		 * This method is used to create and add dynamic tab view
		 * @Param,
		 *  tabText: title to be displayed in tab
		 *  iconResourceId: image/resource id
		 *  fragment: fragment reference
		 * 
		*/
        void AddTab(string tabText, int iconResourceId, Fragment fragment)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(tabText);
            tab.SetIcon(iconResourceId);

            // must set event handler for replacing tabs tab
            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e) {
                e.FragmentTransaction.Replace(Resource.Id.fragmentContainer, fragment);
            };

            this.ActionBar.AddTab(tab);
        }

    }
}