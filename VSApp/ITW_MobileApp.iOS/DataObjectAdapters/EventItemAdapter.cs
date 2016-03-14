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