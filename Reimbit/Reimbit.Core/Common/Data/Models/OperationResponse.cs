using Newtonsoft.Json;

namespace Common.Data.Models;

public class OperationResponse<TValue>
{
    [JsonProperty(PropertyName = "id")]
    public TValue Id { get; set; }

    [JsonProperty(PropertyName = "rowsAffected")]
    public int RowsAffected { get; set; }
}
