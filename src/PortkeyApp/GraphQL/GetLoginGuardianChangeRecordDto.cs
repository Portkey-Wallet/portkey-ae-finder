namespace PortkeyApp.GraphQL;

public class GetLoginGuardianChangeRecordDto
{
    public string? ChainId { get; set; }

    public long? StartBlockHeight { get; set; }
    
    public long? EndBlockHeight { get; set; }
}