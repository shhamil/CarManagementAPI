namespace CarManagement.Models
{
    public class Car
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }

        public int carfactoryid { get; set; }
        public CarFactory CarFactory { get; set; }
    }
}
