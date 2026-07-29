namespace AncinInsaat.ViewComponents;

public class PartnerLogoItem
{
    public required string ImageSrc { get; init; }
    public required string ImageAlt { get; init; }
}

public class PartnerLogosViewModel
{
    public required IReadOnlyList<PartnerLogoItem> Partners { get; init; }
}
