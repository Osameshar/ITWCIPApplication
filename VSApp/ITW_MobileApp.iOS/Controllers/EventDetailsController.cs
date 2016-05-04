using MonoTouch.Dialog;

using UIKit;

namespace ITW_MobileApp.iOS
{
    public  class EventDetailsController : DialogViewController
    {
        EventItem MyEvent;
        public EventDetailsController(EventItem Event) : base(new RootElement("Event Details"), true)
        {
            MyEvent = Event;
            UIImage hamburgericon = UIImage.FromFile("Menu Filled-20");

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, delegate
            {
                NavigationController.PopViewController(true);
            });

        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            var info = new RootElement("Info") {
                new Section ("Category"){
                    new StringElement (MyEvent.Category)
                },
                new Section ("Details"){
                    new StringElement ("Date:  " + MyEvent.EventDate.ToString("MM/dd/yyy")),
                    new StringElement ("Time:  " + MyEvent.EventDate.ToString("h:mm tt")),
                    new StringElement ("Location:  " + MyEvent.Location)
                },
                new Section ("Description"){
                    new StringElement (MyEvent.EventDescription)
                },

            };
            Root.Add(info);
        }
    }
}