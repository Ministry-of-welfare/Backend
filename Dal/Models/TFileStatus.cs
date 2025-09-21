using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Models;

[Table("TFileStatus")]
public partial class TFileStatus
{
    [Key]
    public int FileStatusId { get; set; }

    public string FileStatusDesc { get; set; }
}
