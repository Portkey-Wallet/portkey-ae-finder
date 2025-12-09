namespace PortkeyApp.GraphQL;

public class CAHolderTokenBalancePageResultDto
{
    public long TotalRecordCount { get; set; }
    
    public List<CAHolderTokenBalanceDto> Data { get; set; }
}