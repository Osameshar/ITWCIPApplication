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
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using System.Threading;
using Android.Support.V4.Content;
using Android.Graphics;
using System.Collections.Generic;



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
        MultiAutoCompleteTextView EditTextEventRecipients;
        EditText EditTextLocation;
        Spinner SpinnerCategoryType;
        Spinner SpinnerPriorty;
        EditText EditTextEventDescription;
        TextView EventDate;
        TextView EventTime;
    

        Button CreateEventBtn;
        ErrorHandler error;
        View dialogView;
        Android.Support.V7.App.AlertDialog alertDialog;

        DateTime newDate;
        DateTime newTime;

        int year = -1;
        int month = -1;
        int day = -1;
        int hour = -1;
        int minute = -1;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            switch (IoC.UserInfo.Employee.PrivledgeLevel)
            {
                case "Admin":
                    {
                        SetContentView(Resource.Layout.EventCreation_Admin);
                        break;
                    }
                case "Moderator":
                    {
                        SetContentView(Resource.Layout.EventCreation_Moderator);
                        break;
                    }
            }

            EmployeeItemAdapter employeeItemAdapter = new EmployeeItemAdapter();
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(employeeItemAdapter);
            List<string> autoCompleteOptions = employeeItemAdapter.getAutoCompleteList();
            ArrayAdapter autoCompleteAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, autoCompleteOptions);

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, autoCompleteOptions);

            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _drawer = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            _navigationview = FindViewById<NavigationView>(Resource.Id.nav_view);
            ToolbarCreator toolbarCreator = new ToolbarCreator();
            toolbarCreator.setupToolbar(_supporttoolbar, _drawer, _navigationview, Resource.String.create_event, this);

            DatePickerBtn = FindViewById<Button>(Resource.Id.ButtonPickDate);
            TimePickerBtn = FindViewById<Button>(Resource.Id.ButtonPickTime);
            EditTextEventName = FindViewById<EditText>(Resource.Id.EditTextEventName);
            EditTextEventRecipients = FindViewById<MultiAutoCompleteTextView>(Resource.Id.EditTextEventRecipients);
            EditTextEventRecipients.Adapter = adapter;
            EditTextEventRecipients.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());


            EditTextLocation = FindViewById<EditText>(Resource.Id.EditTextLocation);
            SpinnerCategoryType = FindViewById<Spinner>(Resource.Id.SpinnerCategoryType);
            SpinnerPriorty = FindViewById<Spinner>(Resource.Id.SpinnerPriority);
            EditTextEventDescription = FindViewById<EditText>(Resource.Id.EditTextEventDescription);
            EventDate = FindViewById<TextView>(Resource.Id.EventDate);
            EventTime = FindViewById<TextView>(Resource.Id.EventTime);
            CreateEventBtn = FindViewById<Button>(Resource.Id.ButtonCreateEvent);
            Color color = new Color(ContextCompat.GetColor(this, Resource.Color.black));
            SpinnerCategoryType.Background.SetColorFilter(color, PorterDuff.Mode.SrcAtop);
            SpinnerPriorty.Background.SetColorFilter(color, PorterDuff.Mode.SrcAtop);

            EventDate.Text += "No date entered.";
            EventTime.Text += "No time entered";

            DatePickerBtn.Click += delegate
            {
                dialogDateOpen();

            };

            TimePickerBtn.Click += delegate
            {
                dialogTimeOpen();

            };

            CreateEventBtn.Click += async delegate
            {
                CreateEventBtn.Enabled = false;
                await createEvent();
                CreateEventBtn.Enabled = true;
            };
            

        }

        private async Task createEvent()
        {
            error = new ErrorHandler(this);
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
            else if (new DateTime(year, month, day).Date.CompareTo(DateTime.Now.Date) < 0 ||
                     (new DateTime(year, month, day).Date.CompareTo(DateTime.Now.Date) == 0 && hour < DateTime.Now.Hour))
            {
                error.CreateAndShowDialog("Event is set to a past date.", "Error");
                return;
            }            
            else
            {
                EventDateTime = new DateTime(year, month, day, hour, minute, 0);
                time = hour.ToString() + ":" + minute.ToString();
                try
                {
                    var progressDialog = ProgressDialog.Show(this, "Please wait...", "Creating Event...", true);
                    new Thread(new ThreadStart(async delegate
                    {
                        await IoC.EventFactory.createEvent(EventName, Recipients, EventDateTime, time, Location, Category, Priority, EventDescription);
                        var intent = new Intent(this, typeof(RecentEventsActivity));
                        StartActivity(intent);
                        RunOnUiThread(() => progressDialog.Hide());
                    })).Start();

                }
                catch (Java.Net.MalformedURLException)
                {
                    System.Diagnostics.Debug.WriteLine("There was an error creating the Mobile Service. Verify the URL");
                }
                catch (MobileServicePushFailedException ex)
                {
                    error.CreateAndShowDialog("Internet connection required for Event creation.", "Connection Failed");
                }
                catch (Java.Net.UnknownHostException ex)
                {
                    
                    error.CreateAndShowDialog("Internet connection required for Event creation.", "Connection Failed");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

        }

        public void dialogDateOpen()
        {
            dialogView = View.Inflate(this, Resource.Layout.date_picker, null);
            alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this).Create();
            DateSetBtn = dialogView.FindViewById<Button>(Resource.Id.ButtonDateSet);
            DateSetBtn.Click += delegate
            {
                EventDate.Text = "Date: ";
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
                EventTime.Text = "Time: ";
                pickTime();
            };
            alertDialog.SetView(dialogView);
            alertDialog.Show();
        }
        public void pickDate()
        {
            DatePicker datePicker = (DatePicker)dialogView.FindViewById(Resource.Id.date_picker);

            year = datePicker.Year;
            month = datePicker.Month + 1;
            day = datePicker.DayOfMonth;
            assignDate();

            alertDialog.Dismiss();
        }

        public void assignDate()
        {
            newDate = new DateTime(year, month, day);
            EventDate.Text += newDate.ToString("MMMM dd, yyyy");
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
            assignTime();

            alertDialog.Dismiss();
        }

        public void assignTime()
        {
            newTime = new DateTime(2000, 1, 1, hour, minute, 0);
            EventTime.Text += newTime.ToString("h:mm tt");
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