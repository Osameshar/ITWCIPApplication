using Microsoft.WindowsAzure.MobileServices.Sync;
using MonoTouch.Dialog;
using System;
using System.Drawing;
using System.Threading.Tasks;
using UIKit;

namespace ITW_MobileApp.iOS
{
    class CreateEmployeeController : DialogViewController
    {
        EntryElement EditTextFirstName;
        EntryElement EditTextLastName;
        EntryElement EditTextEmployeeID;
        EntryElement EditTextEmail;
        RadioGroup SpinnerDepartment;
        RadioGroup SpinnerPrivledge;
        string[] departments = new string[] { "Meeting", "Company Event", "Machine Maintenance", "Emergency" };
        string[] privledges = new string[] { "User", "Moderators", "Admin" };

        public CreateEmployeeController() : base(new RootElement("Employee Creation"), true)
        {

            UIImage hamburgericon = UIImage.FromFile("Menu Filled-20");
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(hamburgericon, UIBarButtonItemStyle.Plain, delegate
            {
                ParentController.getNavigationMenu().ToggleMenu();
            });

        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
            var info = new RootElement("Info") {
                new Section() {
                    { EditTextFirstName = new EntryElement("First Name", "", null) },
                    { EditTextLastName = new EntryElement("Last Name", "", null) },
                    { EditTextEmployeeID = new EntryElement("EmployeeID", "", null) },
                    { EditTextEmail = new EntryElement("Email", "", null) }
                },

                new Section()
                {
                    new RootElement("Category", SpinnerDepartment = new RadioGroup("Department", 0)) {
                        new Section() {
                            new RadioElement ("Meeting", "Type of Event"),
                            new RadioElement ("Company Event", "Type of Event"),
                            new RadioElement ("Machine Maintenance", "Type of Event"),
                            new RadioElement ("Emergency", "Type of Event")
                        }
                    },

                    new RootElement("Privledge", SpinnerPrivledge = new RadioGroup ("Privledge", 0))
                    {
                        new Section()
                        {
                            new RadioElement("User", "Privledge"),
                            new RadioElement("Moderator", "Privledge"),
                            new RadioElement("Admin", "Privledge")
                        }
                    }                    
                },
            };

            Root.Add(info);

            UIButton createEmployeeBtn = UIButton.FromType(UIButtonType.RoundedRect);
            createEmployeeBtn.SetTitle("Add Employee", UIControlState.Normal);

            createEmployeeBtn.Frame = new Rectangle(0,0, 320, 44);
            int y = (int)((View.Frame.Size.Height - createEmployeeBtn.Frame.Size.Height) / 1.25 );
            int x = ((int)(View.Frame.Size.Width - createEmployeeBtn.Frame.Size.Width)) / 2;
            createEmployeeBtn.Frame = new Rectangle(x,y, (int)createEmployeeBtn.Frame.Width, (int)createEmployeeBtn.Frame.Height);
            View.AddSubview(createEmployeeBtn);


            createEmployeeBtn.TouchUpInside += async (object sender, EventArgs e) =>
            {
                await createEmployee(info);
            };
        }

        private async Task createEmployee(RootElement info)
        {
            if (!Validate())
            {
                return;
            }

            try
            {

                int EmployeeID = int.Parse(EditTextEmployeeID.Value);
                string fullName = EditTextFirstName.Value + " " + EditTextLastName.Value;
                string Email = EditTextEmail.Value;
                string Department = departments[SpinnerDepartment.Selected];
                string Privledege = privledges[SpinnerPrivledge.Selected];
                bool validID = await validateEmployeeID(EmployeeID);
                if (validID)
                {

                    var bounds = UIScreen.MainScreen.Bounds;
                    LoadingOverlay loadingOverlay = new LoadingOverlay(bounds, "Creating Employee...");
                    View.Add(loadingOverlay);

                    await IoC.EmployeeFactory.createEmployee(fullName, Email, EmployeeID, Department, Privledege);

                    loadingOverlay.Hide();

                    UIAlertView _message = new UIAlertView("Success!", "Employee creation successful!", null, "Ok", null);
                    _message.Show();
                }
                else
                {
                    UIAlertView _message = new UIAlertView("Invalid EmployeeID", "EmployeeID is already used.", null, "Ok", null);
                    _message.Show();
                }
            }
            catch (MobileServicePushFailedException ex)
            {
                UIAlertView _message = new UIAlertView("Connection Failed", "Internet connection required for Event creation.", null, "Ok", null);
                _message.Show();
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

            string FirstName = EditTextFirstName.Value;
            string LastName = EditTextLastName.Value;
            string strEmployeeID = EditTextEmployeeID.Value;
            string Email = EditTextEmail.Value;
            string Department = departments[SpinnerDepartment.Selected];
            string Privledege = privledges[SpinnerPrivledge.Selected];

            int checkID;

            if (strEmployeeID.Length == 0 || !(int.TryParse(strEmployeeID, out checkID)))
            {
                UIAlertView _message = new UIAlertView("Invalid EmployeeID", "Enter a valid EmployeeID", null, "Ok", null);
                _message.Show();
                valid = false;
            }
            else if (FirstName.Length == 0)
            {
                UIAlertView _message = new UIAlertView("Name Required", "First Name Required", null, "Ok", null);
                _message.Show();
                valid = false;
            }
            else if (LastName.Length == 0)
            {
                UIAlertView _message = new UIAlertView("Name Required", "Last Name Required", null, "Ok", null);
                _message.Show();
                valid = false;
            }

            return valid;
        }
    }
}
