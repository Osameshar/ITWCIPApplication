using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ITW_MobileApp.Droid
{
    public class EventViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name { get; private set; }
        public TextView Date { get; private set; }
        public TextView Time { get; private set; }
        public TextView Category { get; private set; }

        public EventViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            Name = itemView.FindViewById<TextView>(Resource.Id.Name);
            Date = itemView.FindViewById<TextView>(Resource.Id.Date);
            Time = itemView.FindViewById<TextView>(Resource.Id.Time);
            Category = itemView.FindViewById<TextView>(Resource.Id.Category);
        }
    }
}