using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITW_MobileApp.Droid
{
    public class EventFactory
    {
        int newEventID = 0;
        public async Task createEvent(string newName, string newEventRecipients, DateTime newEventDate, string newEventTime, string newLocation, string newCategory, string newEventPriority, string newEventDescription)
        {
            try {
                await IoC.Dbconnect.SyncAsyncConnected(pullData: true);
                await setNextEventID();

                //create a recipientListitem for every EventRecipient after parsing Event Recipients
                List<int> EmpIds = new List<int>();
                await parseRecipients(newEventRecipients, EmpIds);

                foreach (int employeeID in EmpIds)
                {
                    IoC.RecipientListFactory.createRecipientList(employeeID, newEventID);
                }

                await IoC.Dbconnect.getEventSyncTable().InsertAsync(new EventItem { Name = newName, EventRecipients = newEventRecipients, EventDate = newEventDate, EventTime = newEventTime, Location = newLocation, Category = newCategory, EventPriority = newEventPriority, EventDescription = newEventDescription, EventID = newEventID, EmployeeID = IoC.UserInfo.EmployeeID, IsDeleted = false });
                await IoC.Dbconnect.SyncAsyncConnected(pullData: true);
            }
            catch (MobileServicePushFailedException)
            {
                throw;
            }
            catch (Java.Net.UnknownHostException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task setNextEventID()
        {
            List<EventItem> eventItems = await IoC.Dbconnect.getEventSyncTable().ToListAsync();
            int highestID = -1;
            foreach (EventItem eventItem in eventItems)
            {
                int currID = eventItem.EventID;
                if (highestID < currID)
                {                    
                    highestID = currID;
                }
            }
            newEventID = highestID + 1;
        }

        //TODO: potentially make this more efficient
        public async Task parseRecipients(string recipients, List<int> EmpIds)
        {
            
            List<EmployeeItem> empItems = await IoC.Dbconnect.getEmployeeSyncTable().ToListAsync();
            string[] parsedRecipients = recipients.Split(',');

            foreach (string employee in parsedRecipients)
            {
                foreach (EmployeeItem employeeItem in empItems)
                {
                    if (employee.Equals(employeeItem.Name))
                    {
                        EmpIds.Add(employeeItem.EmployeeID);
                    }
                }
            }
        }

    }
}