using Android.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ITW_MobileApp.Droid
{
    public class EmployeeItemAdapter : BaseAdapter<EmployeeItem>
    {
        Activity activity;
        int layoutResourceId;
        List<EmployeeItem> items = new List<EmployeeItem>();

        public EmployeeItemAdapter(Activity activity, int layoutResourceId)
        {
            this.activity = activity;
            this.layoutResourceId = layoutResourceId;
        }
        public EmployeeItemAdapter()
        {
        }
        public EmployeeItem findEmployeeByEmployeeID(int employeeID)
        {
            foreach (EmployeeItem item in items)
            {
                if (item.EmployeeID == employeeID)
                {
                    return item;
                }
            }
            return null;
        }
        public List<string> getAutoCompleteList()
        {
            List<string> entries = new List<string>();
            foreach (EmployeeItem item in items)
            {
                if (!entries.Contains(item.Department))
                {
                    entries.Add(item.Department);
                }
                entries.Add(item.Name);
            }
            return entries;
        }

        public List<string> getDepartmentList()
        {
            List<string> departments = new List<string>();
            foreach (EmployeeItem item in items)
            {
                if(departments.Contains(item.Department))
                departments.Add(item.Department);
            }
            return departments;
        }
        //Returns the view for a specific item on the list
        //TODO: fix view
        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var row = convertView;
            var currentItem = this[position];

            return row;
        }

        public void Add(EmployeeItem item)
        {
            items.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            items.Clear();
            NotifyDataSetChanged();
        }

        public void Remove(EmployeeItem item)
        {
            items.Remove(item);
            NotifyDataSetChanged();
        }

        public int getCount()
        {
            return items.Count;
        }
        #region implemented abstract members of BaseAdapter

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override EmployeeItem this[int position]
        {
            get
            {
                return items[position];
            }
        }

        #endregion
    }
}