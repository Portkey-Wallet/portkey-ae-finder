using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetCAHolderTransferLimitDto
{
    [Name("caHash")] public string? CAHash { get; set; }
}