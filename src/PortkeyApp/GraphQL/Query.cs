using System.Linq.Dynamic.Core;
using AeFinder.Sdk;
using PortkeyApp.Entities;
using GraphQL;
using Volo.Abp.ObjectMapping;
using System.Linq.Expressions;

namespace PortkeyApp.GraphQL;

public class Query
{
    public static async Task<List<MyEntityDto>> MyEntity(
        [FromServices] IReadOnlyRepository<MyEntity> repository,
        [FromServices] IObjectMapper objectMapper,
        GetMyEntityInput input)
    {
        var queryable = await repository.GetQueryableAsync();

        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);

        if (!input.Address.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Address == input.Address);
        }

        var accounts = queryable.ToList();

        return objectMapper.Map<List<MyEntity>, List<MyEntityDto>>(accounts);
    }

    public static async Task<List<TokenInfoDto>> TokenInfo(
        [FromServices] IReadOnlyRepository<TokenInfoIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetTokenInfoDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();

        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (!dto.Symbol.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Symbol == dto.Symbol);
        }

        if (!dto.SymbolKeyword.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Symbol.Contains(dto.SymbolKeyword));
        }

        var result = queryable.OrderBy(t => t.Symbol).Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList();
        return objectMapper.Map<List<TokenInfoIndex>, List<TokenInfoDto>>(result);
    }

    [Name("caHolderTransaction")]
    public static async Task<CAHolderTransactionPageResultDto> CAHolderTransaction(
        [FromServices] IReadOnlyRepository<CAHolderTransactionIndex> repository,
        [FromServices] IReadOnlyRepository<TransactionFeeChangedIndex> transactionFeeRepository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderTransactionDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();

        if (dto.StartBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight >= dto.StartBlockHeight);
        }

        if (dto.EndBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight <= dto.EndBlockHeight);
        }

        if (dto.StartTime > 0)
        {
            queryable = queryable.Where(t => t.Timestamp >= dto.StartTime);
        }

        if (dto.EndTime > 0)
        {
            queryable = queryable.Where(t => t.Timestamp <= dto.EndTime);
        }

        if (!dto.Symbol.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.NftInfo.Symbol == dto.Symbol || t.TokenInfo.Symbol == dto.Symbol);
        }

        if (!dto.BlockHash.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHash == dto.BlockHash);
        }

        if (!dto.TransactionId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.TransactionId == dto.TransactionId);
        }

        if (!dto.TransferTransactionId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.TransferInfo.TransferTransactionId == dto.TransferTransactionId);
        }

        if (!dto.MethodNames.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.MethodNames.Select(name =>
                        (Expression<Func<CAHolderTransactionIndex, bool>>)(t => t.MethodName == name))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        if (!dto.CAAddressInfos.IsNullOrEmpty())
        {
            Expression<Func<CAHolderTransactionIndex, bool>> expression = t => true;
            foreach (var info in dto.CAAddressInfos)
            {
                expression = expression.Or(t =>
                    (t.Metadata.ChainId == info.ChainId && t.FromAddress == info.CAAddress) ||
                    (t.TransferInfo != null && t.TransferInfo.FromChainId == info.ChainId &&
                     t.TransferInfo.FromAddress == info.CAAddress) ||
                    (t.TransferInfo != null && t.TransferInfo.FromChainId == info.ChainId &&
                     t.TransferInfo.FromCAAddress == info.CAAddress) ||
                    (t.TransferInfo != null && t.TransferInfo.ToChainId == info.ChainId &&
                     t.TransferInfo.ToAddress == info.CAAddress));

                expression = expression.Or(t => t.TokenTransferInfos.Any(f =>
                    f.TransferInfo.FromAddress == info.CAAddress && f.TransferInfo.FromChainId == info.ChainId));

                expression = expression.Or(t => t.TokenTransferInfos.Any(f =>
                    f.TransferInfo.FromCAAddress == info.CAAddress && f.TransferInfo.FromChainId == info.ChainId));

                expression = expression.Or(t => t.TokenTransferInfos.Any(f =>
                    f.TransferInfo.ToAddress == info.CAAddress && f.TransferInfo.ToChainId == info.ChainId));
            }

            queryable = queryable.Where(expression);
        }

        var result = queryable.OrderByDescending(t => t.Timestamp).Skip(dto.SkipCount).Take(dto.MaxResultCount)
            .ToList();
        var dataList = objectMapper.Map<List<CAHolderTransactionIndex>, List<CAHolderTransactionDto>>(result);

        var transactionIds =
            dataList.Where(t => !t.TransactionId.IsNullOrEmpty()).Select(t => t.TransactionId).ToList();

        if (!transactionIds.IsNullOrEmpty())
        {
            var feeQueryable = await transactionFeeRepository.GetQueryableAsync();
            feeQueryable = feeQueryable.Where(
                transactionIds.Select(transId =>
                        (Expression<Func<TransactionFeeChangedIndex, bool>>)(t => t.TransactionId == transId))
                    .Aggregate((prev, next) => prev.Or(next)));

            var feeList = feeQueryable.ToList();
            foreach (var transaction in dataList)
            {
                if (string.IsNullOrEmpty(transaction.TransactionId)) continue;
                var transactionFee = feeList.FirstOrDefault(t => t.TransactionId == transaction.TransactionId);
                if (transactionFee == null || string.IsNullOrEmpty(transactionFee.CAAddress))
                    transaction.IsManagerConsumer = true;
            }
        }

        return new CAHolderTransactionPageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = dataList
        };
    }

    [Name("twoCaHolderTransaction")]
    public static async Task<CAHolderTransactionPageResultDto> TwoCAHolderTransaction(
        [FromServices] IReadOnlyRepository<CAHolderTransactionIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetTwoCAHolderTransactionDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();

        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (dto.StartBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight >= dto.StartBlockHeight);
        }

        if (dto.EndBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight <= dto.EndBlockHeight);
        }

        if (!dto.Symbol.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.TokenInfo.Symbol == dto.Symbol);
        }

        if (!dto.BlockHash.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHash == dto.BlockHash);
        }

        if (!dto.MethodNames.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.MethodNames.Select(name =>
                        (Expression<Func<CAHolderTransactionIndex, bool>>)(t => t.MethodName == name))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        if (dto.CAAddressInfos is not { Count: 2 })
        {
            return new CAHolderTransactionPageResultDto
            {
                TotalRecordCount = 0,
                Data = new List<CAHolderTransactionDto>()
            };
        }

        var expression = GetTwoCaHolderQueryExpression(dto.CAAddressInfos[0], dto.CAAddressInfos[1]);
        queryable = queryable.Where(expression);

        var result = queryable.OrderByDescending(t => t.Timestamp).Take(dto.SkipCount).Take(dto.MaxResultCount)
            .ToList();

        return new CAHolderTransactionPageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = objectMapper.Map<List<CAHolderTransactionIndex>, List<CAHolderTransactionDto>>(result)
        };
    }

    private static Expression<Func<CAHolderTransactionIndex, bool>> GetTwoCaHolderQueryExpression(
        CAAddressInfo fromHolder, CAAddressInfo toHolder)
    {
        Expression<Func<CAHolderTransactionIndex, bool>> expression = t => true;

        expression = expression.Or(t =>
            (t.Metadata.ChainId == fromHolder.ChainId && t.FromAddress == fromHolder.CAAddress) &&
            (t.TransferInfo != null && t.TransferInfo.ToChainId == toHolder.ChainId &&
             t.TransferInfo.ToAddress == toHolder.CAAddress));

        expression = expression.Or(t =>
            t.Metadata.ChainId == fromHolder.ChainId && t.TransferInfo != null &&
            t.TransferInfo.FromAddress == fromHolder.CAAddress &&
            t.TransferInfo.ToChainId == toHolder.ChainId &&
            t.TransferInfo.ToAddress == toHolder.CAAddress);

        expression = expression.Or(t =>
            t.Metadata.ChainId == fromHolder.ChainId && t.TransferInfo != null &&
            t.TransferInfo.FromCAAddress == fromHolder.CAAddress &&
            t.TransferInfo.ToChainId == toHolder.ChainId &&
            t.TransferInfo.ToAddress == toHolder.CAAddress);

        expression = expression.Or(t =>
            t.Metadata.ChainId == toHolder.ChainId &&
            t.FromAddress == toHolder.CAAddress &&
            t.TransferInfo != null &&
            t.TransferInfo.ToChainId == fromHolder.ChainId &&
            t.TransferInfo.ToAddress == fromHolder.CAAddress);

        expression = expression.Or(t =>
            t.Metadata.ChainId == toHolder.ChainId &&
            t.TransferInfo != null &&
            t.TransferInfo.FromAddress == toHolder.CAAddress &&
            t.TransferInfo.ToChainId == fromHolder.ChainId &&
            t.TransferInfo.ToAddress == fromHolder.CAAddress);

        expression = expression.Or(t =>
            t.Metadata.ChainId == toHolder.ChainId &&
            t.TransferInfo != null &&
            t.TransferInfo.FromCAAddress == toHolder.CAAddress &&
            t.TransferInfo.ToChainId == fromHolder.ChainId &&
            t.TransferInfo.ToAddress == fromHolder.CAAddress);

        return expression;
    }

    [Name("caHolderInfo")]
    public static async Task<List<CAHolderInfoDto>> CAHolderInfo(
        [FromServices] IReadOnlyRepository<CAHolderIndex> repository,
        [FromServices] IReadOnlyRepository<LoginGuardianIndex> repositoryLoginGuardian,
        [FromServices] IObjectMapper objectMapper, GetCAHolderInfoDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (string.IsNullOrWhiteSpace(dto.CAHash) && string.IsNullOrWhiteSpace(dto.LoginGuardianIdentifierHash))
        {
            if (!dto.CAAddresses.IsNullOrEmpty())
            {
                queryable = queryable.Where(
                    dto.CAAddresses.Select(address =>
                            (Expression<Func<CAHolderIndex, bool>>)(t => t.CAAddress == address))
                        .Aggregate((prev, next) => prev.Or(next)));
            }
        }
        else
        {
            string hash;
            if (!string.IsNullOrWhiteSpace(dto.CAHash))
            {
                hash = dto.CAHash;
            }
            else
            {
                var loginGuardianQueryable = await repositoryLoginGuardian.GetQueryableAsync();
                loginGuardianQueryable = loginGuardianQueryable.Where(t =>
                    t.LoginGuardian.IdentifierHash == dto.LoginGuardianIdentifierHash);

                var holderInfoResult = loginGuardianQueryable.FirstOrDefault();
                if (holderInfoResult == null) return new List<CAHolderInfoDto>();

                hash = holderInfoResult.CAHash;
            }

            queryable = queryable.Where(t => t.CAHash == hash);
        }

        var result = queryable.Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList();
        if (result.Count > 0)
        {
            await AddOriginChainIdIfNullAsync(result, repository);
        }

        return objectMapper.Map<List<CAHolderIndex>, List<CAHolderInfoDto>>(result);
    }

    private static async Task AddOriginChainIdIfNullAsync(List<CAHolderIndex> holders,
        IReadOnlyRepository<CAHolderIndex> repository)
    {
        foreach (var holder in holders.Where(holder => holder.OriginChainId.IsNullOrWhiteSpace()))
        {
            holder.OriginChainId = await GetOriginChainIdAsync(holder.CAHash, repository);
        }
    }

    private static async Task<string> GetOriginChainIdAsync(string caHash,
        IReadOnlyRepository<CAHolderIndex> repository)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(t => t.CAHash == caHash);
        var result = queryable.ToList();

        return result.FirstOrDefault(t => !t.OriginChainId.IsNullOrWhiteSpace())?.OriginChainId;
    }

    [Name("caHolderManagerInfo")]
    public static async Task<List<CAHolderManagerDto>> CAHolderManagerInfo(
        [FromServices] IReadOnlyRepository<CAHolderIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderManagerInfoDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (!dto.CAHash.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.CAHash == dto.CAHash);
        }

        if (!dto.CAAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.CAAddresses.Select(address =>
                        (Expression<Func<CAHolderIndex, bool>>)(t => t.CAAddress == address))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        // mustQuery.Add(n => n.Nested(n =>
        //     n.Path("ManagerInfos").Query(q => q.Term(i => i.Field("ManagerInfos.address").Value(dto.Manager)))));
        if (!dto.Manager.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.ManagerInfos.Any(f => f.Address == dto.Manager));
        }

        var result = queryable.OrderBy(t => t.Metadata.Block.BlockHeight).Skip(dto.SkipCount).Take(dto.MaxResultCount)
            .ToList();
        return objectMapper.Map<List<CAHolderIndex>, List<CAHolderManagerDto>>(result);
    }

    public static async Task<List<LoginGuardianDto>> LoginGuardianInfo(
        [FromServices] IReadOnlyRepository<LoginGuardianIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetLoginGuardianInfoDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (!dto.CAHash.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.CAHash == dto.CAHash);
        }

        if (!dto.CAAddress.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.CAAddress == dto.CAAddress);
        }

        if (!dto.LoginGuardian.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.LoginGuardian.IdentifierHash == dto.LoginGuardian);
        }

        var result = queryable.OrderBy(t => t.Metadata.Block.BlockHeight).Skip(dto.SkipCount).Take(dto.MaxResultCount)
            .ToList();
        return objectMapper.Map<List<LoginGuardianIndex>, List<LoginGuardianDto>>(result);
    }

    [Name("caHolderNFTCollectionBalanceInfo")]
    public static async Task<CAHolderNFTCollectionBalancePageResultDto> CAHolderNFTCollecitonBalanceInfo(
        [FromServices] IReadOnlyRepository<CAHolderNFTCollectionBalanceIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderNFTCollectionInfoDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (!dto.Symbol.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.NftCollectionInfo.Symbol == dto.Symbol);
        }

        queryable = queryable.Where(t => t.TokenIdCount > 0);
        //mustQuery.Add(q => q.Script(i => i.Script(sq => sq.Source($"doc['tokenIds'].getLength()>0"))));

        if (!dto.CAAddressInfos.IsNullOrEmpty())
        {
            Expression<Func<CAHolderNFTCollectionBalanceIndex, bool>> expression = t => true;
            foreach (var info in dto.CAAddressInfos)
            {
                expression = expression.Or(t => t.Metadata.ChainId == info.ChainId && t.CAAddress == info.CAAddress);
            }

            queryable = queryable.Where(expression);
        }

        var result = queryable.OrderBy(t => t.NftCollectionInfo.Symbol).ThenBy(t => t.Metadata.ChainId)
            .Skip(dto.SkipCount)
            .Take(dto.MaxResultCount).ToList();

        // temp
        // var result = queryable.OrderBy(t => t.NftCollectionInfo.Symbol).ThenBy(t => t.Metadata.ChainId)
        //     .Skip(IndexerConstant.DefaultSkip)
        //     .Take(IndexerConstant.DefaultLimit).ToList().Where(t => !t.TokenIds.IsNullOrEmpty()).Skip(dto.SkipCount)
        //     .Take(dto.MaxResultCount).ToList();

        var dataList =
            objectMapper
                .Map<List<CAHolderNFTCollectionBalanceIndex>, List<CAHolderNFTCollectionBalanceInfoDto>>(result);

        return new CAHolderNFTCollectionBalancePageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = dataList
        };
    }

    [Name("caHolderNFTBalanceInfo")]
    public static async Task<CAHolderNFTBalancePageResultDto> CAHolderNFTBalanceInfo(
        [FromServices] IReadOnlyRepository<CAHolderNFTBalanceIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderNFTInfoDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (!dto.Symbol.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.NftInfo.Symbol == dto.Symbol);
        }

        if (!dto.CollectionSymbol.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.NftInfo.CollectionSymbol == dto.CollectionSymbol);
        }

        queryable = queryable.Where(t => t.Balance > 0);

        if (!dto.CAAddressInfos.IsNullOrEmpty())
        {
            Expression<Func<CAHolderNFTBalanceIndex, bool>> expression = t => true;
            foreach (var info in dto.CAAddressInfos)
            {
                expression = expression.Or(t => t.CAAddress == info.CAAddress && t.Metadata.ChainId == info.ChainId);
            }

            queryable = queryable.Where(expression);
        }

        var result = queryable.OrderBy(t => t.NftInfo.Symbol).ThenBy(t => t.Metadata.ChainId)
            .Skip(dto.SkipCount)
            .Take(dto.MaxResultCount).ToList();

        return new CAHolderNFTBalancePageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = objectMapper.Map<List<CAHolderNFTBalanceIndex>, List<CAHolderNFTBalanceInfoDto>>(result)
        };
    }

    [Name("caHolderTokenBalanceInfo")]
    public static async Task<CAHolderTokenBalancePageResultDto> CAHolderTokenBalanceInfo(
        [FromServices] IReadOnlyRepository<CAHolderTokenBalanceIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderTokenBalanceDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (!dto.Symbol.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.TokenInfo.Symbol == dto.Symbol);
        }

        if (!dto.CAAddressInfos.IsNullOrEmpty())
        {
            Expression<Func<CAHolderTokenBalanceIndex, bool>> expression = t => true;
            foreach (var info in dto.CAAddressInfos)
            {
                expression = expression.Or(t => t.Metadata.ChainId == info.ChainId && t.CAAddress == info.CAAddress);
            }

            queryable = queryable.Where(expression);
        }

        var result = queryable.OrderBy(t => t.TokenInfo.Symbol).ThenBy(t => t.Metadata.ChainId)
            .Skip(dto.SkipCount)
            .Take(dto.MaxResultCount).ToList();

        return new CAHolderTokenBalancePageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = objectMapper.Map<List<CAHolderTokenBalanceIndex>, List<CAHolderTokenBalanceDto>>(result)
        };
    }

    [Name("caHolderTransactionAddressInfo")]
    public static async Task<CAHolderTransactionAddressPageResultDto> CAHolderTransactionAddressInfo(
        [FromServices] IReadOnlyRepository<CAHolderTransactionAddressIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderTransactionAddressDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (!dto.CAAddressInfos.IsNullOrEmpty())
        {
            Expression<Func<CAHolderTransactionAddressIndex, bool>> expression = t => true;
            foreach (var info in dto.CAAddressInfos)
            {
                expression = expression.Or(t => t.Metadata.ChainId == info.ChainId && t.CAAddress == info.CAAddress);
            }

            queryable = queryable.Where(expression);
        }

        var result = queryable.OrderByDescending(t => t.TransactionTime).Skip(dto.SkipCount).Take(dto.MaxResultCount)
            .ToList();
        var dataList =
            objectMapper.Map<List<CAHolderTransactionAddressIndex>, List<CAHolderTransactionAddressDto>>(result);

        return new CAHolderTransactionAddressPageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = dataList
        };
    }

    public static async Task<List<LoginGuardianChangeRecordDto>> LoginGuardianChangeRecordInfo(
        [FromServices] IReadOnlyRepository<LoginGuardianChangeRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetLoginGuardianChangeRecordDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (dto.StartBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight >= dto.StartBlockHeight);
        }

        if (dto.EndBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight <= dto.EndBlockHeight);
        }

        var result = queryable.OrderBy(t => t.Metadata.Block.BlockHeight)
            .Skip(IndexerConstant.DefaultSkip)
            .Take(IndexerConstant.DefaultLimit).ToList();
        return objectMapper.Map<List<LoginGuardianChangeRecordIndex>, List<LoginGuardianChangeRecordDto>>(result);
    }

    [Name("caHolderManagerChangeRecordInfo")]
    public static async Task<List<CAHolderManagerChangeRecordDto>> CAHolderManagerChangeRecordInfo(
        [FromServices] IReadOnlyRepository<CAHolderManagerChangeRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderManagerChangeRecordDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (dto.StartBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight >= dto.StartBlockHeight);
        }

        if (dto.EndBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight <= dto.EndBlockHeight);
        }

        var result = queryable.OrderBy(t => t.Metadata.Block.BlockHeight)
            .Skip(IndexerConstant.DefaultSkip)
            .Take(IndexerConstant.DefaultLimit).ToList();
        return objectMapper.Map<List<CAHolderManagerChangeRecordIndex>, List<CAHolderManagerChangeRecordDto>>(result);
    }

    [Name("caHolderSearchTokenNFT")]
    public static async Task<CAHolderSearchTokenNFTPageResultDto> CAHolderSearchTokenNFT(
        [FromServices] IReadOnlyRepository<CAHolderSearchTokenNFTIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderSearchTokenNFTDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        queryable = queryable.Where(t => t.Balance > 0);
        if (!dto.CAAddressInfos.IsNullOrEmpty())
        {
            Expression<Func<CAHolderSearchTokenNFTIndex, bool>> expression = t => true;
            foreach (var info in dto.CAAddressInfos)
            {
                expression = expression.Or(t => t.Metadata.ChainId == info.ChainId && t.CAAddress == info.CAAddress);
            }

            queryable = queryable.Where(expression);
        }

        if (!string.IsNullOrEmpty(dto.SearchWord))
        {
            //long.TryParse(dto.SearchWord, out long tokenId);

            queryable = queryable.Where(t =>
                t.TokenInfo.Symbol.Contains(dto.SearchWord) || t.NftInfo.Symbol.Contains(dto.SearchWord) ||
                t.TokenInfo.TokenName.Contains(dto.SearchWord) || t.NftInfo.TokenName.Contains(dto.SearchWord));
        }

        var result = queryable.OrderBy(t => t.NftInfo.Symbol).ThenBy(t => t.Metadata.ChainId)
            .Skip(dto.SkipCount)
            .Take(dto.MaxResultCount).ToList();

        var dataList =
            objectMapper.Map<List<CAHolderSearchTokenNFTIndex>, List<CAHolderSearchTokenNFTDto>>(result);
        return new CAHolderSearchTokenNFTPageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = dataList
        };
    }

    [Name("caHolderTransferLimit")]
    public static async Task<CAHolderTransferLimitResultDto> CAHolderTransferLimitAsync(
        [FromServices] IReadOnlyRepository<TransferLimitIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderTransferLimitDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.CAHash.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.CaHash == dto.CAHash);
        }

        var data = queryable.ToList();
        return new CAHolderTransferLimitResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = objectMapper.Map<List<TransferLimitIndex>, List<CAHolderTransferlimitDto>>(data)
        };
    }

    [Name("caHolderManagerApproved")]
    public static async Task<CAHolderManagerApprovedPageResultDto> CAHolderManagerApprovedAsync(
        [FromServices] IReadOnlyRepository<ManagerApprovedIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderManagerApprovedDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (!dto.CAHash.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.CaHash == dto.CAHash);
        }

        if (!string.IsNullOrEmpty(dto.Spender))
            queryable = queryable.Where(t => t.Spender == dto.Spender);
        if (!string.IsNullOrEmpty(dto.Symbol))
            queryable = queryable.Where(t => t.Symbol == dto.Symbol);

        if (dto.StartHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight >= dto.StartHeight);
        }

        if (dto.EndHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight <= dto.EndHeight);
        }

        var result = queryable.Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList();
        return new CAHolderManagerApprovedPageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = objectMapper.Map<List<ManagerApprovedIndex>, List<CAHolderManagerApprovedDto>>(result)
        };
    }

    [Name("transferSecurityThresholdList")]
    public static async Task<TransferSecurityThresholdPageResultDto> TransferSecurityThresholdListAsync(
        [FromServices] IReadOnlyRepository<TransferSecurityThresholdIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetTransferSecurityThresholdChangedDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        var result = queryable.Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList();
        return new TransferSecurityThresholdPageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = objectMapper.Map<List<TransferSecurityThresholdIndex>, List<TransferSecurityThresholdDto>>(result)
        };
    }

    [Name("guardianAddedCAHolderInfo")]
    public static async Task<GuardianAddedCAHolderInfoResultDto> GuardianAddedCAHolderInfoAsync(
        [FromServices] IReadOnlyRepository<CAHolderIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetGuardianAddedCAHolderInfo? dto)
    {
        var queryable = await repository.GetQueryableAsync();

        // mustQuery.Add(q => q.Terms(t => t.Field("Guardians.identifierHash").Terms(dto.LoginGuardianIdentifierHash)));
        if (!dto.LoginGuardianIdentifierHash.IsNullOrEmpty())
        {
            Expression<Func<CAHolderIndex, bool>> expression = t =>
                t.Guardians.Any(f => f.IdentifierHash == dto.LoginGuardianIdentifierHash);

            queryable = queryable.Where(expression);
        }

        var result = queryable.Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList();
        return new GuardianAddedCAHolderInfoResultDto()
        {
            TotalRecordCount = queryable.Count(),
            Data = objectMapper.Map<List<CAHolderIndex>, List<CAHolderInfoDto>>(result)
        };
    }

    [Name("guardianChangeRecordInfo")]
    public static async Task<List<GuardianChangeRecordDto>> GuardianAddedCAHolderInfoAsync(
        [FromServices] IReadOnlyRepository<GuardianChangeRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetGuardianChangeRecordDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (dto.StartBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight >= dto.StartBlockHeight);
        }

        if (dto.EndBlockHeight > 0)
        {
            queryable = queryable.Where(t => t.Metadata.Block.BlockHeight <= dto.EndBlockHeight);
        }

        var result = queryable.OrderBy(t => t.Metadata.Block.BlockHeight).Skip(IndexerConstant.DefaultSkip)
            .Take(IndexerConstant.DefaultLimit).ToList();
        return objectMapper.Map<List<GuardianChangeRecordIndex>, List<GuardianChangeRecordDto>>(result);
    }

    [Name("referralInfoPage")]
    public static async Task<ReferralInfoResultDto> GetReferralInfoPageAsync(
        [FromServices] IReadOnlyRepository<InviteIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetReferralInfoPageDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ProjectCode.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.ProjectCode == dto.ProjectCode);
        }

        if (!dto.MethodName.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.MethodName == dto.MethodName);
        }

        if (!dto.CaHashes.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.CaHashes.Select(caHash =>
                        (Expression<Func<InviteIndex, bool>>)(t => t.CaHash == caHash))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        if (!dto.ReferralCodes.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.ReferralCodes.Select(referralCode =>
                        (Expression<Func<InviteIndex, bool>>)(t => t.ReferralCode == referralCode))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        if (dto.StartTime > 0)
        {
            queryable = queryable.Where(t => t.Timestamp >= dto.StartTime);
        }

        if (dto.EndTime > 0)
        {
            queryable = queryable.Where(t => t.Timestamp <= dto.EndTime);
        }

        var result = queryable.OrderBy(t => t.Timestamp).Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList();
        return new ReferralInfoResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = objectMapper.Map<List<InviteIndex>, List<ReferralInfoDto>>(result)
        };
    }

    [Name("referralInfo")]
    public static async Task<List<ReferralInfoDto>> GetReferralInfoAsync(
        [FromServices] IReadOnlyRepository<InviteIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetReferralInfoDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.CaHashes.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.CaHashes.Select(caHash =>
                        (Expression<Func<InviteIndex, bool>>)(t => t.CaHash == caHash))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        if (!dto.ReferralCodes.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.ReferralCodes.Select(referralCode =>
                        (Expression<Func<InviteIndex, bool>>)(t => t.ReferralCode == referralCode))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        if (dto.StartTime > 0)
        {
            queryable = queryable.Where(t => t.Timestamp >= dto.StartTime);
        }

        if (dto.EndTime > 0)
        {
            queryable = queryable.Where(t => t.Timestamp <= dto.EndTime);
        }

        if (!dto.ProjectCode.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.ProjectCode == dto.ProjectCode);
        }

        if (!dto.MethodNames.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.MethodNames.Select(methodName =>
                        (Expression<Func<InviteIndex, bool>>)(t => t.MethodName == methodName))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        var data = queryable.OrderBy(t => t.Metadata.Block.BlockHeight).Skip(IndexerConstant.DefaultSkip)
            .Take(IndexerConstant.DefaultLimit).ToList();
        return objectMapper.Map<List<InviteIndex>, List<ReferralInfoDto>>(data);
    }

    [Name("autoReceiveTransaction")]
    public static async Task<CAHolderTransactionPageResultDto> GetAutoReceiveTransactionAsync(
        [FromServices] IReadOnlyRepository<CAHolderTransactionIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetAutoReceiveTransactionDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();

        if (!dto.TransferTransactionIds.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.TransferTransactionIds.Select(id =>
                        (Expression<Func<CAHolderTransactionIndex, bool>>)(t =>
                            t.TransferInfo.TransferTransactionId == id))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        var result = queryable.OrderByDescending(t => t.Timestamp).Skip(dto.SkipCount).Take(dto.MaxResultCount)
            .ToList();
        var dataList = objectMapper.Map<List<CAHolderTransactionIndex>, List<CAHolderTransactionDto>>(result);

        return new CAHolderTransactionPageResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = dataList
        };
    }

    [Name("nftItemInfos")]
    public static async Task<List<NFTItemInfoDto>> GetNftItemInfosAsync(
        [FromServices] IReadOnlyRepository<NFTInfoIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetNftItemInfosDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.GetNftItemInfos.IsNullOrEmpty())
        {
            Expression<Func<NFTInfoIndex, bool>> expression = t => true;
            foreach (var info in dto.GetNftItemInfos)
            {
                Expression<Func<NFTInfoIndex, bool>> itemExpression = t => true;
                if (!info.ChainId.IsNullOrEmpty())
                {
                    itemExpression = itemExpression.And(t => t.Metadata.ChainId == info.ChainId);
                }

                if (!info.Symbol.IsNullOrEmpty())
                {
                    itemExpression = itemExpression.And(t => t.Symbol == info.Symbol);
                }

                if (!info.CollectionSymbol.IsNullOrEmpty())
                {
                    itemExpression = itemExpression.And(t => t.CollectionSymbol == info.CollectionSymbol);
                }

                expression = expression.Or(itemExpression);
            }

            queryable = queryable.Where(expression);
        }

        var result = queryable.OrderBy(t => t.Symbol).Skip(dto.SkipCount)
            .Take(dto.MaxResultCount).ToList();
        return objectMapper.Map<List<NFTInfoIndex>, List<NFTItemInfoDto>>(result);
    }

    [Name("caHolderTokenApproved")]
    public static async Task<CAHolderTokenApprovedPageResultDto> CAHolderTokenApprovedAsync(
        [FromServices] IReadOnlyRepository<CAHolderTokenApprovedIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetCAHolderTokenApprovedDto? dto)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!dto.ChainId.IsNullOrEmpty())
        {
            queryable = queryable.Where(t => t.Metadata.ChainId == dto.ChainId);
        }

        if (!dto.CAAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.CAAddresses.Select(address =>
                        (Expression<Func<CAHolderTokenApprovedIndex, bool>>)(t => t.CAAddress == address))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        var result = queryable.Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList();
        return new CAHolderTokenApprovedPageResultDto()
        {
            TotalRecordCount = queryable.Count(),
            Data = objectMapper.Map<List<CAHolderTokenApprovedIndex>, List<CAHolderTokenApprovedDto>>(result)
        };
    }
}