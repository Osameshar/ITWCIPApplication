using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using MonoTouch.Dialog;
using CoreGraphics;

namespace ITW_MobileApp.iOS
{
    public class DeleteEventController : DialogViewController
    {
        public DeleteEventController() : base(new RootElement("Event Deletion"), true)
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

            UIButton myBUtton = UIButton.FromType(UIButtonType.RoundedRect);
            myBUtton.SetTitle("Delete Events", UIControlState.Normal);
            myBUtton.Frame = new Rectangle(22, 0, 320, 44);
            View.AddSubview(myBUtton);

            var page = new RootElement("Event Deletion Page") {

                new Section()
                {

                }
            };

            Root.Add(page);
        } 
    }
}