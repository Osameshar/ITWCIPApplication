using System.Threading.Tasks;

namespace ITW_MobileApp.iOS
{
    public class UserInfo
    {
        public int EmployeeID { get; set; }
        public EmployeeItem Employee { get; set; }
        public async Task setEmployee()
        {
            EmployeeItemAdapter employeeItemAdapter = new EmployeeItemAdapter();
            await IoC.ViewRefresher.RefreshItemsFromTableAsync(employeeItemAdapter);
            Employee = employeeItemAdapter.findEmployeeByEmployeeID(EmployeeID);
        }
    }
}