using Newtonsoft.Json;

namespace PortkeyApp.Configs;

public static class ConfigConstants
{
    static ConfigConstants()
    {
        PortkeyConfig = GetPortkeyConfig();
        SetConfigs();
    }

    private static void SetConfigs()
    {
       
        CAHolderTransactionInfos = PortkeyConfig.GetCAHolderTransactionInfos();
        ContractInfos = PortkeyConfig.GetContractInfos();
        InitialInfo = PortkeyConfig.GetInitialInfo();
        Inscriptions = PortkeyConfig.GetInscriptions();
    }

    public static List<CAHolderTransactionInfo> CAHolderTransactionInfos { get; set; } 
    public static List<ContractInfo> ContractInfos { get; set; }
    public static InitialInfo InitialInfo { get; set; }
    public static List<string> Inscriptions { get; set; }

    public static IPortkeyConfig PortkeyConfig { get; set; }
    private static IPortkeyConfig GetPortkeyConfig() => new PortkeyConfig();

}