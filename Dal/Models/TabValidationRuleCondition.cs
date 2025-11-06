using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Models
{
    [Table("Tab_ValidationRuleCondition")]  
    public class TabValidationRuleCondition
    {
        public int RuleConditionId { get; set; }
        public int ValidationRuleId { get; set; }
        public int GroupNo { get; set; } = 1;
        public string GroupOperator { get; set; } = "AND";
        public string LeftRefType { get; set; } = string.Empty;
        public string? LeftColumnName { get; set; }
        public string? LeftConstValue { get; set; }
        public string? LeftFunc { get; set; }
        public string Operator { get; set; } = string.Empty;
        public string? RightRefType { get; set; }
        public string? RightColumnName { get; set; }
        public string? RightConstValue { get; set; }
        public string? RightFunc { get; set; }

        public virtual TabValidationRule ValidationRule { get; set; } = null!;
    }
}
