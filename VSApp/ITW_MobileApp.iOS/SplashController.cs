using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ITW_MobileApp.iOS
{
	partial class SplashController : UIViewController
	{
		public SplashController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // code goes here

            LoginController controller = Storyboard.InstantiateViewController("LoginController") as LoginController;
            this.NavigationController.PushViewController(controller, true);
        }

    }

}
