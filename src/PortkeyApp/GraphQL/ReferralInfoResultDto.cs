namespace PortkeyApp.GraphQL;

public class ReferralInfoResultDto
{
    public long TotalRecordCount { get; set; }
    public List<ReferralInfoDto> Data { get; set; }
}