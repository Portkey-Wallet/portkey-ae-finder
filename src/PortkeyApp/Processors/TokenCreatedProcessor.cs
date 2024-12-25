using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using AElf.Types;
using Newtonsoft.Json;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;
using Volo.Abp.ObjectMapping;

namespace PortkeyApp.Processors;

public class TokenCreatedProcessor : LogEventProcessorBase<TokenCreated>
{
    private readonly IObjectMapper _objectMapper;
    private readonly IAeFinderLogger _logger;

    public TokenCreatedProcessor(IObjectMapper objectMapper, IAeFinderLogger logger)
    {
        _objectMapper = objectMapper;
        _logger = logger;
    }

    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress;
    }

    public override async Task ProcessAsync(TokenCreated logEvent, LogEventContext context)
    {
        var tokenType = TokenHelper.GetTokenType(logEvent.Symbol);
        if (tokenType == TokenType.Token)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, logEvent.Symbol);
            var tokenInfoIndex = await GetEntityAsync<TokenInfoIndex>(id);
            if (tokenInfoIndex != null) return;

            tokenInfoIndex = new TokenInfoIndex
            {
                Id = IdGenerateHelper.GetId(context.ChainId, logEvent.Symbol),
                TokenContractAddress = GetContractAddress(context.ChainId)
            };
            _objectMapper.Map(logEvent, tokenInfoIndex);
            tokenInfoIndex.Type = TokenHelper.GetTokenType(logEvent.Symbol);

            if (logEvent.ExternalInfo is { Value.Count: > 0 })
            {
                tokenInfoIndex.ExternalInfoDictionary = logEvent.ExternalInfo.Value
                    .Where(t => !t.Key.IsNullOrWhiteSpace())
                    .ToDictionary(item => item.Key, item => item.Value);
                
                tokenInfoIndex.ImageUrl = NftExternalInfoHelper.GetFtImageUrl(logEvent.ExternalInfo.Value);
            }

            tokenInfoIndex.Issuer = GetIssuerAddress(logEvent);
            tokenInfoIndex.ExternalInfoDictionary ??= new Dictionary<string, string>();
            await SaveEntityAsync(tokenInfoIndex);
        }


        if (tokenType == TokenType.NFTCollection)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, logEvent.Symbol);
            var nftCollectionInfoIndex = await GetEntityAsync<NFTCollectionInfoIndex>(id);
            if (nftCollectionInfoIndex != null) return;
            nftCollectionInfoIndex = new NFTCollectionInfoIndex()
            {
                Id = IdGenerateHelper.GetId(context.ChainId, logEvent.Symbol),
                TokenContractAddress = GetContractAddress(context.ChainId)
            };

            _objectMapper.Map(logEvent, nftCollectionInfoIndex);
            nftCollectionInfoIndex.Type = TokenHelper.GetTokenType(logEvent.Symbol);
            nftCollectionInfoIndex.Issuer = GetIssuerAddress(logEvent);
            if (logEvent.ExternalInfo is { Value.Count: > 0 })
            {
                var externalInfo = logEvent.ExternalInfo.Value;
                var buildNftExternalInfo = NftExternalInfoHelper.BuildNftExternalInfo(externalInfo);
                _objectMapper.Map(buildNftExternalInfo, nftCollectionInfoIndex);
            }

            nftCollectionInfoIndex.ExternalInfoDictionary ??= new Dictionary<string, string>();
            await SaveEntityAsync(nftCollectionInfoIndex);
        }

        if (tokenType == TokenType.NFTItem)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, logEvent.Symbol);
            var nftInfoIndex = await GetEntityAsync<NFTInfoIndex>(id);
            if (nftInfoIndex != null) return;

            nftInfoIndex = new NFTInfoIndex()
            {
                Id = IdGenerateHelper.GetId(context.ChainId, logEvent.Symbol),
                TokenContractAddress = GetContractAddress(context.ChainId)
            };
            _objectMapper.Map(logEvent, nftInfoIndex);
            nftInfoIndex.Type = TokenHelper.GetTokenType(logEvent.Symbol);
            nftInfoIndex.Issuer = GetIssuerAddress(logEvent);

            if (logEvent.ExternalInfo is { Value.Count: > 0 })
            {
                var externalInfo = logEvent.ExternalInfo.Value;
                var nftExternalInfo = NftExternalInfoHelper.BuildNftExternalInfo(externalInfo);

                _objectMapper.Map(nftExternalInfo, nftInfoIndex);
            }

            var nftCollectionSymbol = TokenHelper.GetNFTCollectionSymbol(logEvent.Symbol);
            var collectionId = IdGenerateHelper.GetId(context.ChainId, nftCollectionSymbol);
            var nftCollectionInfoIndex = await GetEntityAsync<NFTCollectionInfoIndex>(collectionId);
            if (nftCollectionInfoIndex != null)
            {
                nftInfoIndex.CollectionSymbol = nftCollectionInfoIndex.Symbol;
                nftInfoIndex.CollectionName = nftCollectionInfoIndex.TokenName;
                nftInfoIndex.Lim = nftCollectionInfoIndex.Lim;
                if (!nftCollectionInfoIndex.InscriptionName.IsNullOrWhiteSpace())
                {
                    nftInfoIndex.InscriptionName = nftCollectionInfoIndex.InscriptionName;
                }
            }

            nftInfoIndex.ExternalInfoDictionary ??= new Dictionary<string, string>();
            await SaveEntityAsync(nftInfoIndex);
        }
    }

    private string GetIssuerAddress(TokenCreated logEvent)
    {
        try
        {
            if (logEvent.Issuer == null || logEvent.Issuer.Value == null || logEvent.Issuer.Value.Length != 32)
            {
                _logger.LogWarning("GetIssuerAddress fail, data:{0}",JsonConvert.SerializeObject(logEvent));
                return string.Empty;
            }

            return logEvent.Issuer.ToBase58();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"GetIssuerAddress error, {0}",JsonConvert.SerializeObject(logEvent));
            return string.Empty;
        }
    }
}