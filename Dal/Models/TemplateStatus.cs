using System.ComponentModel.DataAnnotations;

namespace server_pra.Models;

public partial class TemplateStatus
{
    [Key]
    public int TemplateStatusId { get; set; }

    [Required]
    [StringLength(20)]
    public string StatusCode { get; set; }

    [Required]
    [StringLength(50)]
    public string StatusName { get; set; }

    [StringLength(255)]
    public string Description { get; set; }
}
