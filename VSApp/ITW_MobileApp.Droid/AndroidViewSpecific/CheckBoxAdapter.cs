using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace ITW_MobileApp.Droid
{
    [Activity(Label = "CheckBoxAdapter")]
    public class CheckBoxAdapter : BaseAdapter<EventItem>
    {
        private Activity activity;
        private List<EventItem> events;
        public List<EventItem> checkedItems;

        public CheckBoxAdapter(Activity activity, List<EventItem> eventList)
        {
            this.activity = activity;
            this.events = eventList;
            checkedItems = new List<EventItem>();
        }

        public override EventItem this[int position]
        {
            get { return events[position]; }
        }

        public override int Count
        {
            get { return events.Count(); }
        }

        public override long GetItemId(int position)
        {
            return 0;
        }
        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = activity.LayoutInflater.Inflate(Resource.Layout.EventListItems, null);
            }

            EventItem eventItem = events[position];

            view.FindViewById<TextView>(Resource.Id.eventName).Text = eventItem.Name;
            view.FindViewById<TextView>(Resource.Id.eventDate).Text = eventItem.EventDate.ToString();
            view.FindViewById<TextView>(Resource.Id.eventRecipients).Text = eventItem.EventRecipients;

            CheckBox chkEvent = view.FindViewById<CheckBox>(Resource.Id.chkEvent);
            chkEvent.Tag = eventItem.Name;

            chkEvent.SetOnCheckedChangeListener(null);
            chkEvent.Checked = eventItem.IsDeleted;
            chkEvent.SetOnCheckedChangeListener(new CheckedChangeListener(activity,position,events, this));

            return view;
        }
        public EventItem GetItemAtPosition(int position)
        {
            return events[position];
        }
        public List<EventItem> getCheckedList()
        {
            return checkedItems;
        }
        public class CheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
        {
            private Activity activity;
            private int index;
            private List<EventItem> localEvents;
            private CheckBoxAdapter localAdapter;

            public CheckedChangeListener(Activity activity, int position, List<EventItem> events, CheckBoxAdapter adapter)
            {
                this.activity = activity;
                index = position;
                localEvents = events;
                localAdapter = adapter;
            }

            public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
            {
                if (isChecked)
                {
                    localAdapter.getCheckedList().Add(localEvents[index]);
                }
                else
                {
                    localAdapter.getCheckedList().Remove(localEvents[index]);
                }
            }
        }
    }
}