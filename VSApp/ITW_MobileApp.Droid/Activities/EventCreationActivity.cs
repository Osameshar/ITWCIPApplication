using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System;
using Android.Content;
using Android.Support.V4.View;

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
        EditText EditTextEventName;
        EditText EditTextEventRecipients;
        EditText EditTextLocation;
        Spinner SpinnerCategoryType;
        Spinner SpinnerPriorty;
        EditText EditTextEventDescription;

        Button CreateEventBtn;

        View dialogView;
        Android.Support.V7.App.AlertDialog alertDialog;

        int year = -1;
        int month = -1;
        int day = -1;
        int hour = -1;
        int minute = -1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventCreation);

            setupToolbar();

            DatePickerBtn = FindViewById<Button>(Resource.Id.ButtonPickDate);
            TimePickerBtn = FindViewById<Button>(Resource.Id.ButtonPickTime);
            EditTextEventName = FindViewById<EditText>(Resource.Id.EditTextEventName);
            EditTextEventRecipients = FindViewById<EditText>(Resource.Id.EditTextEventRecipients);
            EditTextLocation = FindViewById<EditText>(Resource.Id.EditTextLocation);
            SpinnerCategoryType = FindViewById<Spinner>(Resource.Id.SpinnerCategoryType);
            SpinnerPriorty = FindViewById<Spinner>(Resource.Id.SpinnerPriority);
            EditTextEventDescription = FindViewById<EditText>(Resource.Id.EditTextEventDescription);

            CreateEventBtn = FindViewById<Button>(Resource.Id.ButtonCreateEvent);

            DatePickerBtn.Click += delegate
            {
                dialogDateOpen();
            };
            TimePickerBtn.Click += delegate
            {
                dialogTimeOpen();
            };

            CreateEventBtn.Click += delegate
            {
                createEvent();
            };
            

        }

        private void createEvent()
        {
            ErrorHandler error = new ErrorHandler(this);
            string EventName;
            string Recipients;
            string Location;
            string Category;
            string Priority;
            string EventDescription;
            DateTime EventDateTime;
            string time;


            EventName = EditTextEventName.Text.ToString();
            Recipients = EditTextEventRecipients.Text.ToString();
            Location = EditTextLocation.Text.ToString();
            Category = SpinnerCategoryType.SelectedItem.ToString();
            Priority = SpinnerPriorty.SelectedItem.ToString();
            EventDescription = EditTextEventDescription.Text.ToString();

            if (string.IsNullOrEmpty(EventName))
            {
                 error.CreateAndShowDialog("Event name is required", "Error");
                 return;
            }
            else if (year == -1 || month == -1 || day == -1 || hour == -1 || minute == -1)
            {
                error.CreateAndShowDialog("Event Time and Date is required", "Error");
                return;
            }
            else
            {
                EventDateTime = new DateTime(year, month, day, hour, minute, 0);
                time = hour.ToString() + ":" + minute.ToString();
                IoC.EventFactory.createEvent(EventName, Recipients, EventDateTime, time, Location, Category, Priority, EventDescription);
            }

            var intent = new Intent(this, typeof(RecentEventsActivity));
            StartActivity(intent);
        }

        private void setupToolbar()
        {
            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _supporttoolbar.SetTitle(Resource.String.create_event);
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
                    case Resource.Id.nav_createEvent:
                        {
                            //switch to calendar view
                            var intent = new Intent(this, typeof(EventCreationActivity));
                            StartActivity(intent);
                        }
                        break;
                    case Resource.Id.nav_calendar:
                        {
                            Console.WriteLine("calendar");
                            //switch to calendar view
                            //var intent = new Intent(this, typeof(EventCreationActivity));
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

            int currentApiVersion = (int) Build.VERSION.SdkInt;
            if (currentApiVersion > (int)Build.VERSION_CODES.LollipopMr1)
            {
                hour = timePicker.Hour;
                minute = timePicker.Minute;
            }
            else {
                hour = (int)timePicker.CurrentHour;
                minute = (int)timePicker.CurrentMinute;
            }

            alertDialog.Dismiss();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _drawer.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            if (_drawer.IsDrawerOpen(GravityCompat.Start))
            {
                _drawer.CloseDrawer(GravityCompat.Start);
            }
            else {
                var intent = new Intent(this, typeof(RecentEventsActivity));
                StartActivity(intent);
            }
        }
    }
}