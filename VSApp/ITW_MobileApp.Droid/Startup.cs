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
        public DatabaseConnection dbconnect;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            if (dbconnect == null)
            {
                dbconnect = new DatabaseConnection();
                await dbconnect.InitLocalDBSyncTables();
            }           

            StartActivity(new Intent(this, typeof(LoginActivity)));

        }
    }
}