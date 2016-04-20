using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using MonoTouch.Dialog;
using CoreGraphics;

namespace ITW_MobileApp.iOS
{
    public class CreateEventController : DialogViewController
    {
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
                    new EntryElement("Event Name", "Enter the Name", null),
                    new EntryElement("Recipients' Names", "Enter the Recipients", null),
                    new EntryElement("Enter Location", "Enter the Location", null)
                },

                new Section()
                {
                    new DateElement("Pick the Date", DateTime.Now)
                },

                new Section()
                {
                    new TimeElement("Pick the Time", DateTime.Now)
                },

                new Section()
                {
                    new RootElement("Type", new RadioGroup("Type of Event", 0)) {
                        new Section() {
                            new RadioElement ("Meeting", "Type of Event"),
                            new RadioElement ("Company Event", "Type of Event"),
                            new RadioElement ("Machine Maintenance", "Type of Event"),
                            new RadioElement ("Emergency", "Type of Event")
                        }
                    },
                    
                    new RootElement("Priority", new RadioGroup ("priority", 0))
                    {
                        new Section()
                        {
                            new RadioElement("Low", "priority"),
                            new RadioElement("Medium", "priority"), 
                            new RadioElement("High", "priority")
                        }
                    }
                },

                new Section()
                {
                    new EntryElement("Description", "Enter a Description", null)
                }
            };

            Root.Add(info);

            UIButton myBUtton = UIButton.FromType(UIButtonType.RoundedRect);
            myBUtton.SetTitle("Add Event", UIControlState.Normal);
            myBUtton.Frame = new Rectangle(22, 545, 320, 44);
            View.AddSubview(myBUtton);
        }
    }
}