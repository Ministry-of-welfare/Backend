using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppImportControl> AppImportControls { get; set; }

    public virtual DbSet<AppImportProblem> AppImportProblems { get; set; }

    public virtual DbSet<Environment> Environments { get; set; }

    public virtual DbSet<InstitutionsTableBulk> InstitutionsTableBulks { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceBucket> ServiceBuckets { get; set; }

    public virtual DbSet<System> Systems { get; set; }

    public virtual DbSet<TDataSourceType> TDataSourceTypes { get; set; }

    public virtual DbSet<TFileStatus> TFileStatuses { get; set; }

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
            entity.HasKey(e => e.ImportControlId).HasName("PK__APP_Impo__33E3CFB82A3F0C28");

            entity.ToTable("APP_ImportControl", tb => tb.HasComment("טבלת בקרה על תהליך הקליטה"));

            entity.HasIndex(e => new { e.ImportStartDate, e.ImportFinishDate }, "IX_APP_ImportControl_Dates");

            entity.HasIndex(e => e.ImportDataSourceId, "IX_APP_ImportControl_ImportDataSourceId");

            entity.HasIndex(e => e.ImportStatusId, "IX_APP_ImportControl_Status");

            entity.Property(e => e.ImportControlId).HasComment("מס’ רץ קליטה");
            entity.Property(e => e.EmailSento)
                .HasMaxLength(1000)
                .HasComment("נמעני מייל");
            entity.Property(e => e.ErrorReportPath)
                .HasMaxLength(400)
                .HasComment("נתיב קובץ האקסל של השגיאות שנשלח");
            entity.Property(e => e.FileName)
                .HasMaxLength(260)
                .HasComment("שם קובץ שנקלט בפועל");
            entity.Property(e => e.ImportDataSourceId).HasComment("מס’ רץ סוג קליטה (TAB_ImportDataSource)");
            entity.Property(e => e.ImportFinishDate)
                .HasComment("תאריך סיום קליטה")
                .HasColumnType("datetime");
            entity.Property(e => e.ImportFromDate)
                .HasComment("מתאריך — ממתי הקליטה בתוקף")
                .HasColumnType("datetime");
            entity.Property(e => e.ImportStartDate)
                .HasComment("תאריך ושעה התחלת קליטה")
                .HasColumnType("datetime");
            entity.Property(e => e.ImportStatusId).HasComment("סטטוס קליטה (FK → T_ImportStatus)");
            entity.Property(e => e.ImportToDate)
                .HasComment("עד תאריך — עד מתי הקליטה בתוקף")
                .HasColumnType("datetime");
            entity.Property(e => e.RowsInvalid).HasComment("מספר שורות לא תקינות");
            entity.Property(e => e.TotalRows).HasComment("סך רשומות שהגיעו בקובץ");
            entity.Property(e => e.TotalRowsAffected).HasComment("סך רשומות שנקלטו/עודכנו");
            entity.Property(e => e.UrlFileAfterProcess)
                .IsRequired()
                .HasComment("כתובת (URL/נתיב) לקובץ לאחר עיבוד");

            entity.HasOne(d => d.ImportDataSource).WithMany(p => p.AppImportControls)
                .HasForeignKey(d => d.ImportDataSourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportControl_DataSource");

            entity.HasOne(d => d.ImportStatus).WithMany(p => p.AppImportControls)
                .HasForeignKey(d => d.ImportStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportControl_Status");
        });

        modelBuilder.Entity<AppImportProblem>(entity =>
        {
            entity.HasKey(e => e.ImportProblemId).HasName("PK__APP_Impo__4D624EC6C73BDD7F");

            entity.ToTable("APP_ImportProblem", tb => tb.HasComment("טבלת בעיות/שגיאות בקליטה"));

            entity.HasIndex(e => e.ImportControlId, "IX_APP_ImportProblem_ImportControl");

            entity.HasIndex(e => e.ImportErrorId, "IX_APP_ImportProblem_ImportError");

            entity.Property(e => e.ImportProblemId).HasComment("מס’ רץ טבלת שגיאות");
            entity.Property(e => e.ErrorColumn).HasComment("העמודה בה קיימת השגיאה");
            entity.Property(e => e.ErrorDetail).HasComment("מלל חופשי של השגיאה");
            entity.Property(e => e.ErrorRow).HasComment("מספר השורה עם השגיאה");
            entity.Property(e => e.ErrorTableId).HasComment("ID רשומת היעד");
            entity.Property(e => e.ErrorValue).HasComment("הערך עבורו התקבלה השגיאה");
            entity.Property(e => e.ImportControlId).HasComment("מס’ רץ קליטה (FK → APP_ImportControl)");
            entity.Property(e => e.ImportErrorId).HasComment("מס’ רץ שגיאה (FK → TAB_ImportError)");

            entity.HasOne(d => d.ImportControl).WithMany(p => p.AppImportProblems)
                .HasForeignKey(d => d.ImportControlId)
                .HasConstraintName("FK_ImportProblem_Control");

            entity.HasOne(d => d.ImportError).WithMany(p => p.AppImportProblems)
                .HasForeignKey(d => d.ImportErrorId)
                .HasConstraintName("FK_ImportProblem_Error");
        });

        modelBuilder.Entity<Environment>(entity =>
        {
            entity.HasKey(e => e.EnvironmentId).HasName("PK__Environm__4B909A490A5C7B9E");

            entity.ToTable(tb => tb.HasComment("טבלת הסביבות (פיתוח, בדיקות, ייצור וכו׳)"));

            entity.HasIndex(e => e.EnvironmentCode, "UX_Environments_Code").IsUnique();

            entity.Property(e => e.EnvironmentId).HasComment("מזהה סביבה (1, 2, 3...)");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasComment("תיאור חופשי על הסביבה");
            entity.Property(e => e.EnvironmentCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("קוד סביבה (DEV, TEST, PREPROD, PROD)");
            entity.Property(e => e.EnvironmentName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("שם תצוגה של הסביבה (סביבת פיתוח, ייצור וכו׳)");
        });

        modelBuilder.Entity<InstitutionsTableBulk>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("InstitutionsTable_BULK");

            entity.Property(e => e.FirstName).HasMaxLength(200);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00A6868AF07");

            entity.ToTable(tb => tb.HasComment("טבלת השירותים (כגון PDF, MAIL)"));

            entity.HasIndex(e => e.ServiceName, "UX_Services_Name").IsUnique();

            entity.Property(e => e.ServiceId).HasComment("מזהה שירות (1, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasComment("תיאור השירות");
            entity.Property(e => e.ServiceName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("שם השירות (PDF, MAIL)");
        });

        modelBuilder.Entity<ServiceBucket>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__ServiceB__C51BB00A963182B7");

            entity.ToTable("ServiceBucket", tb => tb.HasComment("טבלת ServiceBucket – מכילה את ה-Buckets ב-S3 המשויכים לשירותים"));

            entity.HasIndex(e => e.BucketName, "IX_ServiceBucket_BucketName");

            entity.HasIndex(e => e.IsActive, "IX_ServiceBucket_IsActive");

            entity.HasIndex(e => e.ServiceId, "IX_ServiceBucket_ServiceId");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasComment("מזהה שירות (קישור לטבלת Services)");
            entity.Property(e => e.BucketName)
                .IsRequired()
                .HasMaxLength(255)
                .HasComment("שם ה-S3 Bucket המשויך לשירות (לדוגמה: pdf-templates-bucket)");
            entity.Property(e => e.BucketRegion)
                .HasMaxLength(50)
                .HasComment("אזור (Region) של ה-Bucket (לדוגמה: us-east-1)");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasComment("תיאור שימוש או הערות");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("האם ה-Bucket פעיל (1 = מותר, 0 = אסור)");

            entity.HasOne(d => d.Service).WithOne(p => p.ServiceBucket)
                .HasForeignKey<ServiceBucket>(d => d.ServiceId)
                .HasConstraintName("FK_ServiceBucket");
        });

        modelBuilder.Entity<System>(entity =>
        {
            entity.HasKey(e => e.SystemId).HasName("PK__Systems__9394F68AFAFA6039");

            entity.ToTable(tb => tb.HasComment("טבלת המערכות"));

            entity.HasIndex(e => e.SystemCode, "UX_Systems_Code").IsUnique();

            entity.Property(e => e.SystemId).HasComment("מזהה מערכת");
            entity.Property(e => e.OwnerEmail)
                .HasMaxLength(100)
                .HasComment("אימייל של בעל המערכת");
            entity.Property(e => e.SystemCode)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("קוד מערכת (HRM, WELFARE, PAYMENTS)");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(255)
                .HasComment("שם תצוגה של המערכת");
        });

        modelBuilder.Entity<TDataSourceType>(entity =>
        {
            entity.HasKey(e => e.DataSourceTypeId).HasName("PK__T_DataSo__CE60140AB7909B5A");

            entity.ToTable("T_DataSourceType", tb => tb.HasComment("טבלת סוגי מקור הקלטה"));

            entity.Property(e => e.DataSourceTypeId)
                .ValueGeneratedNever()
                .HasComment("קוד מקור");
            entity.Property(e => e.DataSourceTypeDesc)
                .HasMaxLength(400)
                .HasComment("תיאור מקור");
        });

        modelBuilder.Entity<TFileStatus>(entity =>
        {
            entity.HasKey(e => e.FileStatusId).HasName("PK__T_FileSt__26A5E153287F9C5F");

            entity.ToTable("T_FileStatus", tb => tb.HasComment("טבלת סטטוס קבצים"));

            entity.HasIndex(e => e.FileStatusDesc, "IX_T_FileStatus_Desc");

            entity.Property(e => e.FileStatusId)
                .ValueGeneratedNever()
                .HasComment("קוד סטטוס (1=פעיל, 2=לא פעיל, 3=בהקמה)");
            entity.Property(e => e.FileStatusDesc)
                .HasMaxLength(400)
                .HasComment("תיאור סטטוס");
        });

        modelBuilder.Entity<TImportStatus>(entity =>
        {
            entity.HasKey(e => e.ImportStatusId).HasName("PK__T_Import__8A27BFA53C5BFC1B");

            entity.ToTable("T_ImportStatus", tb => tb.HasComment("טבלת סטטוסי קליטה"));

            entity.Property(e => e.ImportStatusId)
                .ValueGeneratedNever()
                .HasComment("קוד סטטוס קליטה");
            entity.Property(e => e.ImportStatusDesc)
                .HasMaxLength(400)
                .HasComment("תיאור סטטוס קליטה");
        });

        modelBuilder.Entity<TabColumnHebDescription>(entity =>
        {
            entity.HasKey(e => e.ColumnHebDescriptionId).HasName("PK__TAB_Colu__5EB7C3ED28402BE2");

            entity.ToTable("TAB_ColumnHebDescription", tb => tb.HasComment("תיאור עמודות בעברית עבור קבצים"));

            entity.HasIndex(e => e.ImportDataSourceId, "IX_TAB_ColumnHebDescription_DataSource");

            entity.Property(e => e.ColumnHebDescriptionId).HasComment("מזהה שדה");
            entity.Property(e => e.ColumnDescription)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("תאור שדה");
            entity.Property(e => e.ColumnName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("שם שדה");
            entity.Property(e => e.ImportDataSourceId).HasComment("מזהה קובץ (FK → TAB_ImportDataSource)");

            entity.HasOne(d => d.ImportDataSource).WithMany(p => p.TabColumnHebDescriptions)
                .HasForeignKey(d => d.ImportDataSourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ColumnHeb_DataSource");
        });

        modelBuilder.Entity<TabFormatColumn>(entity =>
        {
            entity.HasKey(e => e.FormatColumnId).HasName("PK__TAB_Form__DD7453F6C34506A5");

            entity.ToTable("TAB_FormatColumn", tb => tb.HasComment("טבלת פורמטי עמודות"));

            entity.Property(e => e.FormatColumnId)
                .ValueGeneratedNever()
                .HasComment("קוד פורמט");
            entity.Property(e => e.FormatColumnDesc)
                .HasMaxLength(20)
                .HasComment("תיאור פורמט");
        });

        modelBuilder.Entity<TabImportDataSource>(entity =>
        {
            entity.HasKey(e => e.ImportDataSourceId).HasName("PK__tmp_ms_x__CADAC94BE445B65A");

            entity.ToTable("TAB_ImportDataSource", tb => tb.HasComment("טבלת סוגי קליטה"));

            entity.HasIndex(e => e.ImportDataSourceDesc, "IX_TAB_ImportDataSource_Desc");

            entity.HasIndex(e => e.SystemId, "IX_TAB_ImportDataSource_System");

            entity.HasIndex(e => e.DataSourceTypeId, "IX_TAB_ImportDataSource_Type");

            entity.Property(e => e.ImportDataSourceId).HasComment("מס’ רץ סוג קליטה");
            entity.Property(e => e.DataSourceTypeId).HasComment("סוג קליטה (מקושר ל-T_DataSourceType)");
            entity.Property(e => e.EndDate)
                .HasComment("תאריך סיום של תהליך שכבר לא פעיל")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorRecipients).HasComment("מייל לשגיאות, אפשרי מספר נמענים");
            entity.Property(e => e.ImportDataSourceDesc)
                .HasMaxLength(200)
                .HasComment("תיאור סוג קליטה");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("תאריך הכנסת השורה")
                .HasColumnType("datetime");
            entity.Property(e => e.JobName)
                .HasMaxLength(400)
                .HasComment("שם הג'וב שמפעיל את טבלת ImportControl");
            entity.Property(e => e.StartDate)
                .HasComment("תאריך תחילת ההרצה")
                .HasColumnType("datetime");
            entity.Property(e => e.SystemId).HasComment("קוד מערכת (קשור ל־T_SystemType)");
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .HasComment("שם טבלה שמקושרת לתהליך");
            entity.Property(e => e.UrlFile)
                .IsRequired()
                .HasComment("נתיב בו קיימים הקבצים");
            entity.Property(e => e.UrlFileAfterProcess)
                .IsRequired()
                .HasComment("נתיב להעברת הקבצים בעת קליטה (מתועד ב־ImportControl)");

            entity.HasOne(d => d.DataSourceType).WithMany(p => p.TabImportDataSources)
                .HasForeignKey(d => d.DataSourceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportDataSource_DataSourceType");

            entity.HasOne(d => d.FileStatus).WithMany(p => p.TabImportDataSources)
                .HasForeignKey(d => d.FileStatusId)
                .HasConstraintName("FK_ImportDataSource_FileStatus");
            entity.HasOne(e => e.System)
                 .WithMany()
                 .HasForeignKey(e => e.SystemId)
                 .OnDelete(DeleteBehavior.ClientSetNull); // הגדרת הקשר
        });

        modelBuilder.Entity<TabImportDataSourceColumn>(entity =>
        {
            entity.HasKey(e => e.ImportDataSourceColumnsId).HasName("PK__TAB_Impo__391898F6D0BDD74F");

            entity.ToTable("TAB_ImportDataSourceColumns", tb => tb.HasComment("טבלת עמודות לכל סוג קובץ"));

            entity.HasIndex(e => e.ColumnName, "IX_TAB_ImportDataSourceColumns_ColumnName");

            entity.HasIndex(e => e.ImportDataSourceId, "IX_TAB_ImportDataSourceColumns_DataSource");

            entity.HasIndex(e => e.FormatColumnId, "IX_TAB_ImportDataSourceColumns_FormatColumn");

            entity.Property(e => e.ImportDataSourceColumnsId)
                .ValueGeneratedNever()
                .HasComment("קוד עמודה");
            entity.Property(e => e.ColumnName)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("שם עמודה");
            entity.Property(e => e.ColumnNameHebDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("שם עמודה בעברית עבור קובץ השגיאות");
            entity.Property(e => e.FormatColumnId).HasComment("פורמט שדה (FK → TAB_FormatColumn)");
            entity.Property(e => e.ImportDataSourceId).HasComment("קוד קובץ (FK → TAB_ImportDataSource)");
            entity.Property(e => e.OrderId).HasComment("סידורי");

            entity.HasOne(d => d.FormatColumn).WithMany(p => p.TabImportDataSourceColumns)
                .HasForeignKey(d => d.FormatColumnId)
                .HasConstraintName("FK_ImportDataSourceColumns_FormatColumn");

            entity.HasOne(d => d.ImportDataSource).WithMany(p => p.TabImportDataSourceColumns)
                .HasForeignKey(d => d.ImportDataSourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportDataSourceColumns_DataSource");
        });

        modelBuilder.Entity<TabImportError>(entity =>
        {
            entity.HasKey(e => e.ImportErrorId).HasName("PK__TAB_Impo__D443F6A218E696E9");

            entity.ToTable("TAB_ImportError", tb => tb.HasComment("טבלת שגיאות קליטה"));

            entity.HasIndex(e => e.ImportDataSourceId, "IX_TAB_ImportError_DataSource");

            entity.Property(e => e.ImportErrorId)
                .ValueGeneratedNever()
                .HasComment("מס’ רץ שגיאה");
            entity.Property(e => e.ImportDataSourceId).HasComment("קוד קליטה מקושר (FK → TAB_ImportDataSource)");
            entity.Property(e => e.ImportErrorDesc)
                .HasMaxLength(400)
                .HasComment("תיאור שגיאה");
            entity.Property(e => e.ImportErrorDescEng).HasComment("תיאור שגיאה באנגלית");

            entity.HasOne(d => d.ImportDataSource).WithMany(p => p.TabImportErrors)
                .HasForeignKey(d => d.ImportDataSourceId)
                .HasConstraintName("FK_ImportError_DataSource");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.TemplateId).HasName("PK__Template__F87ADD27751CC64E");

            entity.ToTable(tb => tb.HasComment("טבלת תבניות (Templates) הכוללת פרטי תבניות למיילים/קבצי PDF"));

            entity.HasIndex(e => e.EnvironmentId, "IX_Templates_EnvironmentId");

            entity.HasIndex(e => e.ServiceId, "IX_Templates_ServiceId");

            entity.HasIndex(e => e.TemplateStatusId, "IX_Templates_StatusId");

            entity.HasIndex(e => e.SystemId, "IX_Templates_SystemId");

            entity.HasIndex(e => new { e.TemplateName, e.EnvironmentId }, "IX_Templates_TemplateName_Env");

            entity.Property(e => e.TemplateId)
                .ValueGeneratedNever()
                .HasComment("מזהה תבנית");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("תאריך יצירה (ברירת מחדל GETDATE())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("שם משתמש יוצר");
            entity.Property(e => e.EnvironmentId).HasComment("מזהה סביבה (קישור ל- Environments)");
            entity.Property(e => e.IsActive).HasComment("האם פעיל (1 = פעיל, 0 = לא פעיל)");
            entity.Property(e => e.S3key)
                .IsRequired()
                .HasMaxLength(1024)
                .HasComment("נתיב מלא בקובץ S3 (bucket-name/path/to/template.html)")
                .HasColumnName("S3Key");
            entity.Property(e => e.ServiceId).HasComment("מזהה שירות (PDF או MAIL, קישור ל- Services)");
            entity.Property(e => e.SystemId).HasComment("מזהה מערכת (קישור ל- Systems)");
            entity.Property(e => e.TemplateName)
                .IsRequired()
                .HasMaxLength(255)
                .HasComment("שם תבנית");
            entity.Property(e => e.TemplateStatusId).HasComment("מזהה סטטוס (DRAFT, ACTIVE, DEPRECATED, DELETED, קישור ל- TemplateStatuses)");
            entity.Property(e => e.UpdatedAt)
                .HasComment("תאריך עדכון אחרון")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .HasComment("משתמש שעידכן אחרון");
            entity.Property(e => e.ValidFrom).HasComment("תוקף התחלה");
            entity.Property(e => e.ValidTo).HasComment("תוקף סיום");

            entity.HasOne(d => d.Environment).WithMany(p => p.Templates)
                .HasForeignKey(d => d.EnvironmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Templates_Environments");

            entity.HasOne(d => d.Service).WithMany(p => p.Templates)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Templates_Services");

            entity.HasOne(d => d.System).WithMany(p => p.Templates)
                .HasForeignKey(d => d.SystemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Templates_Systems");

            entity.HasOne(d => d.TemplateStatus).WithMany(p => p.Templates)
                .HasForeignKey(d => d.TemplateStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Templates_TemplateStatuses");
        });

        modelBuilder.Entity<TemplateAuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Template__5E5486485B015ADE");

            entity.ToTable("TemplateAuditLog", tb => tb.HasComment("טבלת רישומי Audit עבור תבניות (TemplateAuditLog)"));

            entity.HasIndex(e => e.ActionAt, "IX_TemplateAuditLog_ActionAt");

            entity.HasIndex(e => e.TemplateId, "IX_TemplateAuditLog_TemplateId");

            entity.Property(e => e.LogId).HasComment("מזהה רישום");
            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("סוג פעולה (View, Edit, Delete, Duplicate, PermissionChange)");
            entity.Property(e => e.ActionAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("תאריך ביצוע הפעולה")
                .HasColumnType("datetime");
            entity.Property(e => e.ActionBy)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("המשתמש שביצע פעולה (user@domain.gov.il)");
            entity.Property(e => e.NewValue).HasComment("ערך חדש (JSON)");
            entity.Property(e => e.OldValue).HasComment("ערך קודם (JSON)");
            entity.Property(e => e.TemplateId).HasComment("מזהה תבנית (קישור ל- Templates)");

            entity.HasOne(d => d.Template).WithMany(p => p.TemplateAuditLogs)
                .HasForeignKey(d => d.TemplateId)
                .HasConstraintName("FK_TemplateAuditLog_Template");
        });

        modelBuilder.Entity<TemplatePermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Template__EFA6FB2F2AE63566");

            entity.ToTable(tb => tb.HasComment("טבלת הרשאות עבור תבניות (TemplatePermissions)"));

            entity.HasIndex(e => e.PrincipalName, "IDX_TemplatePermissions_PrincipalName");

            entity.HasIndex(e => e.TemplateId, "IDX_TemplatePermissions_TemplateId");

            entity.HasIndex(e => e.PermissionId, "UX_TemplatePermissions_PermissionId").IsUnique();

            entity.Property(e => e.PermissionId).HasComment("מזהה הרשאה");
            entity.Property(e => e.CanDelete).HasComment("הרשאת מחיקה (1 = מותר, 0 = אסור)");
            entity.Property(e => e.CanDuplicate).HasComment("הרשאת שכפול (1 = מותר, 0 = אסור)");
            entity.Property(e => e.CanEdit).HasComment("הרשאת עריכה (1 = מותר, 0 = אסור)");
            entity.Property(e => e.CanView).HasComment("הרשאת צפייה (1 = מותר, 0 = אסור)");
            entity.Property(e => e.PrincipalName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("שם משתמש או קבוצה (user@domain.gov.il או Developers)");
            entity.Property(e => e.PrincipalType)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("סוג ישות (User או Group)");
            entity.Property(e => e.TemplateId).HasComment("מזהה תבנית (קישור לטבלת Templates)");

            entity.HasOne(d => d.Template).WithMany(p => p.TemplatePermissions)
                .HasForeignKey(d => d.TemplateId)
                .HasConstraintName("FK_TemplatePermissions_Template");
        });

        modelBuilder.Entity<TemplateStatus>(entity =>
        {
            entity.HasKey(e => e.TemplateStatusId).HasName("PK__Template__B255A4CF9D094A9A");

            entity.ToTable(tb => tb.HasComment("טבלת סטטוסים של תבניות (טיוטה, פעיל, לא בשימוש)"));

            entity.HasIndex(e => e.StatusCode, "UX_TemplateStatuses_Code").IsUnique();

            entity.Property(e => e.TemplateStatusId).HasComment("מזהה סטטוס (1, 2, 3)");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasComment("תיאור חופשי על הסטטוס");
            entity.Property(e => e.StatusCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("קוד סטטוס (DRAFT, ACTIVE, DEPRECATED)");
            entity.Property(e => e.StatusName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("שם סטטוס לתצוגה (טיוטה, פעיל, לא בשימוש)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
