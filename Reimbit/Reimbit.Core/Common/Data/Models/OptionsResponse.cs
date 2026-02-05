namespace Common.Data.Models;

public class OptionsResponse<TValue>
{
    public TValue Value { get; set; }
    public string Label { get; set; }
}