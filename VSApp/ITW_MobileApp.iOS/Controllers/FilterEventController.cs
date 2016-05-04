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
using System.Collections.Generic;

namespace ITW_MobileApp.iOS
{
    public class FilterEventsController : DialogViewController
    {
        CheckboxElement meeting;
        CheckboxElement companyevent;
        CheckboxElement machinemaint;


        public FilterEventsController() : base(new RootElement("Filter Events"), true)
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
                    { meeting = new CheckboxElement("Meeting",true)},
                    { companyevent = new CheckboxElement("Company Event",true) },
                    { machinemaint = new CheckboxElement("Machine Maintenance" ,true) }
                },              
            };

            Root.Add(info);

            UIButton filterEventsBtn = UIButton.FromType(UIButtonType.RoundedRect);
            filterEventsBtn.SetTitle("Filter Events", UIControlState.Normal);
            filterEventsBtn.Frame = new Rectangle(0, 0, 320, 44);
            int y = (int)((View.Frame.Size.Height - filterEventsBtn.Frame.Size.Height) / 1.25);
            int x = ((int)(View.Frame.Size.Width - filterEventsBtn.Frame.Size.Width)) / 2;
            filterEventsBtn.Frame = new Rectangle(x, y, (int)filterEventsBtn.Frame.Width, (int)filterEventsBtn.Frame.Height);
            View.AddSubview(filterEventsBtn);


            filterEventsBtn.TouchUpInside += (object sender, EventArgs e) =>
            {
                setFilter();
                UIAlertView _error = new UIAlertView("Success!", "Event filter successful!", null, "Ok", null);
                _error.Show();
            };
        }
        public void setFilter()
        {
            List<string> filteredList = new List<string>();
            if (meeting.Value == true)
            {
                filteredList.Add("Meeting");
            }
            if (companyevent.Value == true)
            {
                filteredList.Add("Company Event");
            }
            if (machinemaint.Value == true)
            {
                filteredList.Add("Machine Maintenance");
            }
            IoC.ViewRefresher.FilterStringList = filteredList;
        }

    }
    
}