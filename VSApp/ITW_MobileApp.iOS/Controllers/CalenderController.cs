using MonoTouch.Dialog;
using Factorymind.Components;

using UIKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITW_MobileApp.iOS
{
    public class CalendarController : DialogViewController
    {
        private FMCalendar fmCalendar = new FMCalendar();
        private List<EventItem> myEventList;

        EventItemAdapter eventItemAdapter;
        RecipientListItemAdapter recipientListItemAdapter;

        public CalendarController() : base(new RootElement("Event Details"), true)
        {
            UIImage hamburgericon = UIImage.FromFile("Menu Filled-20");

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(hamburgericon, UIBarButtonItemStyle.Plain, delegate
            {
                ParentController.getNavigationMenu().ToggleMenu();
            });

            eventItemAdapter = new EventItemAdapter();
            recipientListItemAdapter = new RecipientListItemAdapter();

        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
 
            await RefreshView();

            myEventList = recipientListItemAdapter.getEventsByEmployeeID(IoC.UserInfo.EmployeeID, eventItemAdapter);
            myEventList = filterEvents();
            fmCalendar = new FMCalendar(View.Bounds);

            View.BackgroundColor = UIColor.White;

            // Specify selection color
            fmCalendar.SelectionColor = UIColor.Red;

            // Specify today circle Color
            fmCalendar.TodayCircleColor = UIColor.Red;

            // Customizing appearance
            fmCalendar.LeftArrow = UIImage.FromFile("leftArrow.png");
            fmCalendar.RightArrow = UIImage.FromFile("rightArrow.png");

            fmCalendar.MonthFormatString = "MMMM yyyy";

            // Shows Sunday as last day of the week
            fmCalendar.SundayFirst = false;

            // Mark with a dot dates that fulfill the predicate
            //for (int i = 0; i < myEventList.Count; )
            List<int> daysofyear = getDateList();
            fmCalendar.IsDayMarkedDelegate = (date) =>
            {

                if (daysofyear.Contains(date.DayOfYear))
                {
                    return true;
                }
                else
                    return false;
                //  return date.DayOfYear == myEventList.EventDate.DayOfYear;                
            };

                    // Turn gray dates that fulfill the predicate
            fmCalendar.IsDateAvailable = (date) =>
            {
                return (date >= DateTime.Today);
            };

            fmCalendar.DateSelected += (date) =>
            {
                List<EventItem> EventsOnDate = filterEventsByDate(myEventList, date);

                CalendarListController eventList = new CalendarListController(EventsOnDate);
                NavigationController.PushViewController(eventList, true);
            };

            // Add FMCalendar to SuperView
            fmCalendar.Center = this.View.Center;
            this.View.AddSubview(fmCalendar);
        }

        public override async void ViewDidAppear(bool animated)
        {
            

            await RefreshView();

            myEventList = recipientListItemAdapter.getEventsByEmployeeID(IoC.UserInfo.EmployeeID, eventItemAdapter);
            myEventList = filterEvents();
            fmCalendar = new FMCalendar(View.Bounds);

            View.BackgroundColor = UIColor.White;

            // Specify selection color
            fmCalendar.SelectionColor = UIColor.Red;

            // Specify today circle Color
            fmCalendar.TodayCircleColor = UIColor.Red;

            // Customizing appearance
            fmCalendar.LeftArrow = UIImage.FromFile("leftArrow.png");
            fmCalendar.RightArrow = UIImage.FromFile("rightArrow.png");

            fmCalendar.MonthFormatString = "MMMM yyyy";

            // Shows Sunday as last day of the week
            fmCalendar.SundayFirst = false;

            // Mark with a dot dates that fulfill the predicate
            //for (int i = 0; i < myEventList.Count; )
            List<int> daysofyear = getDateList();
            fmCalendar.IsDayMarkedDelegate = (date) =>
            {

                if (daysofyear.Contains(date.DayOfYear))
                {
                    return true;
                }
                else
                    return false;
                //  return date.DayOfYear == myEventList.EventDate.DayOfYear;                
            };

            // Turn gray dates that fulfill the predicate
            fmCalendar.IsDateAvailable = (date) =>
            {
                return (date >= DateTime.Today);
            };

            fmCalendar.DateSelected += (date) =>
            {

                List<EventItem> EventsOnDate = recipientListItemAdapter.getEventsByEmployeeID(IoC.UserInfo.EmployeeID, eventItemAdapter);
                EventsOnDate = filterEventsByDate(EventsOnDate, date);

                CalendarListController eventList = new CalendarListController(EventsOnDate);
                NavigationController.PushViewController(eventList, true);
            };

            // Add FMCalendar to SuperView
            fmCalendar.Center = this.View.Center;

            this.View.AddSubview(fmCalendar);
            base.ViewDidAppear(animated);
        }
        public List<int> getDateList()
        {
            List<int> dates = new List<int>();
            foreach (EventItem item in myEventList)
            {
                dates.Add(item.EventDate.DayOfYear);
            }
            return dates;
        }
        public async Task RefreshView()
        {
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(eventItemAdapter);
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(recipientListItemAdapter);
        }
        private List<EventItem> filterEvents()
        {
            List<EventItem> filteredList = new List<EventItem>();
            foreach (EventItem eventitem in myEventList)
            {
                    foreach (string filter in IoC.ViewRefresher.FilterStringList)
                    {
                        if (eventitem.Category == filter)
                        {
                            filteredList.Add(eventitem);
                        }
                    }
                    if (eventitem.Category == "Emergency")
                    {
                        filteredList.Add(eventitem);
                    }
            }
            return filteredList;
        }
        private List<EventItem> filterEventsByDate(List<EventItem> eventList, DateTime dateSelected)
        {
            List<EventItem> filteredEvents = new List<EventItem>();

            foreach (EventItem eventItem in eventList)
            {
                if (eventItem.EventDate.DayOfYear == dateSelected.DayOfYear)
                {
                    filteredEvents.Add(eventItem);
                }
            }
            return filteredEvents;
        }

    }

}