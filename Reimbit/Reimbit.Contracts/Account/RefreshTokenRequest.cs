using Newtonsoft.Json;

namespace Reimbit.Contracts.Account;

public sealed class RefreshTokenRequest
{
    public RefreshTokenRequest() { }

    public string? RefreshToken { get; set; }
    [JsonIgnore]
    public DateTime CurrentDate { get; set; }
}