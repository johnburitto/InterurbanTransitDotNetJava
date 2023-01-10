using Flight.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Flight.API.Dtos.Create
{
    public class DriverCreateDto
    {
        [Required] public string? Name { get; set; }
        [Required] public float Experience { get; set; }
        [Required] public Category Category { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
    }
}
