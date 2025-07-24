
using System.ComponentModel.DataAnnotations;

public class SwordDTO
{    
    public Guid SwordGuid { get; set; } 
    public String Name { get; set; }    
    public String Description { get; set; }    
    public DateTime ForgedDate { get; set; }
}