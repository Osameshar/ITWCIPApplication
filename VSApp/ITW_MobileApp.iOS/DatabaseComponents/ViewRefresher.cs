using System;
using System.Threading.Tasks;

namespace ITW_MobileApp.Droid
{
    public class ViewRefresher
    {

        public async Task RefreshItemsFromTableAsync(EventItemAdapter adapter)
        {
            try
            {
                var eventList = await IoC.Dbconnect.getEventSyncTable().ToListAsync();
                adapter.Clear();
                foreach (EventItem currentEvent in eventList)
                    adapter.Add(currentEvent);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
        public async Task RefreshItemsFromTableAsync(RecipientListItemAdapter adapter)
        {
            try
            {
                var recipientList = await IoC.Dbconnect.getRecipientListSyncTable().ToListAsync();
                adapter.Clear();
                foreach (RecipientListItem currentRecipientList in recipientList)
                    adapter.Add(currentRecipientList);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private async Task RefreshItemsFromTableAsync(EmployeeItemAdapter adapter)
        {
            try
            {
                // Get the items that weren't marked as completed and add them in the adapter
                //var list = await toDoTable.Where(item => item.Complete == false).ToListAsync();
                var employeeList = await IoC.Dbconnect.getEmployeeSyncTable().ToListAsync();
                adapter.Clear();
                foreach (EmployeeItem currentEmployee in employeeList)
                    adapter.Add(currentEmployee);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}