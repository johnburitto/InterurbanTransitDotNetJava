namespace Flight.API.Entities
{
    public class FlightEntity : BaseEntity
    {
        public int DriverId { get; set; }
        public int TransportId { get; set; }
        public int RouteId { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }

        public Driver? Driver { get; set; }
        public Transport? Transport { get; set; }
        public FlightRoute? Route { get; set; }
    }
}
