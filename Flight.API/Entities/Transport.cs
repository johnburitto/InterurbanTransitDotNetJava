namespace Flight.API.Entities
{
    public class Transport : BaseEntity
    {
        public string? Barnd { get; set; }
        public string? Model { get; set;}
        public Category Category { get; set; }
        public DateTime ReleaseDate { get; set; }

        public IEnumerable<FlightEntity>? Flights { get; set; }
    }
}
