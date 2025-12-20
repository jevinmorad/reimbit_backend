using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class SecOperation
{
    public int OperationId { get; set; }

    public int OperationNo { get; set; }

    public string OperationName { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? Description { get; set; }
}
