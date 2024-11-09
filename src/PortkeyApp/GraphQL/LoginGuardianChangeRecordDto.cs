namespace PortkeyApp.GraphQL;

public class LoginGuardianChangeRecordDto : LoginGuardianDtoBase
{
    public string ChangeType { get; set; }

    public long BlockHeight { get; set; }
    public string BlockHash { get; set; }
}