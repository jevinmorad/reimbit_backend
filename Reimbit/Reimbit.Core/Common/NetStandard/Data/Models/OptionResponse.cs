namespace Reimbit.Core.Common.NetStandard.Data.Models;

public class OptionResponse<TValue, TLabel>
{
    public TValue Value { get; set; }
    public TLabel Lable { get; set; }
}

public class OptionResponse : OptionResponse<int, string>
{ }
