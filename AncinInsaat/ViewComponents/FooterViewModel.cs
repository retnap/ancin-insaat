namespace AncinInsaat.ViewComponents;

public class FooterViewModel
{
    public required string AddressLine1 { get; init; }
    public required string AddressLine2 { get; init; }
    public required string Phone { get; init; }
    public required string PhoneHref { get; init; }
    public required string Email { get; init; }
    public int CopyrightYear { get; init; }
}
