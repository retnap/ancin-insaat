namespace AncinInsaat.Models;

public class ImageBlockModel
{
    public required string Src { get; init; }
    public required string Alt { get; init; }
    public string? CssClass { get; init; }
}
