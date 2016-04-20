using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITW_MobileApp.iOS
{
    public class RecipientListFactory
    {
        public async Task createRecipientList(int newEmployeeID, int newEventID)
        {
            await IoC.Dbconnect.getRecipientListSyncTable().InsertAsync(new RecipientListItem {EmployeeID = newEmployeeID, EventID = newEventID });
            await IoC.Dbconnect.SyncAsync();
        }
    }
}