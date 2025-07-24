
using System.ComponentModel.DataAnnotations;

public class Sword
{
    [Key]
    public Guid SwordGuid { get; set; }

    [Required]
    public Guid OrderGuid { get; set; }

     [Required]
    public Order Order { get; set; }

    [Required]
    public String Name { get; set; }

    [Required]
    public String Description { get; set; }

    [Required]
    public DateTime ForgedDate { get; set; }
}