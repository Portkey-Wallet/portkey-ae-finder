using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetCAHolderManagerApprovedDto : PagedResultRequestDto
{
    public string? ChainId { get; set; }
    [Name("caHash")] public string? CAHash { get; set; }
    public string? Spender { get; set; }
    public string? Symbol { get; set; }
    public long? StartHeight { get; set; }
    public long? EndHeight { get; set; }
}