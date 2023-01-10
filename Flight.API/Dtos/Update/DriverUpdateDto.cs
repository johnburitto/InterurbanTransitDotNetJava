using Flight.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Flight.API.Dtos.Update
{
    public class DriverUpdateDto
    {
        [Required] public int Id { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public float Experience { get; set; }
        [Required] public Category Category { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
    }
}
