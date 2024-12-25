using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class ManagerSocialRecoveredProcessor : CAHolderTransactionProcessorBase<ManagerInfoSocialRecovered>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(ManagerInfoSocialRecovered logEvent, LogEventContext context)
    {
        BreakHelper.CheckBreak(context.ChainId, context.Block.BlockHeight);
        await HandlerTransactionIndexAsync(logEvent, context);
        //check manager is already exist in caHolderManagerIndex
        var managerIndexId = IdGenerateHelper.GetId(context.ChainId, logEvent.Manager.ToBase58());
        var caHolderManagerIndex = await GetEntityAsync<CAHolderManagerIndex>(managerIndexId);

        if (caHolderManagerIndex == null)
        {
            caHolderManagerIndex = new CAHolderManagerIndex
            {
                Id = managerIndexId,
                Manager = logEvent.Manager.ToBase58(),
                CAAddresses = new List<string>()
                {
                    logEvent.CaAddress.ToBase58()
                }
            };
        }
        else
        {
            if (!caHolderManagerIndex.CAAddresses.Contains(logEvent.CaAddress.ToBase58()))
            {
                caHolderManagerIndex.CAAddresses.Add(logEvent.CaAddress.ToBase58());
            }
        }

        await SaveEntityAsync(caHolderManagerIndex);

        //check ca address if already exist in caHolderIndex
        var indexId = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58());
        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(indexId);
        if (caHolderIndex == null)
        {
            return;
        }

        if (caHolderIndex.ManagerInfos.Count(m => m.Address == logEvent.Manager.ToBase58()) == 0)
        {
            caHolderIndex.ManagerInfos.Add(new Entities.ManagerInfo()
            {
                Address = logEvent.Manager.ToBase58(),
                ExtraData = logEvent.ExtraData
            });
        }

        await SaveEntityAsync(caHolderIndex);
    }

    protected override async Task HandlerTransactionIndexAsync(ManagerInfoSocialRecovered eventValue,
        LogEventContext context)
    {
        await ProcessCAHolderTransactionAsync(context, eventValue.CaAddress.ToBase58(), eventValue.Platform);
    }
}