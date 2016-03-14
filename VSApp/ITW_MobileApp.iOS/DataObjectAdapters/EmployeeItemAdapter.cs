using System.Collections.Generic;

namespace ITW_MobileApp.iOS
{
    public class EmployeeItemAdapter
    {
        List<EmployeeItem> items = new List<EmployeeItem>();

        public EmployeeItemAdapter()
        {
        }

        public void Add(EmployeeItem item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public void Remove(EmployeeItem item)
        {
            items.Remove(item);
        }

        public int getCount()
        {
            return items.Count;
        }
    }
}