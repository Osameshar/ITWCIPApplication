namespace ITW_MobileApp.Droid
{
    public static class IoC
    {
        public static DatabaseConnection Dbconnect { get; set; }
        public static EventFactory EventFactory { get; set; }
        public static ViewRefresher ViewRefresher { get; set; }
    }
}