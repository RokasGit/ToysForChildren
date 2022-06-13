using System.ComponentModel.DataAnnotations;

namespace Blazor.Models;

public class Child
{
    public int Id { get; set; }
    [Required,MaxLength(50)]
    public string Name { get; set; }
    [Required,Range(3,6)]
    public int Age { get; set; }
    [Required]
    public string Gender { get; set; }
    public IList<Toy> Toys { get; set; }

    public Child()
    {
        Toys = new List<Toy>();
    }
}