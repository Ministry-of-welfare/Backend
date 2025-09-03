using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [InverseProperty("TemplateStatus")]
    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
