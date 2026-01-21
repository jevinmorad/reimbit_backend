using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Account;

public sealed class LogoutRequest
{
    public required string RefreshToken { get; set; }
    [JsonIgnore]
    public DateTime CurrentDate { get; set; }
}