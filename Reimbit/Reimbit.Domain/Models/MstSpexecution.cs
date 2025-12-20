using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class MstSpexecution
{
    public int SpexecutionId { get; set; }

    public string Spname { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Remarks { get; set; }

    public int? ExecutionTimeMs { get; set; }

    public int? UserId { get; set; }
}
