using System;
using System.Collections.Generic;

namespace ITW_MobileApp.Droid
{
    public class RecipientListFactory
    {
        public async void createRecipientList(int newEmployeeID, int newEventID)
        {
            await IoC.Dbconnect.getRecipientListSyncTable().InsertAsync(new RecipientListItem {EmployeeID = newEmployeeID, EventID = newEventID });
            await IoC.Dbconnect.SyncAsync();
        }
    }
}