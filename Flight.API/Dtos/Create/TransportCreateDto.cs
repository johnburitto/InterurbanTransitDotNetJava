using Flight.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Flight.API.Dtos.Create
{
    public class TransportCreateDto
    {
        [Required] public string? Barnd { get; set; }
        [Required] public string? Model { get; set; }
        [Required] public Category Category { get; set; }
        [Required] public DateTime ReleaseDate { get; set; }
    }
}
