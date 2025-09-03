namespace BL.Models
{
    public class BlTemplatePermission
    {
        public int PermissionId { get; set; }
        public string PrincipalName { get; set; } = string.Empty;
        public string PrincipalType { get; set; } = string.Empty;
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDuplicate { get; set; }
    }
}
