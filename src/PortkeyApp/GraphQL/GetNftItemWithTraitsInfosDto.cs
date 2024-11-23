namespace PortkeyApp.GraphQL;

public class GetNftItemWithTraitsInfosDto : PagedResultRequestDto
{
    public string? Symbol { get; set; }
    public string? CollectionSymbol { get; set; }
}