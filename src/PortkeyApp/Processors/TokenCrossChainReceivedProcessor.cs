using AeFinder.Sdk.Processor;
using AElf;
using AElf.Contracts.MultiToken;
using Google.Protobuf.WellKnownTypes;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TokenCrossChainReceivedProcessor : CAHolderTokenBalanceProcessorBase<CrossChainReceived>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress;
    }

    public override async Task ProcessAsync(CrossChainReceived logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        var holder = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, logEvent.To.ToBase58()));
        if (holder != null)
        {
            await ModifyBalanceAsync(holder.CAAddress, logEvent.Symbol, logEvent.Amount, context);
        }
        else
        {
            await ModifyBalanceAsync(logEvent.To.ToBase58(), logEvent.Symbol, logEvent.Amount, context);
        }
    }
    
    protected override async Task HandlerTransactionIndexAsync(CrossChainReceived eventValue, LogEventContext context)
    {
        if (!IsValidTransaction(context.ChainId, context.Transaction.To, context.Transaction.MethodName, context.Transaction.Params)) return;
        
        var tokenInfoIndex = await GetTokenInfoIndexFromStateOrChainAsync(eventValue.Symbol, context);
        var nftInfoIndex = await GetNftInfoIndexFromStateOrChainAsync(eventValue.Symbol, context);
        var from_manager = await GetEntityAsync<CAHolderManagerIndex>(IdGenerateHelper.GetId(context.ChainId, eventValue.From.ToBase58()));
        string fromManagerCAAddress = from_manager == null ? "" : from_manager.CAAddresses.FirstOrDefault();
        await SaveEntityAsync(GetCaHolderTransactionIndex(eventValue, tokenInfoIndex,nftInfoIndex,
            fromManagerCAAddress,context));
        
        var to_ca = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, eventValue.To.ToBase58()));
        if (to_ca != null)
        {
            await AddCAHolderTransactionAddressAsync(to_ca.CAAddress, eventValue.From.ToBase58(),
                ChainHelper.ConvertChainIdToBase58(eventValue.FromChainId), context);
        }
        else
        {
            var to_manager = await GetEntityAsync<CAHolderManagerIndex>(IdGenerateHelper.GetId(context.ChainId, eventValue.To.ToBase58()));
            if (to_manager != null)
            {
                await AddCAHolderTransactionAddressAsync(to_manager.CAAddresses.FirstOrDefault(), eventValue.From.ToBase58(),
                    ChainHelper.ConvertChainIdToBase58(eventValue.FromChainId), context);
            }
        }
    }
    
    private CAHolderTransactionIndex GetCaHolderTransactionIndex(CrossChainReceived transferred, TokenInfoIndex tokenInfoIndex,
        NFTInfoIndex nftInfoIndex,string fromManagerCAAddress, LogEventContext context)
    {
        var index = new CAHolderTransactionIndex
        {
            Id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId),
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = context.Transaction.From,
            TokenInfo=tokenInfoIndex,
            NftInfo = nftInfoIndex,
            TransactionFee = GetTransactionFee(context.Transaction.ExtraProperties),
            TransferInfo = new TransferInfo
            {
                Amount = transferred.Amount,
                FromAddress = transferred.From.ToBase58(),
                FromCAAddress = fromManagerCAAddress,
                ToAddress = transferred.To.ToBase58(),
                FromChainId = ChainHelper.ConvertChainIdToBase58(transferred.FromChainId),
                ToChainId = context.ChainId,
                TransferTransactionId = transferred.TransferTransactionId.ToHex()
            },
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };

        index.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        return index;
    }
}