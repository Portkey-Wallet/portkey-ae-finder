namespace PortkeyApp.GraphQL;

public class GetReferralInfoPageDto : PagedResultRequestDto
{
    public List<string?>? CaHashes { get; set; }
    public string? MethodName { get; set; }
    public List<string?>? ReferralCodes { get; set; }
    public string? ProjectCode { get; set; }
    public long? StartTime { get; set; }
    public long? EndTime { get; set; }
}