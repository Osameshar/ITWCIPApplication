using System;
using Newtonsoft.Json;

namespace ITW_MobileApp.Droid
{
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
}