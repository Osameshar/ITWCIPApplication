using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ITW_MobileApp.iOS
{
	partial class LoginController : UIViewController
	{
		public LoginController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // code goes here

            LoginButton.TouchUpInside += (object sender, EventArgs e) => {
               // Launches a new instance of CallHistoryController
                ParentController parent = this.Storyboard.InstantiateViewController("ParentController") as ParentController;
                if (parent != null)
                {
                    PresentViewController(parent, true, null);
                }
            };
        }
    }
}
