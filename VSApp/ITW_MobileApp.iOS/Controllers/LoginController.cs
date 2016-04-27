using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace ITW_MobileApp.iOS
{
	partial class LoginController : UIViewController
	{

        private EmployeeItemAdapter employeeItemAdapter;
        private EventItemAdapter eventItemAdapter;
        private RecipientListItemAdapter recipientListItemAdapter;
        
        
        private UITextField EditTextEmployeeID;
        private UITextField EditTextPassword;
        private UIButton loginButton;
        private UIButton createUserButton;


        public LoginController (IntPtr handle) : base (handle)
		{

		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            employeeItemAdapter = new EmployeeItemAdapter();
            eventItemAdapter = new EventItemAdapter();
            recipientListItemAdapter = new RecipientListItemAdapter();

            loginButton = LoginButton;
            EditTextEmployeeID = Username;
            EditTextPassword = Password;


            loginButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                Login();
            };

            //add create user button
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
        public async void Login()
        {
            if (!Validate())
            {
                return;
            }

            loginButton.Enabled = false;

            string EmployeeID = EditTextEmployeeID.Text;
            string password = EditTextPassword.Text;

            var bounds = UIScreen.MainScreen.Bounds;
            LoadingOverlay loadingOverlay = new LoadingOverlay(bounds, "Checking account info...");
            View.Add(loadingOverlay);

            bool authenticated = await AuthenticateUser(EmployeeID, password);
            if (authenticated)
            {
                IoC.UserInfo.EmployeeID = int.Parse(EmployeeID);
                loadingOverlay.Hide();
                ParentController parent = this.Storyboard.InstantiateViewController("ParentController") as ParentController;
                if (parent != null)
                {
                    PresentViewController(parent, true, null);
                }
            }
            else
            {
                loadingOverlay.Hide();
                UIAlertView _error = new UIAlertView("Login Unsuccessful", "User credentials invalid or not a User", null, "Ok", null);
                _error.Show();
            }
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
                UIAlertView _error = new UIAlertView("Invalid Credentials", "Enter a valid EmployeeID and Password", null, "Ok", null);
                _error.Show();
                valid = false;
            }
            else if (password.Length == 0 || password.Length < 4 || password.Length > 10)
            {
                UIAlertView _error = new UIAlertView("Invalid Credentials", "Enter a valid EmployeeID and Password", null, "Ok", null);
                _error.Show();
                valid = false;
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
                UIAlertView _error = new UIAlertView("Success", "User Creation Successful.", null, "Ok", null);
                _error.Show();
            }
            else if (isEmployee == false)
            {
                UIAlertView _error = new UIAlertView("Incorrect ID", "This EmployeeID is not setup as an Employee.", null, "Ok", null);
                _error.Show();
            }
            else
            {
                UIAlertView _error = new UIAlertView("User Already Exists", "This EmployeeID is already a user.", null, "Ok", null);
                _error.Show();
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
    }
}

