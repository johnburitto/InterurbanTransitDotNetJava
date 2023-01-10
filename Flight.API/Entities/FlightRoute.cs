namespace Flight.API.Entities
{
    public class FlightRoute : BaseEntity
    {
        public string? FromCity { get; set; }
        public string? ToCity { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }

        public IEnumerable<FlightEntity>? Flights { get; set; }
    }
}
