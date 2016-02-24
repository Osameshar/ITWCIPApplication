using System;
using Newtonsoft.Json;

namespace ITW_MobileApp.Droid
{
    public class EventItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "EventRecipients")]
        public string EventRecipients { get; set; }

        [JsonProperty(PropertyName = "EventDate")]
        public DateTime EventDate { get; set; }

        [JsonProperty(PropertyName = "EventTime")]
        public string EventTime { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "EventPriority")]
        public string EventPriority { get; set; }

        [JsonProperty(PropertyName = "EventDescription")]
        public string EventDescription { get; set; }

        [JsonProperty(PropertyName = "EventID")]
        public int EventID { get; set; }

        [JsonProperty(PropertyName = "EmployeeID")]
        public int EmployeeID { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public bool deleted { get; set; }

    }

    public class EventItemWrapper : Java.Lang.Object
    {
        public EventItemWrapper(EventItem item)
        {
            EventItem = item;
        }

        public EventItem EventItem { get; private set; }
    }
}