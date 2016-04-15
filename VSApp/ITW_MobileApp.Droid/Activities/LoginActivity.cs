using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System;
using Android.Support.V7.App;
using System.Threading;
using Android.Views;
using Android.Views.InputMethods;

namespace ITW_MobileApp.Droid
{
    [Activity(Theme = "@style/MyTheme.Login")]
    public class LoginActivity : AppCompatActivity
    {
        //Adapter to map the items list to the view
        private EmployeeItemAdapter employeeItemAdapter;
        private EventItemAdapter eventItemAdapter;
        private RecipientListItemAdapter recipientListItemAdapter;
        private MobileServiceUser user;
        private Button loginButton;
        private EditText EditTextEmployeeID;
        private EditText EditTextPassword;
        ErrorHandler error;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            // Create an adapter to bind the items with the view
            employeeItemAdapter = new EmployeeItemAdapter(this, Resource.Layout.Row_List_To_Do);
            eventItemAdapter = new EventItemAdapter(this, Resource.Layout.Row_List_To_Do);
            recipientListItemAdapter = new RecipientListItemAdapter(this, Resource.Layout.Row_List_To_Do);
            error = new ErrorHandler(this);

            loginButton = FindViewById<Button>(Resource.Id.loginBtn);
            EditTextEmployeeID = FindViewById<EditText>(Resource.Id.userName);
            EditTextPassword = FindViewById<EditText>(Resource.Id.password);


            //Login Button sends us to the Main View. THIS WILL NEED TO BE CHANGED FOR AUTHENTICATION.
            
            loginButton.Click += (sender, e) =>
            {
                Login();
                //if (EditTextEmployeeID.Text != "")
                //{
                //    IoC.UserInfo.EmployeeID = int.Parse(EditTextEmployeeID.Text);
                //    var intent = new Intent(this, typeof(RecentEventsActivity));
                //    StartActivity(intent);
                //}
                //else
                //{
                //    error.CreateAndShowDialog("EmployeeID required.","Authentication Error");
                //}
                //LoginUser();
            };

        }

        public void Login()
        {
            if (!Validate())
            {
                return;
            }            

            loginButton.Enabled = false;

            string EmployeeID = EditTextEmployeeID.Text;
            string password = EditTextPassword.Text;

            //implement authentication
            var progressDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
            new Thread(new ThreadStart(delegate
            {
                //LOAD METHOD TO GET ACCOUNT INFO
                if (AuthenticateUser(EmployeeID,password))
                {
                    IoC.UserInfo.EmployeeID = int.Parse(EmployeeID);//this is acting as "Curtis Keller" logged in.
                    var intent = new Intent(this, typeof(RecentEventsActivity));
                    StartActivity(intent);
                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, "Login Unsuccessful.", ToastLength.Long).Show());
                }
                RunOnUiThread(() => progressDialog.Hide());
            })).Start();

            //ProgressDialog progressDialog = new ProgressDialog(this, Resource.Style.MyTheme_Login);
            //progressDialog.Indeterminate = true;
            //progressDialog.SetMessage("Authenticating...");
            //progressDialog.Show();
            //new Android.OS.Handler().PostDelayed( new Java.Lang.Runnable(() =>
            //    {
            //        IoC.UserInfo.EmployeeID = 1;//this is acting as "Curtis Keller" logged in.
            //        var intent = new Intent(this, typeof(RecentEventsActivity));
            //        StartActivity(intent);
            //        progressDialog.Dismiss();
            //    })
            //   , 3000);
            loginButton.Enabled = true;
        }

        private bool AuthenticateUser(string employeeID, string password)
        {
            return true;
        }

        public bool Validate()
        {
            bool valid = true;

            string EmployeeID = EditTextEmployeeID.Text;
            string password = EditTextPassword.Text;
            int checkID;

            if (EmployeeID.Length == 0 || !(int.TryParse(EmployeeID, out checkID)))
            {
                EditTextEmployeeID.Error = "Enter a valid EmployeeID";
                valid = false;
            }
            else {
                EditTextEmployeeID.Error = null;
            }

            if (password.Length == 0 || password.Length < 4 || password.Length > 10)
            {
                EditTextPassword.Error = "between 4 and 10 alphanumeric characters";
                valid = false;
            }
            else {
                EditTextPassword.Error = null;
            }

            return valid;
        }

        //private async Task<bool> Authenticate()
        //{
        //    var success = false;
        //    try
        //    {
        //        // Sign in with Facebook login using a server-managed flow.
        //        user = await IoC.Dbconnect.getClient().LoginAsync(this,
        //            MobileServiceAuthenticationProvider.Google);

        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        error.CreateAndShowDialog(ex, "Authentication failed");
        //    }
        //    return success;
        //}
        //public override void OnBackPressed()
        //{
        //      MoveTaskToBack(true);
        //}
        //[Java.Interop.Export()]
        //public async void LoginUser()
        //{
        //    // Load data only after authentication succeeds.
        //    if (await Authenticate())
        //    {
        //        IoC.UserInfo.EmployeeID = 1;//this is acting as "Curtis Keller" logged in.
        //        var intent = new Intent(this, typeof(RecentEventsActivity));
        //        StartActivity(intent);
        //    }
        //}
    }
}