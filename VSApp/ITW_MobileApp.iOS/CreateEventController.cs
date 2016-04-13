using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ITW_MobileApp.iOS
{
	partial class CreateEventController : UIViewController
	{
		public CreateEventController (IntPtr handle) : base (handle)
		{
            UIImage hamburgericon = UIImage.FromFile("Menu Filled-30");
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(hamburgericon, UIBarButtonItemStyle.Plain, delegate
            {
                ParentController.getNavigationMenu().ToggleMenu();
            });
        }
	}
}
