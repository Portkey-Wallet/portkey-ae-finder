using AeFinder.Sdk.Entities;
using AElf.EntityMapping.Elasticsearch.Linq;
using Nest;
using PortkeyApp.Configs;

namespace PortkeyApp.Entities;

public class CAHolderIndex :  AeFinderEntity, IAeFinderEntity
{
    [Keyword]public override string Id { get; set; }
    
    /// <summary>
    /// CA holder hash(Id)
    /// </summary>
    [Keyword]public string CAHash { get; set; }
    
    /// <summary>
    /// CA holder address
    /// </summary>
    [Keyword]public string CAAddress { get; set; }
    
    /// <summary>
    /// CA holder creator address
    /// </summary>
    [Keyword]public string Creator { get; set; }
    
    /// <summary>
    /// CA Holder manager address list
    /// </summary>
    [Nested(Name = "ManagerInfos",Enabled = true,IncludeInParent = true,IncludeInRoot = true)]
    public List<ManagerInfo> ManagerInfos { get; set; }
    
    [Nested(Name = "ManagerInfosNew",Enabled = true,IncludeInParent = true,IncludeInRoot = true)]
    public List<ManagerInfo> ManagerInfosNew { get; set; }
    
    [Nested(Name = "Guardians",Enabled = true,IncludeInParent = true,IncludeInRoot = true)]
    public List<Guardian> Guardians { get; set; }
    
    /// <summary>
    /// ChainId where CAHolder created
    /// </summary>
    [Keyword]public string OriginChainId { get; set; }
    
    
    public  List<ManagerInfo> GetManagerInfos(string chainId, long blockHeight)
    {
        var contractInfo = ConfigConstants.ContractInfos.FirstOrDefault(t => t.ChainId == chainId);
        if (contractInfo == null)
        {
            return ManagerInfos;
        }

        return blockHeight > contractInfo.ResetManagerInfoHeight ? ManagerInfosNew : ManagerInfos;
    }
    
    public void SetManagerInfos(List<ManagerInfo> managerInfos, string chainId, long blockHeight)
    {
        var contractInfo = ConfigConstants.ContractInfos.FirstOrDefault(t => t.ChainId == chainId);
        if (contractInfo == null || blockHeight <= contractInfo.ResetManagerInfoHeight)
        {
            ManagerInfos = managerInfos;
        }
        else
        {
            ManagerInfosNew = managerInfos;
        }
    }
}

[NestedAttributes("ManagerInfos")]
public class ManagerInfo
{
    [Keyword]public string Address { get; set; }
    
    [Keyword]public string ExtraData { get; set; }
}

