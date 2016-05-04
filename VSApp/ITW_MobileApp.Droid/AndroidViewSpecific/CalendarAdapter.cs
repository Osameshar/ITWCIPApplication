using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace ITW_MobileApp.Droid
{
    [Activity(Label = "CalendarAdapter")]
    public class CalendarAdapter : BaseAdapter<EventItem>
    {
        private Activity activity;
        private List<EventItem> events;

        public CalendarAdapter(Activity activity, List<EventItem> eventList)
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
                view = activity.LayoutInflater.Inflate(Resource.Layout.CalendarListItems, null);
            }

            EventItem eventItem = events[position];

            view.FindViewById<TextView>(Resource.Id.eventName).Text = eventItem.Name;
            view.FindViewById<TextView>(Resource.Id.eventDate).Text = eventItem.EventDate.ToString();
            view.FindViewById<TextView>(Resource.Id.eventRecipients).Text = eventItem.EventRecipients;

            return view;
        }
        public EventItem GetItemAtPosition(int position)
        {
            return events[position];
        }
    }
}