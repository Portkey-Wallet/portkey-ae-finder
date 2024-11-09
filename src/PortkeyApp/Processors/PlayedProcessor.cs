using AeFinder.Sdk.Processor;
using Google.Protobuf.WellKnownTypes;
using Portkey.Contracts.BingoGameContract;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class PlayedProcessor : CAHolderTransactionProcessorBase<Played>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).BingoGameContractAddress;
    }

    public override async Task ProcessAsync(Played logEvent, LogEventContext context)
    {
        if (logEvent.PlayerAddress == null || logEvent.PlayerAddress.Value == null)
        {
            return;
        }

        // await ProcessCAHolderTransactionAsync(context, eventValue.PlayerAddress.ToBase58());
        if (!IsValidTransaction(context.ChainId, context.Transaction.To, context.Transaction.MethodName,
                context.Transaction.Params)) return;
        var holder =
            await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId,
                logEvent.PlayerAddress.ToBase58()));
        if (holder == null) return;

        var transIndex = new CAHolderTransactionIndex
        {
            Id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId),
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = logEvent.PlayerAddress.ToBase58(),
            TransactionFee = GetTransactionFee(context.Transaction.ExtraProperties),
            TransferInfo = new TransferInfo
            {
                FromAddress = logEvent.PlayerAddress.ToBase58(),
                ToAddress = GetContractAddress(context.ChainId),
                Amount = logEvent.Amount / 100000000,
                FromChainId = context.ChainId,
                ToChainId = context.ChainId,
            },
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };
        transIndex.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);

        await SaveEntityAsync(transIndex);

        var index = await GetEntityAsync<BingoGameIndex>(logEvent.PlayId.ToHex());
        if (index != null)
        {
            return;
        }

        var feeMap = GetTransactionFee(context.Transaction.ExtraProperties);
        List<TransactionFee> feeList;
        if (!feeMap.IsNullOrEmpty())
        {
            feeList = feeMap.Select(pair => new TransactionFee
            {
                Symbol = pair.Key,
                Amount = pair.Value
            }).ToList();
        }
        else
        {
            feeList = new List<TransactionFee>
            {
                new()
                {
                    Symbol = null,
                    Amount = 0
                }
            };
        }

        var bingoIndex = new BingoGameIndex
        {
            Id = logEvent.PlayId.ToHex(),
            PlayBlockHeight = logEvent.PlayBlockHeight,
            Amount = logEvent.Amount,
            IsComplete = false,
            PlayId = context.Transaction.TransactionId,
            BingoType = (int)logEvent.Type,
            Dices = new List<int> { },
            PlayerAddress = logEvent.PlayerAddress.ToBase58(),
            PlayTime = context.Block.BlockTime.ToTimestamp().Seconds,
            PlayTransactionFee = feeList,
            PlayBlockHash = context.Block.BlockHash
        };

        await SaveEntityAsync(bingoIndex);
    }
}