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

    // Added from scaffold: permissions / roles / users
    public virtual DbSet<TabPermission> TabPermissions { get; set; }

    public virtual DbSet<TabRole> TabRoles { get; set; }

    public virtual DbSet<TabRolePermission> TabRolePermissions { get; set; }

    public virtual DbSet<TabUser> TabUsers { get; set; }

    public virtual DbSet<TabUserRole> TabUserRoles { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<TemplateAuditLog> TemplateAuditLogs { get; set; }

    public virtual DbSet<TemplatePermission> TemplatePermissions { get; set; }

    public virtual DbSet<TemplateStatus> TemplateStatuses { get; set; }
    public virtual DbSet<TabValidationRule> TabValidationRules { get; set; }
    public virtual DbSet<TabValidationRuleCondition> TabValidationRuleConditions { get; set; }
    public virtual DbSet<TabValidationRuleAssert> TabValidationRuleAsserts { get; set; }

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

        // ... existing entity configurations preserved (Services, Templates, etc.) ...
        // Inserted configurations for auth tables (copied from scaffold)

        modelBuilder.Entity<TabPermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__TAB_Perm__EFA6FB2FE588E751");

            entity.ToTable("TAB_Permission", "auth", tb => tb.HasComment("רשימת ההרשאות במערכת (כגון צפייה, עריכה, אישור)"));

            entity.HasIndex(e => e.PermissionName, "UQ__TAB_Perm__0FFDA35731A9B69A").IsUnique();

            entity.Property(e => e.PermissionId).HasComment("מזהה ייחודי להרשאה");
            entity.Property(e => e.ModuleName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("שם המודול במערכת שבו ההרשאה רלוונטית");
            entity.Property(e => e.PermissionDesc)
                .HasMaxLength(250)
                .HasComment("תיאור קצר של הההרשאה");
            entity.Property(e => e.PermissionName)
                .IsRequired()
                .HasMaxLength(150)
                .HasComment("שם ההרשאה (כגון ViewFiles, ApproveImport)");
        });

        modelBuilder.Entity<TabRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__TAB_Role__8AFACE1AFF0EC680");

            entity.ToTable("TAB_Role", "auth", tb => tb.HasComment("טבלת תפקידים במערכת (כגון מנהל, עובד וכו׳)"));

            entity.HasIndex(e => e.RoleName, "UQ__TAB_Role__8A2B6160F21948DB").IsUnique();

            entity.Property(e => e.RoleId).HasComment("מזהה תפקיד ייחודי");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("תאריך יצירת התפקיד")
                .HasColumnType("datetime");
            entity.Property(e => e.IsSystemRole).HasComment("מסמן אם מדובר בתפקיד מערכת (כמו Admin)");
            entity.Property(e => e.RoleDesc)
                .HasMaxLength(250)
                .HasComment("תיאור קצר של התפקיד");
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("שם התפקיד (ייחודי)");
        });

        modelBuilder.Entity<TabRolePermission>(entity =>
        {
            entity.HasKey(e => e.RolePermissionId).HasName("PK__TAB_Role__120F46BACF1C28E6");

            entity.ToTable("TAB_RolePermission", "auth", tb => tb.HasComment("שיוך בין תפקידים להרשאות (קשר N:N)"));

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "UQ_RolePermission").IsUnique();

            entity.Property(e => e.RolePermissionId).HasComment("מזהה ייחודי לשורה");
            entity.Property(e => e.PermissionId).HasComment("מזהה ההרשאה");
            entity.Property(e => e.RoleId).HasComment("מזהה התפקיד");

            entity.HasOne(d => d.Permission).WithMany(p => p.TabRolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Permission");

            entity.HasOne(d => d.Role).WithMany(p => p.TabRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Role");
        });

        modelBuilder.Entity<TabUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__TAB_User__1788CC4C6154A836");

            entity.ToTable("TAB_User", "auth", tb => tb.HasComment("טבלת משתמשים במערכת ההרשאות"));

            entity.HasIndex(e => e.UserName, "UQ__TAB_User__C9F28456125428CD").IsUnique();

            entity.Property(e => e.UserId).HasComment("מזהה משתמש ייחודי");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("תאריך יצירת המשתמש במערכת")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(200)
                .HasComment("שם לתצוגה (מלא או מותאם)");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasComment("כתובת מייל של המשתמש");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasComment("שם פרטי של המשתמש");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("האם המשתמש פעיל (1=כן, 0=לא)");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasComment("שם משפחה של המשתמש");
                    entity.Property(e => e.Password)
            .HasMaxLength(255)
            .HasComment("סיסמת המשתמש");
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("שם משתמש לכניסה למערכת (ייחודי)");
        });

        modelBuilder.Entity<TabUserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__TAB_User__3D978A35A05F1314");

            entity.ToTable("TAB_UserRole", "auth", tb => tb.HasComment("שיוך משתמשים לתפקידים (קשר N:N)"));

            entity.Property(e => e.UserRoleId).HasComment("מזהה ייחודי לשיוך");
            entity.Property(e => e.AssignedBy)
                .HasMaxLength(100)
                .HasComment("שם המשתמש שהקצה את ההרשאה");
            entity.Property(e => e.FromDate).HasComment("תאריך התחלת ההרשאה");
            entity.Property(e => e.RoleId).HasComment("מזהה התפקיד");
            entity.Property(e => e.ToDate).HasComment("תאריך סיום ההרשאה");
            entity.Property(e => e.UserId).HasComment("מזהה המשתמש");

            entity.HasOne(d => d.Role).WithMany(p => p.TabUserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");

            entity.HasOne(d => d.User).WithMany(p => p.TabUserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_User");
        });

        // Configure remaining entities with primary keys
        modelBuilder.Entity<Environment>(entity =>
        {
            entity.HasKey(e => e.EnvironmentId);
            entity.ToTable("Environments");
        });

        modelBuilder.Entity<InstitutionsTableBulk>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("InstitutionsTableBulk");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId);
            entity.ToTable("Services");
        });

        modelBuilder.Entity<ServiceBucket>(entity =>
        {
            entity.HasKey(e => e.ServiceBucketId);
            entity.ToTable("ServiceBuckets");
        });

        modelBuilder.Entity<System>(entity =>
        {
            entity.HasKey(e => e.SystemId);
            entity.ToTable("Systems");
        });

        modelBuilder.Entity<TDataSourceType>(entity =>
        {
            entity.HasKey(e => e.DataSourceTypeId);
            entity.ToTable("T_DataSourceType");
        });

        modelBuilder.Entity<TFileStatus>(entity =>
        {
            entity.HasKey(e => e.FileStatusId);
            entity.ToTable("T_FileStatus");
        });

        modelBuilder.Entity<TImportStatus>(entity =>
        {
            entity.HasKey(e => e.ImportStatusId);
            entity.ToTable("T_ImportStatus");
        });

        modelBuilder.Entity<TabColumnHebDescription>(entity =>
        {
            entity.HasKey(e => e.ColumnHebDescriptionId);
            entity.ToTable("TAB_ColumnHebDescription");
        });

        modelBuilder.Entity<TabFormatColumn>(entity =>
        {
            entity.HasKey(e => e.FormatColumnId);
            entity.ToTable("TAB_FormatColumn");
        });

        modelBuilder.Entity<TabImportDataSource>(entity =>
        {
            entity.HasKey(e => e.ImportDataSourceId);
            entity.ToTable("TAB_ImportDataSource");
        });

        modelBuilder.Entity<TabImportDataSourceColumn>(entity =>
        {
            entity.HasKey(e => e.ImportDataSourceColumnsId);
            entity.ToTable("TAB_ImportDataSourceColumn");
        });

        modelBuilder.Entity<TabImportError>(entity =>
        {
            entity.HasKey(e => e.ImportErrorId);
            entity.ToTable("TAB_ImportError");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.TemplateId);
            entity.ToTable("Templates");
        });

        modelBuilder.Entity<TemplateAuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId);
            entity.ToTable("TemplateAuditLogs");
        });

        modelBuilder.Entity<TemplatePermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId);
            entity.ToTable("TemplatePermissions");
        });

        modelBuilder.Entity<TemplateStatus>(entity =>
        {
            entity.HasKey(e => e.TemplateStatusId);
            entity.ToTable("TemplateStatuses");
        });

        modelBuilder.Entity<TabValidationRule>(entity =>
        {
            entity.HasKey(e => e.ValidationRuleId);
            entity.HasOne(e => e.ImportDataSource)
                  .WithMany(d => d.TabValidationRules)
                  .HasForeignKey(e => e.ImportDataSourceId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ImportError)
                  .WithMany()
                  .HasForeignKey(e => e.ImportErrorId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.ToTable("Tab_ValidationRule");
        });


        modelBuilder.Entity<TabValidationRuleCondition>(entity =>
        {
            entity.HasKey(e => e.RuleConditionId);
            entity.HasOne(e => e.ValidationRule)
                  .WithMany(r => r.Conditions)
                  .HasForeignKey(e => e.ValidationRuleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TabValidationRuleAssert>(entity =>
        {
            entity.HasKey(e => e.RuleAssertId);
            entity.HasOne(e => e.ValidationRule)
                  .WithMany(r => r.Asserts)
                  .HasForeignKey(e => e.ValidationRuleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}