using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogErrorDbm
{
    public int ErrorDbmsid { get; set; }

    public string? Spdetail { get; set; }

    public string? ErrorMessage { get; set; }

    public string? ErrorProcedure { get; set; }

    public int? ErrorSeverity { get; set; }

    public int? ErrorState { get; set; }

    public int? ErrorLine { get; set; }

    public int? ErrorNumber { get; set; }

    public DateTime Created { get; set; }

    public DateTime? VerifiedOn { get; set; }

    public string? VerifiedRemarks { get; set; }

    public bool? IsSolved { get; set; }
}
