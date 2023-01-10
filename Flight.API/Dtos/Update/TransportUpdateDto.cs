using Flight.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Flight.API.Dtos.Update
{
    public class TransportUpdateDto
    {
        [Required] public int Id { get; set; }
        [Required] public string? Barnd { get; set; }
        [Required] public string? Model { get; set; }
        [Required] public Category Category { get; set; }
        [Required] public DateTime ReleaseDate { get; set; }
    }
}
