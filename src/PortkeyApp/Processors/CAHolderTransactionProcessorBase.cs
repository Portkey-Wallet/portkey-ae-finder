using AeFinder.Sdk;
using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using AElf.CSharp.Core;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;
using Volo.Abp.ObjectMapping;

namespace PortkeyApp.Processors;

public abstract class CAHolderTransactionProcessorBase<TEvent> : LogEventProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();
    protected IBlockChainService BlockChainService => LazyServiceProvider.LazyGetRequiredService<IBlockChainService>();

    protected IAeFinderLogger Logger => LazyServiceProvider.LazyGetRequiredService<IAeFinderLogger>();

    protected bool IsValidTransaction(string chainId, string to, string methodName, string parameter)
    {
        if (!ConfigConstants.CAHolderTransactionInfos.Where(t => t.ChainId == chainId).Any(t =>
                t.ContractAddress == to && t.MethodName == methodName &&
                t.EventNames.Contains(GetEventName()))) return false;
        if (methodName == "ManagerForwardCall" &&
            !IsValidManagerForwardCallTransaction(chainId, to, methodName, parameter)) return false;
        return true;
    }

    protected string GetToContractAddress(string chainId, string to, string methodName, string parameter)
    {
        if (to == ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress &&
            methodName == "ManagerForwardCall")
        {
            var managerForwardCallInput = ManagerForwardCallInput.Parser.ParseFrom(ByteString.FromBase64(parameter));
            return managerForwardCallInput.ContractAddress.ToBase58();
        }

        return to;
    }

    protected bool IsMultiTransaction(string chainId, string to, string methodName)
    {
        var caHolderTransactionInfo = ConfigConstants.CAHolderTransactionInfos.FirstOrDefault(t =>
            t.ChainId == chainId &&
            t.ContractAddress == to && t.MethodName == methodName &&
            t.EventNames.Contains(GetEventName()));
        return caHolderTransactionInfo?.MultiTransaction ?? false;
    }

    private bool IsValidManagerForwardCallTransaction(string chainId, string to, string methodName, string parameter)
    {
        if (methodName != "ManagerForwardCall") return false;
        if (to != ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress && to !=
            ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).AnotherCAContractAddress) return false;
        var managerForwardCallInput = ManagerForwardCallInput.Parser.ParseFrom(ByteString.FromBase64(parameter));
        return IsValidTransaction(chainId, managerForwardCallInput.ContractAddress.ToBase58(),
            managerForwardCallInput.MethodName, managerForwardCallInput.Args.ToBase64());
    }

    protected string GetMethodName(string methodName, string parameter)
    {
        if (methodName == "ManagerTransfer") return "Transfer";
        if (methodName != "ManagerForwardCall") return methodName;
        var managerForwardCallInput = ManagerForwardCallInput.Parser.ParseFrom(ByteString.FromBase64(parameter));
        return GetMethodName(managerForwardCallInput.MethodName, managerForwardCallInput.Args.ToBase64());
    }

    protected Dictionary<string, long> GetTransactionFee(Dictionary<string, string> extraProperties)
    {
        return new Dictionary<string, long>();
    }

    protected virtual Task HandlerTransactionIndexAsync(TEvent eventValue, LogEventContext context)
    {
        return Task.CompletedTask;
    }

    protected async Task AddCAHolderTransactionAddressAsync(string caAddress, string address, string addressChainId,
        LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(context.ChainId, caAddress, address, addressChainId);
        var caHolderTransactionAddressIndex = await GetEntityAsync<CAHolderTransactionAddressIndex>(id);
        if (caHolderTransactionAddressIndex == null)
        {
            caHolderTransactionAddressIndex = new CAHolderTransactionAddressIndex
            {
                Id = id,
                CAAddress = caAddress,
                Address = address,
                AddressChainId = addressChainId
            };
        }

        var transactionTime = context.Block.BlockTime.ToTimestamp().Seconds;
        if (caHolderTransactionAddressIndex.TransactionTime >= transactionTime) return;
        caHolderTransactionAddressIndex.TransactionTime = transactionTime;
        await SaveEntityAsync(caHolderTransactionAddressIndex);
    }

    protected async Task<string> ProcessCAHolderTransactionAsync(LogEventContext context, string caAddress)
    {
        if (!IsValidTransaction(context.ChainId, context.Transaction.To, context.Transaction.MethodName,
                context.Transaction.Params)) return null;
        var holder = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId,
            caAddress));
        if (holder == null) return null;

        var id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId);
        var transIndex = await GetEntityAsync<CAHolderTransactionIndex>(id);
        var transactionFee = GetTransactionFee(context.Transaction.ExtraProperties);
        if (transIndex != null)
        {
            transactionFee = transIndex.TransactionFee.IsNullOrEmpty() ? transactionFee : transIndex.TransactionFee;
        }

        var index = new CAHolderTransactionIndex
        {
            Id = id,
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = caAddress,
            TransactionFee = transactionFee,
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };

        index.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        await SaveEntityAsync(index);
        return holder.CAAddress;
    }

    protected async Task<string> ProcessCAHolderTransactionAsync(LogEventContext context, string caAddress,
        int platform)
    {
        if (!IsValidTransaction(context.ChainId, context.Transaction.To, context.Transaction.MethodName,
                context.Transaction.Params)) return null;
        var holder = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId,
            caAddress));
        if (holder == null) return null;

        var id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId);
        var transIndex = await GetEntityAsync<CAHolderTransactionIndex>(id);
        var transactionFee = GetTransactionFee(context.Transaction.ExtraProperties);
        if (transIndex != null)
        {
            transactionFee = transIndex.TransactionFee.IsNullOrEmpty() ? transactionFee : transIndex.TransactionFee;
        }

        var index = new CAHolderTransactionIndex
        {
            Id = id,
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = caAddress,
            TransactionFee = transactionFee,
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status,
            Platform = platform
        };

        index.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        await SaveEntityAsync(index);
        return holder.CAAddress;
    }

    protected long GetFeeAmount(Dictionary<string, string> extraProperties)
    {
        var feeMap = GetTransactionFee(extraProperties);
        if (feeMap.TryGetValue("ELF", out var value))
        {
            return value;
        }

        return 0;
    }

    protected async Task<NFTInfoIndex> GetNftInfoIndexFromStateOrChainAsync(string symbol, LogEventContext context)
    {
        if (TokenHelper.GetTokenType(symbol) != TokenType.NFTItem)
        {
            return null;
        }

        var nftInfoIndex =
            await GetEntityAsync<NFTInfoIndex>(IdGenerateHelper.GetId(context.ChainId, symbol));

        if (nftInfoIndex != null)
        {
            return nftInfoIndex;
        }

        var nftInfoId = IdGenerateHelper.GetId(context.ChainId, symbol);
        nftInfoIndex = new NFTInfoIndex()
        {
            Id = nftInfoId,
            Symbol = symbol,
            Type = TokenType.NFTItem,
            TokenContractAddress = GetContractAddress(context.ChainId)
        };

        var nftInfo = await BlockChainService.ViewContractAsync<AElf.Contracts.MultiToken.TokenInfo>(
            context.ChainId,
            ConfigConstants.ContractInfos.First(c => c.ChainId == context.ChainId).TokenContractAddress,
            "GetTokenInfo",
            new GetTokenInfoInput
            {
                Symbol = nftInfoIndex.Symbol
            });
        if (nftInfo.Symbol == nftInfoIndex.Symbol)
        {
            ObjectMapper.Map(nftInfo, nftInfoIndex);
            if (nftInfo.ExternalInfo is { Value: { Count: > 0 } })
            {
                nftInfoIndex.ExternalInfoDictionary = nftInfo.ExternalInfo.Value
                    .Where(t => !t.Key.IsNullOrWhiteSpace())
                    .ToDictionary(item => item.Key, item => item.Value);


                if (nftInfo.ExternalInfo.Value.TryGetValue("__nft_image_url", out var image))
                {
                    nftInfoIndex.ImageUrl = image;
                }
                else if (nftInfo.ExternalInfo.Value.TryGetValue("inscription_image", out var inscriptionImage))
                {
                    nftInfoIndex.ImageUrl = inscriptionImage;
                }
                else if (nftInfo.ExternalInfo.Value.TryGetValue("__nft_image_uri", out var inscriptionImageUrl))
                {
                    nftInfoIndex.ImageUrl = inscriptionImageUrl;
                }
                else if (nftInfo.ExternalInfo.Value.TryGetValue("__inscription_image", out var imageUrl))
                {
                    nftInfoIndex.ImageUrl = imageUrl;
                }
            }

            nftInfoIndex.ExternalInfoDictionary ??= new Dictionary<string, string>();
        }

        return nftInfoIndex;
    }

    protected async Task<TokenInfoIndex> GetTokenInfoIndexFromStateOrChainAsync(string symbol, LogEventContext context)
    {
        if (TokenHelper.GetTokenType(symbol) != TokenType.Token)
        {
            return null;
        }

        var tokenInfoIndex = await GetEntityAsync<TokenInfoIndex>(IdGenerateHelper.GetId(context.ChainId, symbol));
        if (tokenInfoIndex != null)
        {
            return tokenInfoIndex;
        }


        tokenInfoIndex = new TokenInfoIndex
        {
            Id = IdGenerateHelper.GetId(context.ChainId, symbol),
            TokenContractAddress = GetContractAddress(context.ChainId),
            Type = TokenType.Token,
            Symbol = symbol
        };

        var tokenInfo = await BlockChainService.ViewContractAsync<AElf.Contracts.MultiToken.TokenInfo>(
            context.ChainId,
            ConfigConstants.ContractInfos.First(c => c.ChainId == context.ChainId).TokenContractAddress,
            "GetTokenInfo",
            new GetTokenInfoInput
            {
                Symbol = tokenInfoIndex.Symbol
            });
        if (tokenInfo.Symbol == tokenInfoIndex.Symbol)
        {
            ObjectMapper.Map(tokenInfo, tokenInfoIndex);
            if (tokenInfo.ExternalInfo != null && tokenInfo.ExternalInfo.Value is { Count: > 0 })
            {
                tokenInfoIndex.ExternalInfoDictionary = tokenInfo.ExternalInfo.Value
                    .Where(t => !t.Key.IsNullOrWhiteSpace())
                    .ToDictionary(item => item.Key, item => item.Value);

                tokenInfoIndex.ImageUrl = TokenHelper.GetFtImageUrl(tokenInfo.ExternalInfo.Value);
            }

            tokenInfoIndex.ExternalInfoDictionary ??= new Dictionary<string, string>();
        }

        return tokenInfoIndex;
    }
}