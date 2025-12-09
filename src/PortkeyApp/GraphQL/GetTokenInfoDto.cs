namespace PortkeyApp.GraphQL;

public class GetTokenInfoDto : PagedResultRequestDto
{
    public string? Symbol { get; set; }
    public string? ChainId { get; set; }
    public string? SymbolKeyword { get; set; }
}