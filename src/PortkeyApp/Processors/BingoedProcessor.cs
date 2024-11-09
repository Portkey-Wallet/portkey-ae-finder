using AeFinder.Sdk.Processor;
using Google.Protobuf.WellKnownTypes;
using Portkey.Contracts.BingoGameContract;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class BingoedProcessor : CAHolderTransactionProcessorBase<Bingoed>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).BingoGameContractAddress;
    }

    public override async Task ProcessAsync(Bingoed logEvent, LogEventContext context)
    {
        if (logEvent.PlayerAddress == null || logEvent.PlayerAddress.Value == null)
        {
            return;
        }
        
        if (!IsValidTransaction(context.ChainId, context.Transaction.To, context.Transaction.MethodName, context.Transaction.Params)) return;
        var holder = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId,
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
                FromAddress = GetContractAddress(context.ChainId),
                ToAddress = logEvent.PlayerAddress.ToBase58(),
                Amount = (logEvent.Amount + logEvent.Award) / 100000000,
                FromChainId = context.ChainId,
                ToChainId = context.ChainId,
            },
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };
        transIndex.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);

        await SaveEntityAsync(transIndex);
        var index = await GetEntityAsync<BingoGameIndex>(logEvent.PlayId.ToHex());
        if (index == null)
        {
            return;
        }
        
        index.BingoBlockHeight = context.Block.BlockHeight;
        index.BingoId = context.Transaction.TransactionId;
        index.BingoTime = context.Block.BlockTime.ToTimestamp().Seconds;
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

        index.BingoTransactionFee = feeList;
        index.IsComplete = true;
        index.Dices = logEvent.Dices.Dices.ToList();
        index.Award = logEvent.Award;
        index.BingoBlockHash = context.Block.BlockHash;
        await SaveEntityAsync(index);
        
        var staticsId = IdGenerateHelper.GetId(context.ChainId, logEvent.PlayerAddress.ToBase58());
        var bingoStaticsIndex = await GetEntityAsync<BingoGameStaticsIndex>(staticsId);
        if (bingoStaticsIndex == null)
        {
            bingoStaticsIndex = new BingoGameStaticsIndex
            {
                Id = staticsId,
                PlayerAddress = logEvent.PlayerAddress.ToBase58(),
                Amount = logEvent.Amount,
                Award = logEvent.Award,
                TotalWins = logEvent.Award > 0 ? 1 : 0,
                TotalPlays = 1
            };
        }
        else
        {
            bingoStaticsIndex.Amount += logEvent.Amount;
            bingoStaticsIndex.Award += logEvent.Award;
            bingoStaticsIndex.TotalPlays += 1;
            bingoStaticsIndex.TotalWins += logEvent.Award > 0 ? 1 : 0;
        }
        
        await SaveEntityAsync(bingoStaticsIndex);
    }
}