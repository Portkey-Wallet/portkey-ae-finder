using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using AElf.Contracts.NFT;
using PortkeyApp.Entities;
using PortkeyApp.GraphQL;
using AutoMapper;
using PortkeyApp.Configs;
using Volo.Abp.AutoMapper;
using TransactionFee = PortkeyApp.GraphQL.TransactionFee;

namespace PortkeyApp;

public class PortkeyAppProfile : Profile
{
    public PortkeyAppProfile()
    {
        CreateMap<MyEntity, MyEntityDto>();
        CreateMap<TokenCreated, TokenInfoIndex>().Ignore(t => t.Issuer);
        CreateMap<TokenCreated, NFTCollectionInfoIndex>().Ignore(t => t.Issuer);
        //CreateMap<Provider.TokenInfoDto, NFTCollectionInfoIndex>();
        CreateMap<TokenCreated, NFTInfoIndex>().Ignore(t => t.Issuer);
        // CreateMap<Provider.TokenInfoDto, NFTInfoIndex>();
        // CreateMap<Provider.TokenInfoDto, TokenInfoIndex>();

        // CreateMap<LogEventContext, TokenInfoIndex>();
        // CreateMap<LogEventContext, NFTCollectionInfoIndex>();
        // CreateMap<LogEventContext, NFTInfoIndex>();
        // CreateMap<LogEventContext, LoginGuardianIndex>();
        // CreateMap<LogEventContext, LoginGuardianChangeRecordIndex>();
        //
        // CreateMap<LogEventContext, CAHolderNFTCollectionBalanceIndex>();
        // CreateMap<LogEventContext, CAHolderNFTBalanceIndex>();
        // CreateMap<LogEventContext, CAHolderTokenBalanceIndex>();
        //
        // CreateMap<LogEventContext, CompatibleCrossChainTransferIndex>();
        // CreateMap<LogEventContext, CAHolderTransactionAddressIndex>();
        // CreateMap<LogEventContext, CAHolderManagerChangeRecordIndex>();
        // CreateMap<LogEventContext, CAHolderSearchTokenNFTIndex>();
        //
        // CreateMap<LogEventContext, TransactionFeeChangedIndex>();
        CreateMap<TransactionFeeCharged, TransactionFeeChangedIndex>();
        // CreateMap<LogEventContext, GuardianChangeRecordIndex>();
        // CreateMap<LogEventContext, TransferLimitIndex>();
        // CreateMap<LogEventContext, TransferSecurityThresholdIndex>();
        // CreateMap<LogEventContext, TransactionFeeChangedIndex>();
        CreateMap<TransactionFeeCharged, TransactionFeeChangedIndex>();

        CreateMap<TokenInfo, TokenInfoIndex>()
            .ForMember(t => t.Issuer, m => m.MapFrom(f => f.Issuer.ToBase58()));
        CreateMap<TokenInfo, NFTCollectionInfoIndex>()
            .ForMember(t => t.Issuer, m => m.MapFrom(f => f.Issuer.ToBase58()));
        CreateMap<TokenInfo, NFTInfoIndex>()
            .ForMember(t => t.Issuer, m => m.MapFrom(f => f.Issuer.ToBase58()));

        CreateMap<TokenInfoIndex, TokenInfoDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId))
            .ForMember(t => t.BlockHash, m => m.MapFrom(f => f.Metadata.Block.BlockHash))
            .ForMember(t => t.BlockHeight, m => m.MapFrom(f => f.Metadata.Block.BlockHeight));

        CreateMap<TokenInfoIndex, TokenSearchInfo>();
        CreateMap<TokenInfoIndex, TokenSearchInfoDto>();
        CreateMap<TokenSearchInfo, TokenInfoDto>();
        CreateMap<TokenTransferInfo, TokenTransferInfoDto>();

        CreateMap<NFTProtocolCreated, NFTCollectionInfoIndex>();
        CreateMap<NFTProtocolInfo, NFTCollectionInfoIndex>();
        CreateMap<NFTCollectionInfoIndex, NFTProtocolInfoDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId))
            .ForMember(t => t.BlockHash, m => m.MapFrom(f => f.Metadata.Block.BlockHash))
            .ForMember(t => t.BlockHeight, m => m.MapFrom(f => f.Metadata.Block.BlockHeight));

        CreateMap<NFTCollectionInfoIndex, NFTProtocol>();
        CreateMap<NFTCollectionInfoIndex, NFTCollectionDto>();
        CreateMap<NFTMinted, NFTInfoIndex>();
        CreateMap<NFTMinted, CAHolderNFTBalanceIndex>();
        CreateMap<NFTMinted, CAHolderNFTCollectionBalanceIndex>();
        CreateMap<NFTInfoIndex, NFTItemInfo>();
        CreateMap<NFTInfoIndex, NFTItemInfoDto>();
        CreateMap<NFTInfoIndex, NFTSearchInfo>();
        CreateMap<NFTInfoIndex, CAHolderNFTBalanceIndex>();
        CreateMap<NFTItemInfo, NFTItemInfoDto>();
        CreateMap<NFTProtocol, NFTCollectionDto>();
        CreateMap<NFTSearchInfo, NFTItemInfoDto>();

        CreateMap<CAHolderTransactionIndex, CAHolderTransactionDto>()
            .ForMember(c => c.TransactionFees, opt => opt.MapFrom<TransactionFeeResolver>())
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId))
            .ForMember(t => t.BlockHash, m => m.MapFrom(f => f.Metadata.Block.BlockHash))
            .ForMember(t => t.BlockHeight, m => m.MapFrom(f => f.Metadata.Block.BlockHeight));

        CreateMap<CAHolderIndex, CAHolderManagerDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));

        CreateMap<CAHolderNFTBalanceIndex, CAHolderNFTBalanceInfoDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));
        CreateMap<CAHolderNFTCollectionBalanceIndex, CAHolderNFTCollectionBalanceInfoDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));

        CreateMap<CAHolderTokenBalanceIndex, CAHolderTokenBalanceDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));
        CreateMap<CAHolderTransactionAddressIndex, CAHolderTransactionAddressDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));
        CreateMap<CAHolderManagerChangeRecordIndex, CAHolderManagerChangeRecordDto>()
            .ForMember(t => t.BlockHash, m => m.MapFrom(f => f.Metadata.Block.BlockHash))
            .ForMember(t => t.BlockHeight, m => m.MapFrom(f => f.Metadata.Block.BlockHeight));
        CreateMap<CAHolderSearchTokenNFTIndex, CAHolderSearchTokenNFTDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));

        CreateMap<LoginGuardianIndex, LoginGuardianDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));
        CreateMap<LoginGuardianChangeRecordIndex, LoginGuardianChangeRecordDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId))
            .ForMember(t => t.BlockHash, m => m.MapFrom(f => f.Metadata.Block.BlockHash))
            .ForMember(t => t.BlockHeight, m => m.MapFrom(f => f.Metadata.Block.BlockHeight));
        CreateMap<Guardian, GuardianDto>();
        CreateMap<BingoGameIndex, BingoInfo>();
        CreateMap<BingoGameStaticsIndex, BingoStatics>();
        // CreateMap<LogEventContext, BingoGameIndex>();
        // CreateMap<LogEventContext, BingoGameStaticsIndex>();
        // CreateMap<LogEventContext, CAHolderTokenApprovedIndex>();
        CreateMap<CAHolderTokenApprovedIndex, CAHolderTokenApprovedDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));
        CreateMap<CAHolderIndex, CAHolderInfoDto>().ForMember(d => d.GuardianList,
                opt => opt.MapFrom(e =>
                    e.Guardians.IsNullOrEmpty() ? null : new GuardianList { Guardians = e.Guardians }))
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));

        CreateMap<Portkey.Contracts.CA.Guardian, Guardian>()
            .ForMember(d => d.IdentifierHash, opt => opt.MapFrom(e => e.IdentifierHash.ToHex()))
            .ForMember(d => d.VerifierId, opt => opt.MapFrom(e => e.VerifierId.ToHex()))
            .ForMember(d => d.Type, opt => opt.MapFrom(e => (int)e.Type));
        CreateMap<TransferLimitIndex, CAHolderTransferlimitDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId));
        CreateMap<TransferSecurityThresholdIndex, TransferSecurityThresholdDto>();
        //CreateMap<LogEventContext, TransferLimitIndex>();
        CreateMap<ManagerApprovedIndex, CAHolderManagerApprovedDto>()
            .ForMember(t => t.ChainId, m => m.MapFrom(f => f.Metadata.ChainId))
            .ForMember(t => t.BlockHeight, m => m.MapFrom(f => f.Metadata.Block.BlockHeight));
        //CreateMap<LogEventContext, ManagerApprovedIndex>();
        CreateMap<GuardianChangeRecordIndex, GuardianChangeRecordDto>()
            .ForMember(t => t.BlockHash, m => m.MapFrom(f => f.Metadata.Block.BlockHash))
            .ForMember(t => t.BlockHeight, m => m.MapFrom(f => f.Metadata.Block.BlockHeight));
        //CreateMap<LogEventContext, InviteIndex>();
        CreateMap<InviteIndex, ReferralInfoDto>();
        CreateMap<NftExternalInfo, NFTCollectionInfoIndex>();
        CreateMap<NftExternalInfo, NFTInfoIndex>();
        
        CreateMap<TokenInitInfo, TokenInfoIndex>();
        CreateMap<NFTProtocolInitInfo, NFTCollectionInfoIndex>();
    }
}

public class
    TransactionFeeResolver : IValueResolver<CAHolderTransactionIndex, CAHolderTransactionDto, List<TransactionFee>>
{
    public List<TransactionFee> Resolve(CAHolderTransactionIndex source, CAHolderTransactionDto destination,
        List<TransactionFee> destMember,
        ResolutionContext context)
    {
        var list = new List<TransactionFee>();
        foreach (var (symbol, amount) in source.TransactionFee)
        {
            list.Add(new TransactionFee
            {
                Amount = amount,
                Symbol = symbol
            });
        }

        return list;
    }
}