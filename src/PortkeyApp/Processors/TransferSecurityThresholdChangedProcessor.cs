using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TransferSecurityThresholdChangedProcessor : LogEventProcessorBase<TransferSecurityThresholdChanged>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(TransferSecurityThresholdChanged logEvent, LogEventContext context)
    {
        var indexId =
            IdGenerateHelper.GetId(context.ChainId, logEvent.Symbol, nameof(TransferSecurityThresholdChanged));
        var  index = new TransferSecurityThresholdIndex
        {
            Id = indexId,
            Symbol = logEvent.Symbol,
            BalanceThreshold = logEvent.BalanceThreshold,
            GuardianThreshold = logEvent.GuardianThreshold
        };
        
        await SaveEntityAsync(index);
    }
}