using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server_pra.Models;

/// <summary>
/// טבלת סטטוסי קובץ
/// </summary>
[Table("FileStatus")]
public partial class FileStatus
{
    /// <summary>
    /// מזהה סטטוס קובץ
    /// </summary>
    [Key]
    public int FileStatusId { get; set; }

    /// <summary>
    /// תיאור סטטוס קובץ
    /// </summary>
    [Required]
    [StringLength(100)]
    public string FileStatusDesc { get; set; }
}