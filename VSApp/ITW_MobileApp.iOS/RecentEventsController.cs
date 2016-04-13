using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ITW_MobileApp.iOS
{
	partial class RecentEventsController : UITableViewController
	{
		public RecentEventsController (IntPtr handle) : base (handle)
		{
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Action, delegate {
                ParentController.getNavigationMenu().ToggleMenu();
            });

            //NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile("hamburger.png"), UIBarButtonItemStyle.Plain, delegate {
            //    ParentController.getNavigationMenu().ToggleMenu();
            //});
        }
	}
}
