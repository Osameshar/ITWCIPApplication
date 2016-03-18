using System.Collections.Generic;
using System.Linq;

using Android.Views;
using Android.Support.V7.Widget;
using System;

namespace ITW_MobileApp.Droid
{
    public class EventListAdapter : RecyclerView.Adapter
    {
        public List<EventItem> adaptereventlist { get; private set; }
        public event EventHandler<int> ItemClick;

        public EventListAdapter(List<EventItem> myEventList)
        {
            adaptereventlist = myEventList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EventCardView, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            EventViewHolder vh = new EventViewHolder(itemView, OnClick);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            EventViewHolder vh = holder as EventViewHolder;

            // Load the photo caption from the photo album

            vh.Name.Text = adaptereventlist.ElementAt(position).Name;
            vh.Date.Text = adaptereventlist.ElementAt(position).EventDate.ToString("MMMM dd, yyyy");
            vh.Time.Text = adaptereventlist.ElementAt(position).EventTime;
            vh.Category.Text = adaptereventlist.ElementAt(position).Category;

            if (vh.Category.Text == "Meeting")
            {
                vh.Category.SetBackgroundColor(new Android.Graphics.Color(0, 0, 255));
            }

            else if (vh.Category.Text == "Company Event")
            {
                vh.Category.SetBackgroundColor(new Android.Graphics.Color(0, 150, 0));
            }

            else if (vh.Category.Text == "Emergency")
            {
                vh.Category.SetBackgroundColor(new Android.Graphics.Color(255, 0, 0));
            }

            else if (vh.Category.Text == "Machine Maintenance")
            {
                vh.Category.SetBackgroundColor(new Android.Graphics.Color(255, 165, 0));
            }

            else
            {
                vh.Category.SetBackgroundColor(new Android.Graphics.Color(0, 0, 255));
            }
        }

        public override int ItemCount
        {
            get { return adaptereventlist.Count; }
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}