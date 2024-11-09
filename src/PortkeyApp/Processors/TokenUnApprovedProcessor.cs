using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TokenUnApprovedProcessor: CAHolderTransactionProcessorBase<UnApproved>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress;
    }

    public override async Task ProcessAsync(UnApproved logEvent, LogEventContext context)
    {
        if (logEvent.Symbol.Equals("*") || (logEvent.Symbol.Contains("-") && !logEvent.Symbol.Contains("-*")))
        {
            return;
        }
        var holder = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, logEvent.Owner.ToBase58()));
        if (holder == null) return;
        
        var batchApprovedIndexId = IdGenerateHelper.GetId(context.ChainId, logEvent.Owner.ToBase58(), logEvent.Spender.ToBase58(), logEvent.Symbol);
        var batchApprovedIndex = await GetEntityAsync<CAHolderTokenApprovedIndex>(batchApprovedIndexId);
        if (batchApprovedIndex == null)
            return;
        
        batchApprovedIndex.BatchApprovedAmount = Math.Max(0, batchApprovedIndex.BatchApprovedAmount - logEvent.Amount);
        await SaveEntityAsync(batchApprovedIndex);
    }
}