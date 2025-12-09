using AeFinder.Sdk.Processor;
using AElf.Standards.ACS0;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;
using Volo.Abp.ObjectMapping;

namespace PortkeyApp.Processors;

public class ContractDeployedProcessor: LogEventProcessorBase<ContractDeployed>
{
    private readonly IObjectMapper _objectMapper;
    public ContractDeployedProcessor(IObjectMapper objectMapper)
    {
        _objectMapper = objectMapper;
    }
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).GenesisContractAddress;
    }

    public override async Task ProcessAsync(ContractDeployed logEvent, LogEventContext context)
    {
        if (logEvent.Address.ToBase58() != ConfigConstants.ContractInfos.First(c => c.ChainId == context.ChainId)
                .CAContractAddress) return;
        var nftProtocolInfoList =
            ConfigConstants.InitialInfo.NFTProtocolInfoList.Where(n => n.ChainId == context.ChainId).ToList();
        
        foreach (var nftProtocolInfo in nftProtocolInfoList)
        {
            var nftProtocolInfoIndex = _objectMapper.Map<NFTProtocolInitInfo, NFTCollectionInfoIndex>(nftProtocolInfo);
            nftProtocolInfoIndex.Id = IdGenerateHelper.GetId(nftProtocolInfo.ChainId, nftProtocolInfo.Symbol);
            await SaveEntityAsync(nftProtocolInfoIndex);
        }

        var tokenInfoList = ConfigConstants.InitialInfo.TokenInfoList.Where(n => n.ChainId == context.ChainId).ToList();
        foreach (var tokenInfo in tokenInfoList)
        {
            var tokenInfoIndex = _objectMapper.Map<TokenInitInfo, TokenInfoIndex>(tokenInfo);
            tokenInfoIndex.Id = IdGenerateHelper.GetId(tokenInfo.ChainId, tokenInfo.Symbol);
            await SaveEntityAsync(tokenInfoIndex);
        }
    }
}