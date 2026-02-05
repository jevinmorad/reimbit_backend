using AegisInt.Core;
using Common.Web.Models;

namespace Reimbit.Contracts.Expenses;

public class ListExpenseRequest : PagedRequest
{
    public EncryptedInt? UserID { get; set; }
}
