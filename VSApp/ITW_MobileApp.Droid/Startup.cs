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
            StartActivity(new Intent(this, typeof(LoginActivity)));

        }
    }
}