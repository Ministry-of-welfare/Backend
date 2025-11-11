using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server_pra.Dal.Models.ScaffoldEntities
{
    [Table("TAB_Permission", Schema = "auth")]
    public partial class TabPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

        [Required]
        [MaxLength(150)]
        public string PermissionName { get; set; }

        [MaxLength(250)]
        public string PermissionDesc { get; set; }

        [Required]
        [MaxLength(100)]
        public string ModuleName { get; set; }

        public virtual ICollection<TabRolePermission> TabRolePermissions { get; set; } = new List<TabRolePermission>();
    }
}
