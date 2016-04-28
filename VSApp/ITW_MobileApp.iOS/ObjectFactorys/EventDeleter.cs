using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITW_MobileApp.iOS
{
    class EventDeleter
    {
        public async Task deleteEvent(int eventID)
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