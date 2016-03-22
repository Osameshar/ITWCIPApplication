using System;
using Newtonsoft.Json;

namespace ITW_MobileApp.Droid
{
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