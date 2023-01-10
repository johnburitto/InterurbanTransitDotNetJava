namespace Flight.API.Dtos.Encrypting
{
    public class FlightRouteEncrypted
    {
        public byte[]? Id { get; set; }
        public byte[]? FromCity { get; set; }
        public byte[]? ToCity { get; set; }
        public byte[]? DepartureTime { get; set; }
        public byte[]? ArrivalTime { get; set; }
        public byte[]? CreatedAt { get; set; }
        public byte[]? UpdatedAt { get; set; }
    }
}
