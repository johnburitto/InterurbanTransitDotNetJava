namespace Flight.API.Entities
{
    public class Driver : BaseEntity
    {
        public string? Name { get; set; }
        public float Experience { get; set; }
        public Category Category { get; set; }
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<FlightEntity>? Flights { get; set; }
    }
}
