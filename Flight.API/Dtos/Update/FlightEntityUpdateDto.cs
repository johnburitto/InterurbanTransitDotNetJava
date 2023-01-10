using System.ComponentModel.DataAnnotations;

namespace Flight.API.Dtos.Update
{
    public class FlightEntityUpdateDto
    {
        [Required] public int Id { get; set; }
        [Required] public int DriverId { get; set; }
        [Required] public int TransportId { get; set; }
        [Required] public int RouteId { get; set; }
        [Required] public DateTime StartDay { get; set; }
        [Required] public DateTime EndDay { get; set; }
    }
}
