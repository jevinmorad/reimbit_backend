namespace Common.Data.Models;

public class PagedResult<TModel>
{
    public int Total { get; set; }
    public IReadOnlyList<TModel> Data { get; set; }
    public PagedResult()
    {
        Total = Total;
        Data = new List<TModel>();
    }
}
