using Newtonsoft.Json;

namespace Reimbit.Contracts.Account;

public sealed class RefreshTokenRequest
{
    public required string RefreshToken { get; set; }
    [JsonIgnore]
    public DateTime CurrentDate { get; set; }
}