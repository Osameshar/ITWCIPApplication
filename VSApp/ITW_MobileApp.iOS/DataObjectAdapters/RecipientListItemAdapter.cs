using System.Collections.Generic;

namespace ITW_MobileApp.iOS
{
    public class RecipientListItemAdapter
    { 
        List<RecipientListItem> items = new List<RecipientListItem>();

        public RecipientListItemAdapter()
        {
        }

        public List<EventItem> getEventsByEmployeeID(int employeeID, EventItemAdapter eventAdapter)
        {
            List<EventItem> filteredItems = new List<EventItem>();
            foreach (RecipientListItem currentEvent in items)
            {
                if (currentEvent.EmployeeID == employeeID)
                {
                    EventItem selectedEvent = eventAdapter.getEventByID(currentEvent.EventID);
                    if (selectedEvent != null)
                    {
                        filteredItems.Add(selectedEvent);
                    }
                }
            }
            return filteredItems;
        }

        public void Add(RecipientListItem item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public void Remove(RecipientListItem item)
        {
            items.Remove(item);
        }
    }
}