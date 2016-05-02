using Foundation;
using UIKit;

namespace ITW_MobileApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        public override UIWindow Window
        {
            get;
            set;
        }

        
        public override bool FinishedLaunching(UIApplication application, NSDictionary options)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            if (IoC.Dbconnect == null)
            {
                IoC.Dbconnect = new DatabaseConnection();
                InitSyncTables();
            }
            if (IoC.EventFactory == null)
            {
                IoC.EventFactory = new EventFactory();
            }
            if (IoC.ViewRefresher == null)
            {
                IoC.ViewRefresher = new ViewRefresher();
            }
            if (IoC.EmployeeFactory == null)
            {
                IoC.EmployeeFactory = new EmployeeFactory();
            }
            if (IoC.RecipientListFactory == null)
            {
                IoC.RecipientListFactory = new RecipientListFactory();
            }
            if (IoC.UserInfo == null)
            {
                IoC.UserInfo = new UserInfo();
            }
            SQLitePCL.CurrentPlatform.Init();

            var settings = UIUserNotificationSettings.GetSettingsForTypes(
              UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

            if (options != null)
            {
                // check for a local notification
                if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                {
                    var localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
                    if (localNotification != null)
                    {
                        UIAlertController okayAlertController = UIAlertController.Create(localNotification.AlertAction, localNotification.AlertBody, UIAlertControllerStyle.Alert);
                        okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                        ParentController.getNavigationMenu().ViewControllers[0].PresentViewController(okayAlertController, true, null);

                        // reset our badge
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
                    }
                }
            }




            return true;
        }
        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            // show an alert
            UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
            okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            ParentController.getNavigationMenu().ViewControllers[0].PresentViewController(okayAlertController, true, null);

            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

        }
        private async void InitSyncTables()
        {
            await IoC.Dbconnect.InitLocalDBSyncTables();
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}