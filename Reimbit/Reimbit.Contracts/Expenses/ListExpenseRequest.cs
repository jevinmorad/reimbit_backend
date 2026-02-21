using Common.Web.Models;

namespace Reimbit.Contracts.Expenses;

public class ListExpenseRequest : PagedRequest
{
    public int? UserID { get; set; }
}
