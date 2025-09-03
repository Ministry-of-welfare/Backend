namespace Dal.Models
{
    //[Table("Environments")] // שם הטבלה במסד הנתונים
    public class DalEnvironment
    {
        //[Key]
        //public int EnvironmentId { get; set; }

        //[Required, StringLength(20)]
        //public string EnvironmentCode { get; set; }

        //[Required, StringLength(50)]
        //public string EnvironmentName { get; set; }

        //[StringLength(255)]
        //public string? Description { get; set; }



        public int EnvironmentId { get; set; }
        public string EnvironmentCode { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = string.Empty;
        public string? Description { get; set; } = null;
    }
}
