using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AppImportControl> AppImportControls { get; set; }

    public virtual DbSet<AppImportProblem> AppImportProblems { get; set; }

    public virtual DbSet<Environment> Environments { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceBucket> ServiceBuckets { get; set; }

    public virtual DbSet<System> Systems { get; set; }

    public virtual DbSet<TDataSourceType> TDataSourceTypes { get; set; }

    public virtual DbSet<TImportStatus> TImportStatuses { get; set; }

    public virtual DbSet<TabColumnHebDescription> TabColumnHebDescriptions { get; set; }

    public virtual DbSet<TabFormatColumn> TabFormatColumns { get; set; }

    public virtual DbSet<TabImportDataSource> TabImportDataSources { get; set; }

    public virtual DbSet<TabImportDataSourceColumn> TabImportDataSourceColumns { get; set; }

    public virtual DbSet<TabImportError> TabImportErrors { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<TemplateAuditLog> TemplateAuditLogs { get; set; }

    public virtual DbSet<TemplatePermission> TemplatePermissions { get; set; }

    public virtual DbSet<TemplateStatus> TemplateStatuses { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Ministry-of-welfare.mdf;Integrated Security=True;Connect Timeout=30");
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("name=DefaultConnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppImportControl>(entity =>
        {
            entity.HasKey(e => e.ImportControlId).HasName("PK__APP_Impo__33E3CFB89E616C88");

            entity.ToTable("APP_ImportControl", tb => tb.HasComment("טבלת בקרה על תהליך הקליטה"));

            entity.Property(e => e.ImportControlId).HasComment("מס’ רץ קליטה");
            entity.Property(e => e.EmailSento).HasComment("נמעני מייל");
            entity.Property(e => e.ErrorReportPath).HasComment("נתיב קובץ האקסל של השגיאות שנשלח");
            entity.Property(e => e.FileName).HasComment("שם קובץ שנקלט בפועל");
            entity.Property(e => e.ImportDataSourceId).HasComment("מס’ רץ סוג קליטה (TAB_ImportDataSource)");
            entity.Property(e => e.ImportFinishDate).HasComment("תאריך סיום קליטה");
            entity.Property(e => e.ImportFromDate).HasComment("מתאריך — ממתי הקליטה בתוקף");
            entity.Property(e => e.ImportStartDate).HasComment("תאריך ושעה התחלת קליטה");
            entity.Property(e => e.ImportStatusId).HasComment("סטטוס קליטה (FK → T_ImportStatus)");
            entity.Property(e => e.ImportToDate).HasComment("עד תאריך — עד מתי הקליטה בתוקף");
            entity.Property(e => e.RowsInvalid).HasComment("מספר שורות לא תקינות");
            entity.Property(e => e.TotalRows).HasComment("סך רשומות שהגיעו בקובץ");
            entity.Property(e => e.TotalRowsAffected).HasComment("סך רשומות שנקלטו/עודכנו");
            entity.Property(e => e.UrlFileAfterProcess).HasComment("כתובת (URL/נתיב) לקובץ לאחר עיבוד");

            entity.HasOne(d => d.ImportStatus).WithMany(p => p.AppImportControls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportControl_Status");
        });

        modelBuilder.Entity<AppImportProblem>(entity =>
        {
            entity.HasKey(e => e.ImportProblemId).HasName("PK__APP_Impo__4D624EC67FA67BD8");

            entity.ToTable("APP_ImportProblem", tb => tb.HasComment("טבלת בעיות/שגיאות בקליטה"));

            entity.Property(e => e.ImportProblemId).HasComment("מס’ רץ טבלת שגיאות");
            entity.Property(e => e.ErrorColumn).HasComment("העמודה בה קיימת השגיאה");
            entity.Property(e => e.ErrorDetail).HasComment("מלל חופשי של השגיאה");
            entity.Property(e => e.ErrorRow).HasComment("מספר השורה עם השגיאה");
            entity.Property(e => e.ErrorTableId).HasComment("ID רשומת היעד");
            entity.Property(e => e.ErrorValue).HasComment("הערך עבורו התקבלה השגיאה");
            entity.Property(e => e.ImportControlId).HasComment("מס’ רץ קליטה (FK → APP_ImportControl)");
            entity.Property(e => e.ImportErrorId).HasComment("מס’ רץ שגיאה (FK → TAB_ImportError)");

            entity.HasOne(d => d.ImportControl).WithMany(p => p.AppImportProblems).HasConstraintName("FK_ImportProblem_Control");
        });

        modelBuilder.Entity<Environment>(entity =>
        {
            entity.HasKey(e => e.EnvironmentId).HasName("PK__Environm__4B909A4913B864C9");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00A5C2FEEB7");

            entity.ToTable(tb => tb.HasComment("טבלת השירותים (כגון PDF, MAIL)"));

            entity.Property(e => e.ServiceId).HasComment("מזהה שירות (1, 2)");
            entity.Property(e => e.Description).HasComment("תיאור השירות");
            entity.Property(e => e.ServiceName).HasComment("שם השירות (PDF, MAIL)");
        });

        modelBuilder.Entity<ServiceBucket>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__ServiceB__C51BB00A7759AF9D");

            entity.ToTable("ServiceBucket", tb => tb.HasComment("טבלת ServiceBucket – מכילה את ה-Buckets ב-S3 המשויכים לשירותים"));

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasComment("מזהה שירות (קישור לטבלת Services)");
            entity.Property(e => e.BucketName).HasComment("שם ה-S3 Bucket המשויך לשירות (לדוגמה: pdf-templates-bucket)");
            entity.Property(e => e.BucketRegion).HasComment("אזור (Region) של ה-Bucket (לדוגמה: us-east-1)");
            entity.Property(e => e.Description).HasComment("תיאור שימוש או הערות");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("האם ה-Bucket פעיל (1 = מותר, 0 = אסור)");

            entity.HasOne(d => d.Service).WithOne(p => p.ServiceBucket).HasConstraintName("FK_ServiceBucket");
        });

        modelBuilder.Entity<System>(entity =>
        {
            entity.HasKey(e => e.SystemId).HasName("PK__Systems__9394F68AD603D80F");

            entity.ToTable(tb => tb.HasComment("טבלת המערכות"));

            entity.Property(e => e.SystemId).HasComment("מזהה מערכת");
            entity.Property(e => e.OwnerEmail).HasComment("אימייל של בעל המערכת");
            entity.Property(e => e.SystemCode).HasComment("קוד מערכת (HRM, WELFARE, PAYMENTS)");
            entity.Property(e => e.SystemName).HasComment("שם תצוגה של המערכת");
        });

        modelBuilder.Entity<TDataSourceType>(entity =>
        {
            entity.HasKey(e => e.DataSourceTypeId).HasName("PK__T_DataSo__CE60140A2ED5101D");

            entity.ToTable("T_DataSourceType", tb => tb.HasComment("טבלת סוגי מקור הקלטה"));

            entity.Property(e => e.DataSourceTypeId)
                .ValueGeneratedNever()
                .HasComment("קוד מקור");
            entity.Property(e => e.DataSourceTypeDesc).HasComment("תיאור מקור");
        });

        modelBuilder.Entity<TImportStatus>(entity =>
        {
            entity.HasKey(e => e.ImportStatusId).HasName("PK__T_Import__8A27BFA5E48DC02B");

            entity.ToTable("T_ImportStatus", tb => tb.HasComment("טבלת סטטוסי קליטה"));

            entity.Property(e => e.ImportStatusId)
                .ValueGeneratedNever()
                .HasComment("קוד סטטוס קליטה");
            entity.Property(e => e.ImportStatusDesc).HasComment("תיאור סטטוס קליטה");
        });

        modelBuilder.Entity<TabColumnHebDescription>(entity =>
        {
            entity.HasKey(e => e.ColumnHebDescriptionId).HasName("PK__TAB_Colu__5EB7C3ED5B2C1C06");

            entity.ToTable("TAB_ColumnHebDescription", tb => tb.HasComment("תיאור עמודות בעברית עבור קבצים"));

            entity.Property(e => e.ColumnHebDescriptionId).HasComment("מזהה שדה");
            entity.Property(e => e.ColumnDescription).HasComment("תאור שדה");
            entity.Property(e => e.ColumnName).HasComment("שם שדה");
            entity.Property(e => e.ImportDataSourceId).HasComment("מזהה קובץ (FK → TAB_ImportDataSource)");
        });

        modelBuilder.Entity<TabFormatColumn>(entity =>
        {
            entity.HasKey(e => e.FormatColumnId).HasName("PK__TAB_Form__DD7453F60045C156");

            entity.ToTable("TAB_FormatColumn", tb => tb.HasComment("טבלת פורמטי עמודות"));

            entity.Property(e => e.FormatColumnId)
                .ValueGeneratedNever()
                .HasComment("קוד פורמט");
            entity.Property(e => e.FormatColumnDesc).HasComment("תיאור פורמט");
        });

        modelBuilder.Entity<TabImportDataSource>(entity =>
        {
            entity.HasKey(e => e.ImportDataSourceId).HasName("PK__TAB_Impo__CADAC94BE38A1F31");

            entity.ToTable("TAB_ImportDataSource", tb => tb.HasComment("טבלת סוגי קליטה"));

            entity.Property(e => e.ImportDataSourceId).HasComment("מס’ רץ סוג קליטה");
            entity.Property(e => e.DataSourceTypeId).HasComment("סוג קליטה (מקושר ל-T_DataSourceType)");
            entity.Property(e => e.EndDate).HasComment("תאריך סיום של תהליך שכבר לא פעיל");
            entity.Property(e => e.ErrorRecipients).HasComment("מייל לשגיאות, אפשרי מספר נמענים");
            entity.Property(e => e.ImportDataSourceDesc).HasComment("תיאור סוג קליטה");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("תאריך הכנסת השורה");
            entity.Property(e => e.JobName).HasComment("שם הג'וב שמפעיל את טבלת ImportControl");
            entity.Property(e => e.StartDate).HasComment("תאריך תחילת ההרצה");
            entity.Property(e => e.SystemId).HasComment("קוד מערכת (קשור ל־T_SystemType)");
            entity.Property(e => e.TableName).HasComment("שם טבלה שמקושרת לתהליך");
            entity.Property(e => e.UrlFile).HasComment("נתיב בו קיימים הקבצים");
            entity.Property(e => e.UrlFileAfterProcess).HasComment("נתיב להעברת הקבצים בעת קליטה (מתועד ב־ImportControl)");

            entity.HasOne(d => d.DataSourceType).WithMany(p => p.TabImportDataSources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportDataSource_DataSourceType");
        });

        modelBuilder.Entity<TabImportDataSourceColumn>(entity =>
        {
            entity.HasKey(e => e.ImportDataSourceColumnsId).HasName("PK__TAB_Impo__391898F61A8BE47D");

            entity.ToTable("TAB_ImportDataSourceColumns", tb => tb.HasComment("טבלת עמודות לכל סוג קובץ"));

            entity.Property(e => e.ImportDataSourceColumnsId)
                .ValueGeneratedNever()
                .HasComment("קוד עמודה");
            entity.Property(e => e.ColumnName).HasComment("שם עמודה");
            entity.Property(e => e.ColumnNameHebDescription).HasComment("שם עמודה בעברית עבור קובץ השגיאות");
            entity.Property(e => e.FormatColumnId).HasComment("פורמט שדה (FK → TAB_FormatColumn)");
            entity.Property(e => e.ImportDataSourceId).HasComment("קוד קובץ (FK → TAB_ImportDataSource)");
            entity.Property(e => e.OrderId).HasComment("סידורי");

            entity.HasOne(d => d.FormatColumn).WithMany(p => p.TabImportDataSourceColumns).HasConstraintName("FK_ImportDataSourceColumns_FormatColumn");
        });

        modelBuilder.Entity<TabImportError>(entity =>
        {
            entity.HasKey(e => e.ImportErrorId).HasName("PK__TAB_Impo__D443F6A295997ECD");

            entity.ToTable("TAB_ImportError", tb => tb.HasComment("טבלת שגיאות קליטה"));

            entity.Property(e => e.ImportErrorId)
                .ValueGeneratedNever()
                .HasComment("מס’ רץ שגיאה");
            entity.Property(e => e.ImportDataSourceId).HasComment("קוד קליטה מקושר (FK → TAB_ImportDataSource)");
            entity.Property(e => e.ImportErrorDesc).HasComment("תיאור שגיאה");
            entity.Property(e => e.ImportErrorDescEng).HasComment("תיאור שגיאה באנגלית");

            entity.HasOne(d => d.ImportDataSource).WithMany(p => p.TabImportErrors).HasConstraintName("FK_ImportError_DataSource");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.TemplateId).HasName("PK__Template__F87ADD2744A6845E");

            entity.ToTable(tb => tb.HasComment("טבלת תבניות (Templates) הכוללת פרטי תבניות למיילים/קבצי PDF"));

            entity.Property(e => e.TemplateId)
                .ValueGeneratedNever()
                .HasComment("מזהה תבנית");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("תאריך יצירה (ברירת מחדל GETDATE())");
            entity.Property(e => e.CreatedBy).HasComment("שם משתמש יוצר");
            entity.Property(e => e.EnvironmentId).HasComment("מזהה סביבה (קישור ל- Environments)");
            entity.Property(e => e.IsActive).HasComment("האם פעיל (1 = פעיל, 0 = לא פעיל)");
            entity.Property(e => e.S3key).HasComment("נתיב מלא בקובץ S3 (bucket-name/path/to/template.html)");
            entity.Property(e => e.ServiceId).HasComment("מזהה שירות (PDF או MAIL, קישור ל- Services)");
            entity.Property(e => e.SystemId).HasComment("מזהה מערכת (קישור ל- Systems)");
            entity.Property(e => e.TemplateName).HasComment("שם תבנית");
            entity.Property(e => e.TemplateStatusId).HasComment("מזהה סטטוס (DRAFT, ACTIVE, DEPRECATED, DELETED, קישור ל- TemplateStatuses)");
            entity.Property(e => e.UpdatedAt).HasComment("תאריך עדכון אחרון");
            entity.Property(e => e.UpdatedBy).HasComment("משתמש שעידכן אחרון");
            entity.Property(e => e.ValidFrom).HasComment("תוקף התחלה");
            entity.Property(e => e.ValidTo).HasComment("תוקף סיום");

            entity.HasOne(d => d.Environment).WithMany(p => p.Templates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Templates_Environments");

            entity.HasOne(d => d.Service).WithMany(p => p.Templates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Templates_Services");

            entity.HasOne(d => d.System).WithMany(p => p.Templates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Templates_Systems");

            entity.HasOne(d => d.TemplateStatus).WithMany(p => p.Templates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Templates_TemplateStatuses");
        });

        modelBuilder.Entity<TemplateAuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Template__5E54864842A941D2");

            entity.ToTable("TemplateAuditLog", tb => tb.HasComment("טבלת רישומי Audit עבור תבניות (TemplateAuditLog)"));

            entity.Property(e => e.LogId).HasComment("מזהה רישום");
            entity.Property(e => e.Action).HasComment("סוג פעולה (View, Edit, Delete, Duplicate, PermissionChange)");
            entity.Property(e => e.ActionAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("תאריך ביצוע הפעולה");
            entity.Property(e => e.ActionBy).HasComment("המשתמש שביצע פעולה (user@domain.gov.il)");
            entity.Property(e => e.NewValue).HasComment("ערך חדש (JSON)");
            entity.Property(e => e.OldValue).HasComment("ערך קודם (JSON)");
            entity.Property(e => e.TemplateId).HasComment("מזהה תבנית (קישור ל- Templates)");

            entity.HasOne(d => d.Template).WithMany(p => p.TemplateAuditLogs).HasConstraintName("FK_TemplateAuditLog_Template");
        });

        modelBuilder.Entity<TemplatePermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Template__EFA6FB2FB7B24E99");

            entity.ToTable(tb => tb.HasComment("טבלת הרשאות עבור תבניות (TemplatePermissions)"));

            entity.Property(e => e.PermissionId).HasComment("מזהה הרשאה");
            entity.Property(e => e.CanDelete).HasComment("הרשאת מחיקה (1 = מותר, 0 = אסור)");
            entity.Property(e => e.CanDuplicate).HasComment("הרשאת שכפול (1 = מותר, 0 = אסור)");
            entity.Property(e => e.CanEdit).HasComment("הרשאת עריכה (1 = מותר, 0 = אסור)");
            entity.Property(e => e.CanView).HasComment("הרשאת צפייה (1 = מותר, 0 = אסור)");
            entity.Property(e => e.PrincipalName).HasComment("שם משתמש או קבוצה (user@domain.gov.il או Developers)");
            entity.Property(e => e.PrincipalType).HasComment("סוג ישות (User או Group)");
            entity.Property(e => e.TemplateId).HasComment("מזהה תבנית (קישור לטבלת Templates)");

            entity.HasOne(d => d.Template).WithMany(p => p.TemplatePermissions).HasConstraintName("FK_TemplatePermissions_Template");
        });

        modelBuilder.Entity<TemplateStatus>(entity =>
        {
            entity.HasKey(e => e.TemplateStatusId).HasName("PK__Template__B255A4CFA76FFF8D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
