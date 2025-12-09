namespace PortkeyApp.GraphQL;

public class GetNFTProtocolInfoDto : PagedResultRequestDto
{
    public string Symbol { get; set; }
    public string ChainId { get; set; }
    
}