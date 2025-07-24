using System.ComponentModel.DataAnnotations;

public class Cart
{

    [Key]
    public Guid cartId { get; set; }

    [Required]
    public List<SwordDTO>? Swords { get; set; }
}