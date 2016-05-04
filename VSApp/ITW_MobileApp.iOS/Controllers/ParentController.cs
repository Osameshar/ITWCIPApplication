using FlyoutNavigation;
using Foundation;
using MonoTouch.Dialog;
using System;
using UIKit;

namespace ITW_MobileApp.iOS
{
    partial class ParentController : UIViewController
    {
        public string overtimeURL = "https://drive.google.com/open?id=0B2kq5WLtKIJHRUtsT0x3cWlPQ1k";
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
            if (IoC.UserInfo.Employee.PrivledgeLevel == "Admin")
            {
                navigation.NavigationRoot = new RootElement("Menu") {
                    new Section ("Menu") {
                       new StringElement("Recent Events"),
                       new StringElement("Create Event"),
                       new StringElement("Delete Event"),
                       new StringElement("Create Employee"),
                       new StringElement("Calendar"),
                       new StringElement("Filter Events"),
                       new StringElement("Overtime Schedule", delegate { viewOvertimeSchedule(); }),
                       new StringElement("Logout", delegate { logout(); }),
                    }
                };
                // Create an array of UINavigationControllers that correspond to your
                // menu items:

                navigation.ViewControllers = new[] {
                   new UINavigationController((RecentEventsController)this.Storyboard.InstantiateViewController("RecentEventsController")),
                   new UINavigationController(new CreateEventController()),
                   new UINavigationController((EventDeleteController)this.Storyboard.InstantiateViewController("EventDeleteController")),
                   new UINavigationController(new CreateEmployeeController()),
                   new UINavigationController(new CalendarController()),
                   new UINavigationController(new FilterEventsController()),
                };
            }

            else if (IoC.UserInfo.Employee.PrivledgeLevel == "Moderator")
            {
                navigation.NavigationRoot = new RootElement("Menu") {
                    new Section ("Menu") {
                       new StringElement("Recent Events"),
                       new StringElement("Create Event"),
                       new StringElement("Delete Event"),
                       new StringElement("Calendar"),
                       new StringElement("Filter Events"),
                       new StringElement("Overtime Schedule", delegate { viewOvertimeSchedule(); }),
                       new StringElement("Logout", delegate { logout(); }),
                    }
                };
                // Create an array of UINavigationControllers that correspond to your
                // menu items:

                navigation.ViewControllers = new[] {
                   new UINavigationController((RecentEventsController)this.Storyboard.InstantiateViewController("RecentEventsController")),
                   new UINavigationController(new CreateEventController()),
                   new UINavigationController((EventDeleteController)this.Storyboard.InstantiateViewController("EventDeleteController")),
                   new UINavigationController(new CalendarController()),
                   new UINavigationController(new FilterEventsController()),
                };
            }

            else
            {
                navigation.NavigationRoot = new RootElement("Menu") {
                    new Section ("Menu") {
                       new StringElement("Recent Events"),
                       new StringElement("Calendar"),
                       new StringElement("Filter Events"),
                       new StringElement("Overtime Schedule", delegate { viewOvertimeSchedule(); }),
                       new StringElement("Logout", delegate { logout(); }),
                    }
                };
                // Create an array of UINavigationControllers that correspond to your
                // menu items:

                navigation.ViewControllers = new[] {
                   new UINavigationController((RecentEventsController)this.Storyboard.InstantiateViewController("RecentEventsController")),
                   new UINavigationController(new CalendarController()),
                   new UINavigationController(new FilterEventsController()),
                };
            }

        }
        public void viewOvertimeSchedule()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(overtimeURL));
        }
        public void logout()
        {
            IoC.UserInfo.Employee = null;
            IoC.UserInfo.EmployeeID = -1;
            DismissModalViewController(true);
        }
        public static FlyoutNavigationController getNavigationMenu()
        {
            return navigation;
        }
    }
}
