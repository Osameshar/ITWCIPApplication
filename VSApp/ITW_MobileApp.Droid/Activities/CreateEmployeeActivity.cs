using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using System.Threading;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V4.View;

namespace ITW_MobileApp.Droid
{
    [Activity(Label = "CreateEmployeeActivity")]
    public class CreateEmployeeActivity : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar _supporttoolbar;
        DrawerLayout _drawer;
        NavigationView _navigationview;

        Button BtnCreateEmployee;
        EditText EditTextFirstName;
        EditText EditTextLastName;
        EditText EditTextEmployeeID;
        EditText EditTextEmail;
        Spinner SpinnerDepartment;
        Spinner SpinnerPrivledge;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreateEmployee);

            _supporttoolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.ToolBar);
            _drawer = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            _navigationview = FindViewById<NavigationView>(Resource.Id.nav_view);
            ToolbarCreator toolbarCreator = new ToolbarCreator();
            toolbarCreator.setupToolbar(_supporttoolbar, _drawer, _navigationview, Resource.String.createemployee, this);

            BtnCreateEmployee = FindViewById<Button>(Resource.Id.ButtonCreateEmployee);        
            EditTextFirstName = FindViewById<EditText>(Resource.Id.EditTextFirstName);
            EditTextLastName = FindViewById<EditText>(Resource.Id.EditTextLastName);
            EditTextEmployeeID = FindViewById<EditText>(Resource.Id.EditTextEmployeeID);
            EditTextEmail = FindViewById<EditText>(Resource.Id.EditTextEmail);
            SpinnerDepartment = FindViewById<Spinner>(Resource.Id.SpinnerDepartment);
            SpinnerPrivledge = FindViewById<Spinner>(Resource.Id.SpinnerPrivledgeLevel);
            Color color = new Color(ContextCompat.GetColor(this, Resource.Color.black)); 
            SpinnerDepartment.Background.SetColorFilter(color,PorterDuff.Mode.SrcAtop);
            SpinnerPrivledge.Background.SetColorFilter(color, PorterDuff.Mode.SrcAtop);

            BtnCreateEmployee.Click += async delegate
            {
                BtnCreateEmployee.Enabled = false;
                await createEmployee();
                BtnCreateEmployee.Enabled = true;
            };
        }

        private async Task createEmployee()
        {
            ErrorHandler error = new ErrorHandler(this);
            if (!Validate())
            {
                return;
            }

            try
            {
                int EmployeeID = int.Parse(EditTextEmployeeID.Text);
                string fullName = EditTextFirstName + " " + EditTextLastName;
                string Email = EditTextEmail.Text;
                string Department = SpinnerDepartment.SelectedItem.ToString();
                string Privledege = SpinnerPrivledge.SelectedItem.ToString();
                bool validID = await validateEmployeeID(EmployeeID);
                if (validID)
                {
                    var progressDialog = ProgressDialog.Show(this, "Please wait...", "Creating Employee...", true);
                    new Thread(new ThreadStart(async delegate
                    {
                        await IoC.EmployeeFactory.createEmployee(fullName, Email, EmployeeID, Department, Privledege);
                        var intent = new Intent(this, typeof(RecentEventsActivity));
                        StartActivity(intent);
                        RunOnUiThread(() => progressDialog.Hide());
                    })).Start();
                }
                else
                {
                    error.CreateAndShowDialog("EmployeeID is already used.", "Invalid EmployeeID");
                }
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

        private async Task<bool> validateEmployeeID(int empID)
        {
            var employees = await IoC.Dbconnect.getClient().GetTable<EmployeeItem>().ToListAsync();

            foreach (EmployeeItem item in employees)
            {
                if (item.EmployeeID == empID)
                {
                    return false;
                }
            }
            return true;
        }

        private bool Validate()
        {
            bool valid = true;

            string FirstName = EditTextFirstName.Text;
            string LastName = EditTextLastName.Text;
            string strEmployeeID = EditTextEmployeeID.Text;
            string Email = EditTextEmail.Text;
            string Department = SpinnerDepartment.SelectedItem.ToString();
            string Privledege = SpinnerPrivledge.SelectedItem.ToString();

            int checkID;

            if (strEmployeeID.Length == 0 || !(int.TryParse(strEmployeeID, out checkID)))
            {
                EditTextEmployeeID.Error = "Enter a valid EmployeeID";
                valid = false;
            }
            else {
                EditTextEmployeeID.Error = null;
            }
            if (FirstName.Length == 0)
            {
                EditTextFirstName.Error = "First Name Required";
                valid = false;
            }
            else {
                EditTextFirstName.Error = null;
            }
            if (LastName.Length == 0)
            {
                EditTextLastName.Error = "Last Name Required";
                valid = false;
            }
            else {
                EditTextLastName.Error = null;
            }

            return valid;
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