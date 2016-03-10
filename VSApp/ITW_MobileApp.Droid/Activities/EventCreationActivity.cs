using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Java.Util;

namespace ITW_MobileApp.Droid
{
    [Activity(Theme = "@style/MyTheme")]
    public class EventCreationActivity : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar _supporttoolbar;
        DrawerLayout _drawer;
        NavigationView _navigationview;

        Button DatePickerBtn;
        Button DateSetBtn;
        Button TimePickerBtn;
        Button TimeSetBtn;

        View dialogView;
        Android.Support.V7.App.AlertDialog alertDialog;

        DateTime EventDateTime;
        int year;
        int month;
        int day;
        int hour;
        int min;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventCreation);

            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _supporttoolbar.SetTitle(Resource.String.recent_events);
            SetSupportActionBar(_supporttoolbar);
            _supporttoolbar.SetNavigationIcon(Resource.Drawable.ic_menu_white_24dp);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);

            _drawer = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);

            _navigationview = FindViewById<NavigationView>(Resource.Id.nav_view);

            _navigationview.NavigationItemSelected += (sender, e) =>

            {
                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_recentEvents:
                        {
                            var intent = new Intent(this, typeof(RecentEventsActivity));
                            StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_calendar:
                        {
                            Console.WriteLine("calendar");
                            //switch to calendar view
                            //var intent = new Intent(this, typeof(RecentEventsActivity));
                            //StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_overtime:
                        {
                            Console.WriteLine("overtime");
                            //switch to overtime view
                            //var intent = new Intent(this, typeof(RecentEventsActivity));
                            //StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_settings:
                        {
                            Console.WriteLine("settings");
                            //switch to settings view
                            //var intent = new Intent(this, typeof(RecentEventsActivity));
                            //StartActivity(intent);
                        }
                        break;
                    case Resource.Id.logoutitem:
                        {
                            Console.WriteLine("logout");
                            //logout
                            //var intent = new Intent(this, typeof(RecentEventsActivity));
                            //StartActivity(intent);
                        }
                        break;

                }
            };
            DatePickerBtn = FindViewById<Button>(Resource.Id.ButtonPickDate);
            TimePickerBtn = FindViewById<Button>(Resource.Id.ButtonPickTime);
            DatePickerBtn.Click += delegate
            {
                dialogDateOpen();
            };
            TimePickerBtn.Click += delegate
            {
                dialogTimeOpen();
            };



        }
        public void dialogDateOpen()
        {
            dialogView = View.Inflate(this, Resource.Layout.date_picker, null);
            alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this).Create();
            DateSetBtn = dialogView.FindViewById<Button>(Resource.Id.ButtonDateSet);
            DateSetBtn.Click += delegate
            {
                pickDate();
            };
            alertDialog.SetView(dialogView);
            alertDialog.Show();
        }
        public void dialogTimeOpen()
        {
            dialogView = View.Inflate(this, Resource.Layout.time_picker, null);
            alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this).Create();
            TimeSetBtn = dialogView.FindViewById<Button>(Resource.Id.ButtonTimeSet);
            TimeSetBtn.Click += delegate
            {
                pickTime();
            };
            alertDialog.SetView(dialogView);
            alertDialog.Show();
        }
        public void pickDate()
        {
            DatePicker datePicker = (DatePicker)dialogView.FindViewById(Resource.Id.date_picker);

            year = datePicker.Year;
            month = datePicker.Month;
            day = datePicker.DayOfMonth;

            alertDialog.Dismiss();
        }
        public void pickTime()
        {
            TimePicker timePicker = (TimePicker)dialogView.FindViewById(Resource.Id.time_picker);
            timePicker.ClearFocus();


            int currentApiVersion = (int) Build.VERSION.SdkInt;
            if (currentApiVersion > (int)Build.VERSION_CODES.LollipopMr1)
            {
                hour = timePicker.Hour;
                min = timePicker.Minute;
            }
            else {
                hour = (int)timePicker.CurrentHour;
                min = (int)timePicker.CurrentMinute;
            }

            EventDateTime.AddHours(hour);
            EventDateTime.AddMinutes(min);

            alertDialog.Dismiss();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _drawer.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}