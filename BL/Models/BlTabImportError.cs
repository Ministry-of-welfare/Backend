namespace BL.Models
{
    public class BlTabImportError
    {
        public int ImportErrorId { get; set; }
        public int ImportDataSourceId { get; set; }
        public string ImportErrorDesc { get; set; } = string.Empty;
        public string? ImportErrorDescEng { get; set; }
    }
}
