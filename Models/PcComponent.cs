namespace Cwiczenia4Api.Models;

public class PcComponent
{
    public int PCId { get; set; }
    public string ComponentCode { get; set; } = null!;
    public int Amount { get; set; }

    public Pc Pc { get; set; } = null!;
    public Component Component { get; set; } = null!;
}