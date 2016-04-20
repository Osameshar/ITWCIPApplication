using System.Collections.Generic;

namespace ITW_MobileApp.iOS
{
    class EventDeleter
    {
        public async void deleteEvent(int eventID)
        {
           List<EventItem> eventList = await IoC.Dbconnect.getEventSyncTable().Where(items => items.EventID == eventID).ToListAsync();
            foreach (EventItem eventItem in eventList)
            {
                if (eventItem.EventID == eventID)
                {
                    eventItem.IsDeleted = true;
                    await IoC.Dbconnect.getEventSyncTable().UpdateAsync(eventItem);
                }
            }
        }
    }
}