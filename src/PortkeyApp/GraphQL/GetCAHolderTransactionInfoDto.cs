using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetCAHolderTransactionInfoDto : PagedResultRequestDto
{
    public string ChainId { get; set; }

    public long StartBlockHeight { get; set; }

    public long EndBlockHeight { get; set; }

    public string Symbol { get; set; }

    [Name("caAddresses")] public List<string?>? CAAddresses { get; set; }

    public string TransactionId { get; set; }

    public string BlockHash { get; set; }

    public string TransferTransactionId { get; set; }

    public List<string> MethodNames { get; set; } = new();
}