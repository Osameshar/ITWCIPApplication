using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace ITW_MobileApp.Droid
{
    public class RecipientListItemAdapter : BaseAdapter<RecipientListItem>
    {
        Activity activity;
        int layoutResourceId;
        List<RecipientListItem> items = new List<RecipientListItem>();

        public RecipientListItemAdapter(Activity activity, int layoutResourceId)
        {
            this.activity = activity;
            this.layoutResourceId = layoutResourceId;
        }

        public RecipientListItemAdapter()
        {
        }

        public List<EventItem> getEventsByEmployeeID(int employeeID, EventItemAdapter eventAdapter)
        {
            List<EventItem> filteredItems = new List<EventItem>();
            foreach (RecipientListItem currentEvent in items)
            {
                if (currentEvent.EmployeeID == employeeID)
                {
                    EventItem selectedEvent = eventAdapter.getEventByIDNotDeleted(currentEvent.EventID);
                    if (selectedEvent != null)
                    {
                        filteredItems.Add(selectedEvent);
                    }
                }
            }
            return filteredItems;
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var row = convertView;
            var currentItem = this[position];
            
            return row;
        }

        public void Add(RecipientListItem item)
        {
            items.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            items.Clear();
            NotifyDataSetChanged();
        }

        public void Remove(RecipientListItem item)
        {
            items.Remove(item);
            NotifyDataSetChanged();
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

        public override RecipientListItem this[int position]
        {
            get
            {
                return items[position];
            }
        }

        #endregion
    }
}