using Newtonsoft.Json;

namespace Common.Data.Models;

public class ListItemBase
{
    [JsonIgnore]
    public int? TotalRecords { get; set; }
}
