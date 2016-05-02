using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using MonoTouch.Dialog;
using CoreGraphics;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace ITW_MobileApp.iOS
{
    public class CreateEventController : DialogViewController
    {
        EntryElement eventname;
        EntryElement recipients;
        EntryElement location;
        DateElement date;
        TimeElement timeelement;
        string[] categories = new string[] { "Meeting", "Company Event", "Machine Maintenance", "Emergency" };
        string[] priorities = new string[] { "Low", "Medium", "High" };
        RadioGroup category;
        RadioGroup priority;
        EntryElement description;

        public CreateEventController() : base(new RootElement("Event Creation"), true)
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
                    { eventname = new EntryElement("Event Name", "Enter the Name", null) },
                    { recipients = new EntryElement("Recipients' Names", "Enter the Recipients", null) },
                    { location = new EntryElement("Enter Location", "Enter the Location", null) }
                },

                new Section()
                {
                    { date = new DateElement("Pick the Date", DateTime.Now) },
                    { timeelement = new TimeElement("Pick the Time", DateTime.Now) },
                    { description = new EntryElement("Description", "Enter a Description", null) }
                },

                new Section()
                {
                    new RootElement("Type", category = new RadioGroup("Type of Event", 0)) {
                        new Section() {
                            new RadioElement ("Meeting", "Type of Event"),
                            new RadioElement ("Company Event", "Type of Event"),
                            new RadioElement ("Machine Maintenance", "Type of Event"),
                            new RadioElement ("Emergency", "Type of Event")
                        }
                    },
                    
                    new RootElement("Priority", priority = new RadioGroup ("priority", 0))
                    {
                        new Section()
                        {
                            new RadioElement("Low", "priority"),
                            new RadioElement("Medium", "priority"), 
                            new RadioElement("High", "priority")
                        }
                    }
                },
            };

            Root.Add(info);

            UIButton createEventBtn = UIButton.FromType(UIButtonType.RoundedRect);
            createEventBtn.SetTitle("Add Event", UIControlState.Normal);
            createEventBtn.Frame = new Rectangle(0, 0, 320, 44);
            int y = (int)((View.Frame.Size.Height - createEventBtn.Frame.Size.Height)/1.15);
            int x = ((int)(View.Frame.Size.Width - createEventBtn.Frame.Size.Width)) / 2;
            createEventBtn.Frame = new Rectangle(x, y, (int)createEventBtn.Frame.Width, (int)createEventBtn.Frame.Height);
            View.AddSubview(createEventBtn);


            createEventBtn.TouchUpInside += async (object sender, EventArgs e) =>
            {
                await createEvent(info);
            };
        }
        private async Task createEvent(RootElement info)
        {
            string EventName = eventname.Value;
            string Recipients = recipients.Value;
            string Location = location.Value;
            string Category = categories[category.Selected];
            string Priority = priorities[priority.Selected];
            string EventDescription = description.Value;
            string time = timeelement.Value;
            int hours = parseTime(time);

            DateTime EventDateTime = new DateTime(date.DateValue.Year, date.DateValue.Month, date.DateValue.Day, hours, timeelement.DateValue.Minute, 0);
            

            if (string.IsNullOrEmpty(EventName))
            {
                UIAlertView _error = new UIAlertView("Error", "Event name is required", null, "Ok", null);
                _error.Show();
                return;
            }
            else if (EventDateTime == null || time == null)
            {
                UIAlertView _error = new UIAlertView("Error", "Event date and time are required", null, "Ok", null);
                _error.Show();
                return;
            }
            else if (EventDateTime.Date.CompareTo(DateTime.Now.Date) < 0 ||
                     (EventDateTime.Date.CompareTo(DateTime.Now.Date) == 0 && EventDateTime.Hour < DateTime.Now.Hour))
            {
                UIAlertView _error = new UIAlertView("Error", "Event is set to a past date.", null, "Ok", null);
                _error.Show();
                return;
            }
            else
            {
                try
                {
                    var bounds = UIScreen.MainScreen.Bounds;
                    LoadingOverlay loadingOverlay = new LoadingOverlay(bounds, "Creating event...");
                    View.Add(loadingOverlay);
                    await IoC.EventFactory.createEvent(EventName, Recipients, EventDateTime, time, Location, Category, Priority, EventDescription);

                    loadingOverlay.Hide();

                    UIAlertView _error = new UIAlertView("Success!", "Event creation successful!", null, "Ok", null);
                    _error.Show();

                }
                catch (MobileServicePushFailedException ex)
                {
                    UIAlertView _error = new UIAlertView("Connection Failed", "Internet connection required for Event creation.", null, "Ok", null);
                    _error.Show();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

        }

        private int parseTime(string time)
        {
            DateTime dt = DateTime.Parse(time);
            int hour = int.Parse(dt.ToString("HH"));
            return hour;
        }
    }
}