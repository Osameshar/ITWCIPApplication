using System;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

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

    public class EventItemAdapter : BaseAdapter<EventItem>
    {
        Activity activity;
        int layoutResourceId;
        List<EventItem> items = new List<EventItem>();

        public EventItemAdapter(Activity activity, int layoutResourceId)
        {
            this.activity = activity;
            this.layoutResourceId = layoutResourceId;
        }

        //Returns the view for a specific item on the list
        //TODO: fix view
        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var row = convertView;
            var currentItem = this[position];
            return row;
        }

        public void Add(EventItem item)
        {
            items.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            items.Clear();
            NotifyDataSetChanged();
        }

        public void Remove(EventItem item)
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

        public override EventItem this[int position]
        {
            get
            {
                return items[position];
            }
        }

        #endregion
    }

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

        //Returns the view for a specific item on the list
        //TODO: fix view
        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var row = convertView;
            var currentItem = this[position];
       /*     CheckBox checkBox;

            if (row == null)
            {
                var inflater = activity.LayoutInflater;
                row = inflater.Inflate(layoutResourceId, parent, false);

                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkToDoItem);

                checkBox.CheckedChange += async (sender, e) => {
                    var cbSender = sender as CheckBox;
                    if (cbSender != null && cbSender.Tag is ToDoItemWrapper && cbSender.Checked)
                    {
                        cbSender.Enabled = false;
                        if (activity is ToDoActivity)
                            await ((ToDoActivity)activity).CheckItem((cbSender.Tag as ToDoItemWrapper).ToDoItem);
                    }
                };
            }
            else
                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkToDoItem);

            checkBox.Text = currentItem.Text;
            checkBox.Checked = false;
            checkBox.Enabled = true;
            checkBox.Tag = new ToDoItemWrapper(currentItem);
            */
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

