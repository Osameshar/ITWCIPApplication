using System;
using Newtonsoft.Json;

namespace ITW_MobileApp.iOS
{
    public class RecipientListItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "EmployeeID")]
        public int EmployeeID { get; set; }

        [JsonProperty(PropertyName = "EventID")]
        public int EventID { get; set; }
    }
