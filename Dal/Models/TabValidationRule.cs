using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Models
{
    [Table("Tab_ValidationRule")]

    public class TabValidationRule
    {
        public int ValidationRuleId { get; set; } // מזהה כלל
        public int ImportDataSourceId { get; set; } // מזהה מקור קליטה
        public string RuleName { get; set; } = string.Empty; // שם כלל תיאורי
        public byte RuleTypeId { get; set; } // סוג בדיקה: 1=טכנית, 2=לוגית
        public bool IsEnabled { get; set; } = true; // האם הכלל פעיל
        public int ImportErrorId { get; set; } // קוד שגיאה לדיווח
        public string? ErrorColumnName { get; set; } // שם עמודה לדוח השגיאות
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // זמן יצירה
        public string? CreatedBy { get; set; } // שם המשתמש שיצר את הכלל

        // Navigation properties
        public virtual TabImportDataSource ImportDataSource { get; set; } = null!;
        public virtual TabImportError ImportError { get; set; } = null!;
        public virtual ICollection<TabValidationRuleCondition> Conditions { get; set; } = new List<TabValidationRuleCondition>();
        public virtual ICollection<TabValidationRuleAssert> Asserts { get; set; } = new List<TabValidationRuleAssert>();
    }
}
