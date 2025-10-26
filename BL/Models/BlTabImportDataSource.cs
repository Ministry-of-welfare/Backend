namespace BL.Models
{
    /// <summary>
    /// ����� ������� �� ���� ������.
    /// </summary>
    public class BlTabImportDataSource
    {
        public int ImportDataSourceId { get; set; } // ���� ���� �����
        public string ImportDataSourceDesc { get; set; } = string.Empty; // ����� ���� �����
        public int DataSourceTypeId { get; set; } // ��� ���� �����
        public int? FileStatusId { get; set; }
        public int? SystemId { get; set; } // ���� �����
        public string? JobName { get; set; } // �� �����
        public string? TableName { get; set; } // �� ����
        public string UrlFile { get; set; } = string.Empty; // ���� ����
        public string UrlFileAfterProcess { get; set; } = string.Empty; // ���� ���� ���� �����
        public DateTime? EndDate { get; set; } // ����� ����
        public string? ErrorRecipients { get; set; } // ������ �������
        public DateTime InsertDate { get; set; } // ����� �����
        public DateTime? StartDate { get; set; } // ����� �����

    }

    /// <summary>
    /// ����� ������� �� ������ ������ ���� ���� ������.
    /// </summary>
    public class BlTabImportDataSourceForQuery
    {
        public int ImportControlId { get; set; } // ���� �����
        public string ImportDataSourceDesc { get; set; } = string.Empty; // ����� ���� �����
        public string SystemName { get; set; } = string.Empty; // �� �����
        public string FileName { get; set; } = string.Empty; // �� ����
        public DateTime ImportStartDate { get; set; } // ����� ����� �����
        public DateTime ImportFinishDate { get; set; } // ����� ���� �����
        public int TotalRows { get; set; } // �� �� ������ �����
        public int TotalRowsAffected { get; set; } // �� ������ ������
        public int RowsInvalid { get; set; } // �� ������ �������
        public string ImportStatusDesc { get; set; } = string.Empty;// ���� ����� �����
        public string UrlFileAfterProcess { get; set; } = string.Empty; // ���� ���� ���� �����
        public string ErrorReportPath { get; set; } = string.Empty; // ���� ���� ������
    }

}
