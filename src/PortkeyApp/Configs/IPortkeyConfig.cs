namespace PortkeyApp.Configs;

public interface IPortkeyConfig
{
    List<CAHolderTransactionInfo> GetCAHolderTransactionInfos();
    List<ContractInfo> GetContractInfos();
    InitialInfo GetInitialInfo();
    List<string> GetInscriptions();
}