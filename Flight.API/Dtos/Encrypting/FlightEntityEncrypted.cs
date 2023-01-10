namespace Flight.API.Dtos.Encrypting
{
    public class FlightEntityEncrypted
    {
        public byte[]? Id { get; set; }
        public byte[]? DriverId { get; set; }
        public byte[]? TransportId { get; set; }
        public byte[]? RouteId { get; set; }
        public byte[]? StartDay { get; set; }
        public byte[]? EndDay { get; set; }
        public byte[]? CreatedAt { get; set; }
        public byte[]? UpdatedAt { get; set; }
    }
}
