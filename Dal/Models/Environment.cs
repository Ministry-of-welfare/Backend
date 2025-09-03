using System.ComponentModel.DataAnnotations;

namespace server_pra.Models;

public partial class Environment
{
    [Key]
    public int EnvironmentId { get; set; }

    [Required]
    [StringLength(20)]
    public string EnvironmentCode { get; set; }

    [Required]
    [StringLength(50)]
    public string EnvironmentName { get; set; }

    [StringLength(255)]
    public string Description { get; set; }
}
