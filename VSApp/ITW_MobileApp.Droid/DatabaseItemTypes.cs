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

    public class EmployeeItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "EmployeeID")]
        public int EmployeeID { get; set; }

        [JsonProperty(PropertyName = "Department")]
        public string Department { get; set; }

        [JsonProperty(PropertyName = "PrivledgeLevel")]
        public string PrivledgeLevel { get; set; }
    }

    public class EmployeeItemWrapper : Java.Lang.Object
    {
        public EmployeeItemWrapper(EmployeeItem item)
        {
            EmployeeItem = item;
        }

        public EmployeeItem EmployeeItem { get; private set; }
    }

    public class RecipientListItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "EmployeeID")]
        public int EmployeeID { get; set; }

        [JsonProperty(PropertyName = "EventID")]
        public int EventID { get; set; }
    }

    public class RecipientListItemWrapper : Java.Lang.Object
    {
        public RecipientListItemWrapper(RecipientListItem item)
        {
            RecipientListItem = item;
        }

        public RecipientListItem RecipientListItem { get; private set; }
    }

}