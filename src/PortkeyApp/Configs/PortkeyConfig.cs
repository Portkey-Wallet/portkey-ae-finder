using Newtonsoft.Json;

namespace PortkeyApp.Configs;

public class PortkeyConfig : IPortkeyConfig
{
    private PortkeyConfigEntity PortkeyConfigEntity { get; set; } = GetPortkeyConfig();

    private static PortkeyConfigEntity GetPortkeyConfig()
    {
        var json = GetConfigurationJson(NetWork.TestNet); // modify network
        var configEntity = JsonConvert.DeserializeObject<PortkeyConfigEntity>(json);
        return configEntity;
    }

    public List<CAHolderTransactionInfo> GetCAHolderTransactionInfos() => PortkeyConfigEntity.CAHolderTransactionInfos;

    public List<ContractInfo> GetContractInfos() => PortkeyConfigEntity.ContractInfos;

    public InitialInfo GetInitialInfo() => PortkeyConfigEntity.InitialInfo;
    public List<string> GetInscriptions() => PortkeyConfigEntity.Inscriptions;

    private static string GetConfigurationJson(NetWork netWork) => netWork == NetWork.MainNet
        ? ConfigFile.MainNetConfigurationJson
        : ConfigFile.TestNetConfigurationJson;
}

public enum NetWork
{
    MainNet,
    TestNet
}

public class PortkeyConfigEntity
{
    public List<CAHolderTransactionInfo> CAHolderTransactionInfos { get; set; }
    public List<ContractInfo> ContractInfos { get; set; }
    public InitialInfo InitialInfo { get; set; }
    public List<string> Inscriptions { get; set; }
}