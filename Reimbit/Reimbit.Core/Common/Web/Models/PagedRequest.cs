using Newtonsoft.Json;

namespace Common.Web.Models;

public class PagedRequest
{
    private int pageOffset;

    private int pageSize;

    private const int _minPageSize = 10;

    private const int _maxPageSize = 5000;

    private string sortOrder;

    private string[] sortOrders = new string[2] { "asc", "desc" };

    public int PageOffset
    {
        get
        {
            if (pageOffset < 0)
            {
                return 0;
            }

            return pageOffset * ((pageSize >= _minPageSize && pageSize <= _maxPageSize) ? pageSize : _minPageSize);
        }
        set
        {
            pageOffset = value;
        }
    }

    public int PageSize
    {
        get
        {
            if (pageSize == -1)
            {
                return int.MaxValue;
            }

            if (pageSize < _minPageSize || pageSize > _maxPageSize)
            {
                return 10;
            }

            return pageSize;
        }
        set
        {
            pageSize = value;
        }
    }

    [JsonProperty("sortField")]
    public string? SortField { get; set; }

    [JsonProperty("sortOrder")]
    public string? SortOrder { get; set; }
}
