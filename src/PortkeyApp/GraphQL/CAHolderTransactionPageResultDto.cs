namespace PortkeyApp.GraphQL;

public class CAHolderTransactionPageResultDto
{
    public long TotalRecordCount { get; set; }
    
    public List<CAHolderTransactionDto> Data { get; set; }
}