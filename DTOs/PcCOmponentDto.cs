namespace Cwiczenia4Api.DTOs;

public class PcComponentDto
{
    public int Amount { get; set; }
    public ComponentDto Component { get; set; } = null!;
}