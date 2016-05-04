using System.Collections.Generic;

namespace ITW_MobileApp.iOS
{
    public class EventItemAdapter
    {
        List<EventItem> items = new List<EventItem>();

        public EventItemAdapter()
        {
        }

        public EventItem getEventByID(int eventID)
        {
            foreach (EventItem eventItem in items)
            {
                if (eventItem.EventID == eventID)
                {
                    return eventItem;
                }
            }
            return null;
        }
        public EventItem getEventByIDNotDeleted(int eventID)
        {
            foreach (EventItem eventItem in items)
            {
                if (eventItem.EventID == eventID && eventItem.IsDeleted == false)
                {
                    return eventItem;
                }
            }
            return null;
        }
        public List<EventItem> getEventsByEmployeeID()
        {
            List<EventItem> filteredItems = new List<EventItem>();
            foreach (EventItem eventItem in items)
            {
                if (eventItem.EmployeeID == IoC.UserInfo.EmployeeID && eventItem.IsDeleted == false)
                {
                    filteredItems.Add(eventItem);
                }
            }
            return filteredItems;
        }

        public void Add(EventItem item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public void Remove(EventItem item)
        {
            items.Remove(item);
        }
    }
}