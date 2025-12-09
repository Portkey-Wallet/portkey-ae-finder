namespace PortkeyApp.GraphQL;

public class GetAutoReceiveTransactionDto : PagedResultRequestDto
{
    public List<string?>? TransferTransactionIds { get; set; }
}