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

        public CheckBoxAdapter(Activity activity, List<EventItem> eventList)
        {
            this.activity = activity;
            this.events = eventList;
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
            chkEvent.Checked = eventItem.deleted;
            chkEvent.SetOnCheckedChangeListener(new CheckedChangeListener(activity));

            return view;
        }
        public class CheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
        {
            private Activity activity;

            public CheckedChangeListener(Activity activity)
            {
                this.activity = activity;
            }

            public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
            {
                if (isChecked)
                {

                }
            }
        }
    }
}