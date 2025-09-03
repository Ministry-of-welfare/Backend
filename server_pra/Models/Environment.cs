using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [InverseProperty("Environment")]
    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
