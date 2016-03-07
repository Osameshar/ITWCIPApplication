using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITW_MobileApp.Droid
{
    public class EventFactory
    {        

        public async void createEvent(string newName, string newEventRecipients, DateTime newEventDate, string newEventTime, string newLocation, string newCategory, string newEventPriority, string newEventDescription, int newEventID, int newEmployeeID)
        {
            //create a recipientListitem for every EventRecipient after parsing Event Recipients
            List<int> EmpIds = new List<int>();
            await parseRecipients(newEventRecipients, EmpIds);
            Console.WriteLine(EmpIds);
            foreach (int employeeID in EmpIds)
            {
                
                IoC.RecipientListFactory.createRecipientList(employeeID,newEventID);
               // await IoC.Dbconnect.getRecipientListSyncTable().InsertAsync(new RecipientListItem { EmployeeID = employeeID, EventID = newEventID });
            }
<<<<<<< HEAD
            EmpIds = new List<int>();
=======
>>>>>>> refs/remotes/origin/develop
            await IoC.Dbconnect.getEventSyncTable().InsertAsync(new EventItem { Name = newName, EventRecipients = newEventRecipients, EventDate = newEventDate, EventTime = newEventTime, Location = newLocation, Category = newCategory, EventPriority = newEventPriority, EventDescription = newEventDescription, EventID = newEventID, EmployeeID = newEmployeeID });
            await IoC.Dbconnect.SyncAsync();
        }

        //TODO: potentially make this more efficient
        public async Task parseRecipients(string recipients, List<int> EmpIds)
        {
            
            List<EmployeeItem> empItems = await IoC.Dbconnect.getEmployeeSyncTable().ToListAsync();
            string[] parsedRecipients = recipients.Split(',');

            foreach (string employee in parsedRecipients)
            {
                Console.WriteLine("Employee String " + employee);
                foreach (EmployeeItem employeeItem in empItems)
                {
                    Console.WriteLine("Employee Item " + employeeItem.Name);
                    if (employee.Equals(employeeItem.Name))
                    {
                        Console.WriteLine("ADDING EMPLOYEE: " + employeeItem.Name);
                        EmpIds.Add(employeeItem.EmployeeID);
                    }
                }
            }
        }

    }
}