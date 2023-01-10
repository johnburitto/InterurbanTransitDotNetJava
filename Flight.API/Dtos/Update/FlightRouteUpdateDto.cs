using System.ComponentModel.DataAnnotations;

namespace Flight.API.Dtos.Update
{
    public class FlightRouteUpdateDto
    {
        [Required] public int Id { get; set; }
        [Required] public string? FromCity { get; set; }
        [Required] public string? ToCity { get; set; }
        [Required] public string? DepartureTime { get; set; }
        [Required] public string? ArrivalTime { get; set; }
    }
}
