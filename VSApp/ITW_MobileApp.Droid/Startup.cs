using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ITW_MobileApp.Droid
{
    [Activity(Label = "Startup", MainLauncher = true)]
    public class Startup : Activity
    {

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            if (IoC.Dbconnect == null)
            {
                IoC.Dbconnect = new DatabaseConnection();
                await IoC.Dbconnect.InitLocalDBSyncTables();
            }           

            StartActivity(new Intent(this, typeof(LoginActivity)));

        }
    }
}