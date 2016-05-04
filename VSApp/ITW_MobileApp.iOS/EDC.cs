using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;

namespace ITW_MobileApp.iOS
{

    partial class EDC : UIViewController
	{
        private List<EventItem> eventList;
        public EDC (IntPtr handle) : base (handle)
		{
            UIImage hamburgericon = UIImage.FromFile("Menu Filled-20");
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(hamburgericon, UIBarButtonItemStyle.Plain, delegate
            {
                ParentController.getNavigationMenu().ToggleMenu();
            });

            eventList = ParentController.getEventList();

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

        }

    }
}
