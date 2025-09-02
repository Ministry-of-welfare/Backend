using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Models
{
    [Table("Environments")] // �� ����� ���� �������
    public class EnvironmentEntity
    {
        [Key]
        public int EnvironmentId { get; set; }

        [Required, StringLength(20)]
        public string EnvironmentCode { get; set; }

        [Required, StringLength(50)]
        public string EnvironmentName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
