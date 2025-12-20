using System;
using System.Collections.Generic;

namespace Reimbit.Domain.Models;

public partial class LogSecUserAuth
{
    public int LoguserAuthId { get; set; }

    public string Iud { get; set; } = null!;

    public DateTime IuddateTime { get; set; }

    public int IudbyUserId { get; set; }

    public int? UserId { get; set; }

    public int? OrganizationId { get; set; }

    public string? PasswordHash { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public DateTime? LastLogin { get; set; }
}
