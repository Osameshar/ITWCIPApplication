using System;
using System.Collections.Generic;

namespace ITW_MobileApp.iOS
{
    public class EmployeeFactory
    {
        public async void createEmployee(string newName, string newEmail, int newEmployeeID, string newDepartment, string newPrivledgeLevel)
        {
            await IoC.Dbconnect.getEmployeeSyncTable().InsertAsync(new EmployeeItem { Name = newName, Email = newEmail, EmployeeID = newEmployeeID, Department = newDepartment, PrivledgeLevel = newPrivledgeLevel});
            await IoC.Dbconnect.SyncAsync();
        }
    }
}