using FlyoutNavigation;
using Foundation;
using MonoTouch.Dialog;
using System;
using System.CodeDom.Compiler;
using System.Linq;
using UIKit;
using System.Collections.Generic;

namespace ITW_MobileApp.iOS
{
    partial class ParentController : UIViewController
    {

        public ParentController(IntPtr handle) : base(handle)
        {
        }

        private static FlyoutNavigationController navigation;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Create the flyout view controller, make it large,
            // and add it as a subview:
            navigation = new FlyoutNavigationController();
            navigation.Position = FlyOutNavigationPosition.Left;
            navigation.View.Frame = UIScreen.MainScreen.Bounds;
            View.AddSubview(navigation.View);
            this.AddChildViewController(navigation);

            // Create the menu:
            navigation.NavigationRoot = new RootElement("Menu") {
                new Section ("Menu") {
                   new StringElement("Recent Events"),
                   new StringElement("Create Event"),
                   new StringElement("Delete Event"),
                   new StringElement("Calendar"),
                   new StringElement("Overtime Schedule"),
                   new StringElement("Setting"),
                   new StringElement("Logout", delegate {DismissModalViewController(true); }),
                }
            };
            // Create an array of UINavigationControllers that correspond to your
            // menu items:

            navigation.ViewControllers = new[] {
               new UINavigationController((RecentEventsController)this.Storyboard.InstantiateViewController("RecentEventsController")),
               new UINavigationController(new CreateEventController()),
               new UINavigationController((EventDeleteController)this.Storyboard.InstantiateViewController("EventDeleteController")),
               new UINavigationController((RecentEventsController)this.Storyboard.InstantiateViewController("RecentEventsController")),
               new UINavigationController((RecentEventsController)this.Storyboard.InstantiateViewController("RecentEventsController")),
               new UINavigationController((RecentEventsController)this.Storyboard.InstantiateViewController("RecentEventsController")),
            };

        }

        public static FlyoutNavigationController getNavigationMenu()
        {
            return navigation;
        }

    }
}
