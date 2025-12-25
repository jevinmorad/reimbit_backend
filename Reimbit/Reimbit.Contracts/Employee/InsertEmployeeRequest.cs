using AegisInt.Core;
using System.Text.Json.Serialization;

namespace Reimbit.Contracts.Employee;

public class InsertEmployeeRequest
{
    [JsonIgnore]
    public EncryptedInt? UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string MobileNo { get; set; }
    public required int RoleId { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }
    [JsonIgnore]
    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public int ModifiedByUserId { get; set; }
    [JsonIgnore]
    public DateTime Created { get; set; }
    [JsonIgnore]
    public DateTime Modified { get; set; }
    [JsonIgnore]
    public bool IsActive { get; set; }
}