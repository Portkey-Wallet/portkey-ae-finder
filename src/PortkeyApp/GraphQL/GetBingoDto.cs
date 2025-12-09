using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetBingoDto: PagedResultRequestDto
{
    
    [Name("caAddresses")]
    public List<string> CAAddresses { get; set; }
    
    public string PlayId { get; set; }
}
