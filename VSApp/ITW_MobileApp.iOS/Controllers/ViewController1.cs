using System;

using UIKit;

namespace ITW_MobileApp.iOS.Controllers
{
    public partial class ViewController1 : UIViewController
    {
        public ViewController1() : base("ViewController1", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}