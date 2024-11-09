using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class ManagerRemovedProcessor : CAHolderManagerProcessorBase<ManagerInfoRemoved>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(ManagerInfoRemoved logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        //check manager is already exist in caHolderManagerIndex
        var managerIndexId = IdGenerateHelper.GetId(context.ChainId, logEvent.Manager.ToBase58());
        var caHolderManagerIndex = await GetEntityAsync<CAHolderManagerIndex>(managerIndexId);
        if (caHolderManagerIndex != null)
        {
            if (caHolderManagerIndex.CAAddresses.Contains(logEvent.CaAddress.ToBase58()))
            {
                caHolderManagerIndex.CAAddresses.Remove(logEvent.CaAddress.ToBase58());
            }

            if (caHolderManagerIndex.CAAddresses.Count == 0)
            {
                await DeleteEntityAsync(caHolderManagerIndex);
            }
            else
            {
                await SaveEntityAsync(caHolderManagerIndex);
            }
        }

        //check ca address if already exist in caHolderIndex
        var indexId = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58());
        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(indexId);
        if (caHolderIndex == null)
        {
            return;
        }

        var item = caHolderIndex.ManagerInfos.FirstOrDefault(m => m.Address == logEvent.Manager.ToBase58());
        if (item != null)
        {
            caHolderIndex.ManagerInfos.Remove(item);
        }

        await SaveEntityAsync(caHolderIndex);
        await AddChangeRecordAsync(logEvent.CaAddress.ToBase58(), logEvent.CaHash.ToHex(),
            logEvent.Manager.ToBase58(), nameof(ManagerInfoRemoved), context);
    }

    protected override async Task HandlerTransactionIndexAsync(ManagerInfoRemoved eventValue, LogEventContext context)
    {
        await ProcessCAHolderTransactionAsync(context, eventValue.CaAddress.ToBase58(), eventValue.Platform);
    }
}