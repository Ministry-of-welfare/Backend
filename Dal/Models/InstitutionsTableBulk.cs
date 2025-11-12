using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class InstitutionsTableBulk
{
    public int Id { get; set; }
    public int? IdentityNumber { get; set; }
    public string FirstName { get; set; }
}
