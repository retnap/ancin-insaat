namespace AncinInsaat.ViewComponents;

public class PartnerLogoItem
{
    public required string Label { get; init; }
}

public class PartnerLogosViewModel
{
    public required IReadOnlyList<PartnerLogoItem> Partners { get; init; }
}
