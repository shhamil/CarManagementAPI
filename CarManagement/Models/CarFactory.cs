namespace CarManagement.Models
{
    public class CarFactory
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
