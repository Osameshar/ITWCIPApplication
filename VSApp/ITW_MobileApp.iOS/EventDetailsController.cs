using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.Dialog;

using Foundation;
using UIKit;

namespace ITW_MobileApp.iOS
{
    public  class EventDetailsController : DialogViewController
    {
        EventItem MyEvent;
        public EventDetailsController(EventItem Event) : base(UITableViewStyle.Grouped, null)
        {
            MyEvent = Event;
            UIImage hamburgericon = UIImage.FromFile("Menu Filled-20");
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(hamburgericon, UIBarButtonItemStyle.Plain, delegate
            {
                ParentController.getNavigationMenu().ToggleMenu();
            });

        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            Root = new RootElement(MyEvent.Name) {
                new Section ("Category"){
                    new StringElement (MyEvent.Category)
                },
                new Section ("Details"){
                    new StringElement ("Date:  " + MyEvent.EventDate.ToString("MM/dd/yyy")),
                    new StringElement ("Time:  " + MyEvent.EventTime),
                    new StringElement ("Location:  " + MyEvent.Location)
                },
                new Section ("Description"){
                    new StringElement (MyEvent.EventDescription)
                },

            };
            

            UIButton myBUtton = UIButton.FromType(UIButtonType.RoundedRect);
            myBUtton.SetTitle("Event Details", UIControlState.Normal);
            myBUtton.Frame = new Rectangle(22, 545, 320, 44);
            View.AddSubview(myBUtton);
        }
    }
}