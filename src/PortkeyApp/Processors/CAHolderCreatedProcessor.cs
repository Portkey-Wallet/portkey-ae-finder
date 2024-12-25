using AeFinder.Sdk.Processor;
using Google.Protobuf.WellKnownTypes;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class CAHolderCreatedProcessor : CAHolderTransactionProcessorBase<CAHolderCreated>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(CAHolderCreated logEvent, LogEventContext context)
    {
        BreakHelper.CheckBreak(context.ChainId, context.Block.BlockHeight);
        await HandlerTransactionIndexAsync(logEvent, context);
        //check manager is already exist in caHolderManagerIndex
        var managerIndexId = IdGenerateHelper.GetId(context.ChainId, logEvent.Manager.ToBase58());
        var caHolderManagerIndex =
            await GetEntityAsync<CAHolderManagerIndex>(managerIndexId);
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
        if (caHolderIndex != null)
        {
            return;
        }

        caHolderIndex = new CAHolderIndex
        {
            Id = indexId,
            CAHash = logEvent.CaHash.ToHex(),
            CAAddress = logEvent.CaAddress.ToBase58(),
            Creator = logEvent.Creator.ToBase58(),
            ManagerInfos = new List<Entities.ManagerInfo>
            {
                new()
                {
                    Address = logEvent.Manager.ToBase58(),
                    ExtraData = logEvent.ExtraData
                }
            },
            Guardians = new List<Entities.Guardian>(),
            OriginChainId = context.ChainId
        };

        await SaveEntityAsync(caHolderIndex);
    }

    protected override async Task HandlerTransactionIndexAsync(CAHolderCreated logEvent, LogEventContext context)
    {
        if (!IsValidTransaction(context.ChainId, context.Transaction.To, context.Transaction.MethodName,
                context.Transaction.Params)) return;

        var index = new CAHolderTransactionIndex
        {
            Id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId),
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = logEvent.CaAddress.ToBase58(),
            TransactionFee = GetTransactionFee(context.Transaction.ExtraProperties),
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status,
            Platform = logEvent.Platform
        };

        index.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        await SaveEntityAsync(index);
    }
}