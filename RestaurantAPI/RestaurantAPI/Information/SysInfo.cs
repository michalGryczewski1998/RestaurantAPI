namespace RestaurantAPI.Information
{
    public class SysInfo
    {
        public string MachineName { get; set; }
        public string OSDescription { get; set; }
        public string OSArchitecture { get; set; }
        public int ProcessorCount { get; set; }
        public string Framework { get; set; }
        public string CurrentDirectory { get; set; }
        public TimeSpan Uptime { get; set; }
    }
}
