using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Models
{
    [Table("Tab_ValidationRuleAssert")]
    public class TabValidationRuleAssert
    {
        public int RuleAssertId { get; set; } // מזהה אסרט
        public int ValidationRuleId { get; set; } // מזהה כלל ראשי
        public int GroupNo { get; set; } = 1; // קבוצה (AND/OR)
        public string GroupOperator { get; set; } = "AND"; // אופרטור בתוך קבוצה
        public string LeftRefType { get; set; } = string.Empty; // סוג אופננד שמאלי
        public string? LeftColumnName { get; set; } // עמודה שמאלית
        public string? LeftConstValue { get; set; } // קבוע שמאלי
        public string? LeftFunc { get; set; } // פונקציה שמאלית
        public string Operator { get; set; } = string.Empty; // אופרטור
        public string? RightRefType { get; set; } // סוג אופננד ימני
        public string? RightColumnName { get; set; } // עמודה ימנית
        public string? RightConstValue { get; set; } // קבוע ימני
        public string? RightFunc { get; set; } // פונקציה ימנית
        public string? LookupTable { get; set; } // טבלת קודקס להשוואה
        public string? LookupColumn { get; set; } // עמודת קודקס להשוואה

        // Navigation properties
        public virtual TabValidationRule ValidationRule { get; set; } = null!;
    }
}
