using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class ManagerUpdatedProcessor: CAHolderManagerProcessorBase<ManagerInfoUpdated>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(ManagerInfoUpdated logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        //check ca address if already exist in caHolderIndex
        var indexId = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58());
        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(indexId);
        if (caHolderIndex == null) return;

        var managerInfo = caHolderIndex.ManagerInfos.FirstOrDefault(m => m.Address == logEvent.Manager.ToBase58());
        if (managerInfo == null) return;

        managerInfo.ExtraData = logEvent.ExtraData;

        await SaveEntityAsync(caHolderIndex);
        await AddChangeRecordAsync(logEvent.CaAddress.ToBase58(), logEvent.CaHash.ToHex(),
            logEvent.Manager.ToBase58(), nameof(ManagerInfoUpdated), context);
    }
    
    protected override async Task HandlerTransactionIndexAsync(ManagerInfoUpdated eventValue, LogEventContext context)
    {
        await ProcessCAHolderTransactionAsync(context, eventValue.CaAddress.ToBase58(), eventValue.Platform);
    }
}