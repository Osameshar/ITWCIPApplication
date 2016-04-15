using Newtonsoft.Json;

namespace ITW_MobileApp.Droid
{
    public class EmployeeLoginItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "EmployeeID")]
        public int EmployeeID { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }
    }
}