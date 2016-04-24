using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;
using System.Threading.Tasks;
using Android.Support.V7.App;
using System.Threading;

namespace ITW_MobileApp.Droid
{
    [Activity(Theme = "@style/MyTheme.Login")]
    public class LoginActivity : AppCompatActivity
    {
        //Adapter to map the items list to the view
        private EmployeeItemAdapter employeeItemAdapter;
        private EventItemAdapter eventItemAdapter;
        private RecipientListItemAdapter recipientListItemAdapter;
        private Button loginButton;
        private Button createUserButton;
        private EditText EditTextEmployeeID;
        private EditText EditTextPassword;
        ErrorHandler error;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            error = new ErrorHandler(this);

            loginButton = FindViewById<Button>(Resource.Id.loginBtn);
            EditTextEmployeeID = FindViewById<EditText>(Resource.Id.userName);
            EditTextPassword = FindViewById<EditText>(Resource.Id.password);
            createUserButton = FindViewById<Button>(Resource.Id.createUserBtn);

            createUserButton.Click += (sender, e) =>
            {
                ValidateCreateUser();
            };
            loginButton.Click += (sender, e) =>
            {
                Login();
            };

        }
        public async void ValidateCreateUser()
        {
            if (!Validate())
            {
                return;
            }

            createUserButton.Enabled = false;

            string EmployeeID = EditTextEmployeeID.Text;
            string password = EditTextPassword.Text;

            await createUser(EmployeeID, password);

            createUserButton.Enabled = true;
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


            var progressDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
            new Thread(new ThreadStart(async delegate
            {
                bool authenticated = await AuthenticateUser(EmployeeID, password);
                if (authenticated)
                {
                    IoC.UserInfo.EmployeeID = int.Parse(EmployeeID);
                    var intent = new Intent(this, typeof(RecentEventsActivity));
                    StartActivity(intent);
                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, "Login Unsuccessful.", ToastLength.Long).Show());
                }
                RunOnUiThread(() => progressDialog.Hide());
            })).Start();

            loginButton.Enabled = true;
        }
        private async Task<bool> AuthenticateUser(string employeeID, string password)
        {
            return await LoginAuthenticator.Authenticate(employeeID, password);
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
        public async Task createUser(string employeeID, string password)
        {
            int empID = int.Parse(employeeID);
            bool isEmployee = await searchForEmployee(empID);
            bool isUser = await searchForUser(empID);
            if (isEmployee && !isUser)
            {
                await LoginAuthenticator.GenerateSaltedSHA1(password, empID);
                RunOnUiThread(() => Toast.MakeText(this, "User Creation Successful.", ToastLength.Long).Show());
            }
            else if (isEmployee == false)
            {
                error.CreateAndShowDialog("This EmployeeID is not setup as an Employee.", "Incorrect ID");
            }
            else
            {
                error.CreateAndShowDialog("This EmployeeID is already a user.", "User Already Exists");
            }
        }
        public async Task<bool> searchForEmployee(int employeeID)
        {
            var employees = await IoC.Dbconnect.getEmployeeSyncTable().ToListAsync();
            foreach (EmployeeItem employee in employees)
            {
                if (employee.EmployeeID == employeeID)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> searchForUser(int employeeID)
        {
            var users = await IoC.Dbconnect.getClient().GetTable<EmployeeLoginItem>().ToListAsync();
            foreach (EmployeeLoginItem user in users)
            {
                if (user.EmployeeID == employeeID)
                {
                    return true;
                }
            }
            return false;
        }
        public override void OnBackPressed()
        {
             MoveTaskToBack(true);
        }
    }
}