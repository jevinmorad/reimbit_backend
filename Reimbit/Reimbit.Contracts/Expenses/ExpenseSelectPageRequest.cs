using Common.Web.Models;

namespace Reimbit.Contracts.Expenses;

public class ExpenseSelectPageRequest : PagedRequest
{
    public int? UserID { get; set; }
    public string? Title { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
