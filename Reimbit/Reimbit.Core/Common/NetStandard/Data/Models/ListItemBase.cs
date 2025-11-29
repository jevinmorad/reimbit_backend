using Newtonsoft.Json;

namespace Reimbit.Core.Common.NetStandard.Data.Models;

public class ListItemBase
{
    [JsonIgnore]
    public int? TotalRecords { get; set; }
}
