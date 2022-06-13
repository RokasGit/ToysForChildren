using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class Toy
{
    [Key]
    public int Id { get; set; }
    [Required,MaxLength(20)]
    public string Name { get; set; }
    public string Color { get; set; }
    public string Condition { get; set; }
    public bool IsFavourite { get; set; }
}