using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;


namespace ITW_MobileApp.Droid
{
    [Activity(Label = "Startup", Theme = "@android:style/Theme.Black.NoTitleBar", MainLauncher = true)]
    public class Startup : Activity
    {

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Splash);

            if (IoC.Dbconnect == null)
            {
                IoC.Dbconnect = new DatabaseConnection();
                await IoC.Dbconnect.InitLocalDBSyncTables();
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

            //IoC.EmployeeFactory.createEmployee("Curtis Keller", "testCurtis@gmail.com", 1, "IT", "User");
            //IoC.EmployeeFactory.createEmployee("Corey Keller", "testCorey@gmail.com", 2, "HR", "User");
            //IoC.EmployeeFactory.createEmployee("Alan Keller", "testAlan@gmail.com", 3, "IT", "User");


            StartActivity(new Intent(this, typeof(LoginActivity)));

        }
    }
}