using AeFinder.App.TestBase;
using Microsoft.Extensions.DependencyInjection;
using PortkeyApp.Configs;
using PortkeyApp.Processors;
using Volo.Abp.Modularity;

namespace PortkeyApp;

public class MockPortkeyConfig : IPortkeyConfig
{
    public List<CAHolderTransactionInfo> GetCAHolderTransactionInfos()
    {
        return new List<CAHolderTransactionInfo>
        {
            new CAHolderTransactionInfo
            {
                ChainId = "AELF",
                ContractAddress = "test_ca_address",
                MethodName = "test_method",
                EventNames = new List<string> { "test_event" },
                MultiTransaction = false
            }
        };
    }

    public List<ContractInfo> GetContractInfos()
    {
        return new List<ContractInfo>
        {
            new ContractInfo
            {
                ChainId = "AELF",
                GenesisContractAddress = "test_genesis_address",
                AnotherCAContractAddress = "test_another_ca_address",
                CAContractAddress = "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                TokenContractAddress = "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                NFTContractAddress = "test_nft_address",
                BingoGameContractAddress = "test_bingo_address",
                BeangoTownContractAddress = "test_beango_address",
                ResetManagerInfoHeight = 0
            }
        };
    }

    public InitialInfo GetInitialInfo()
    {
        return new InitialInfo();
    }

    public List<string> GetInscriptions()
    {
        return new List<string>();
    }
}

[DependsOn(
    typeof(AeFinderAppTestBaseModule),
    typeof(PortkeyAppModule))]
public class PortkeyAppTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AeFinderAppEntityOptions>(options => { options.AddTypes<PortkeyAppModule>(); });
        
        // Replace IPortkeyConfig with mock
        var existingService = context.Services.FirstOrDefault(s => s.ServiceType == typeof(IPortkeyConfig));
        if (existingService != null)
        {
            context.Services.Remove(existingService);
        }
        context.Services.AddSingleton<IPortkeyConfig, MockPortkeyConfig>();
        
        // Manually set ConfigConstants with mock data
        var mockConfig = new MockPortkeyConfig();
        ConfigConstants.PortkeyConfig = mockConfig;
        ConfigConstants.ContractInfos = mockConfig.GetContractInfos();
        ConfigConstants.CAHolderTransactionInfos = mockConfig.GetCAHolderTransactionInfos();
        ConfigConstants.InitialInfo = mockConfig.GetInitialInfo();
        ConfigConstants.Inscriptions = mockConfig.GetInscriptions();
        
        // Add your Processors.
        context.Services.AddSingleton<CAHolderCreatedProcessor>();
        context.Services.AddSingleton<CAHolderAccelerateCreationProcessor>();
        context.Services.AddSingleton<CAHolderSyncedProcessor>();
        context.Services.AddSingleton<ManagerUpdatedProcessor>();
        context.Services.AddSingleton<ManagerRemovedProcessor>();
        context.Services.AddSingleton<ManagerAddedProcessor>();
        context.Services.AddSingleton<ManagerSocialRecoveredProcessor>();
        context.Services.AddSingleton<TokenCrossChainReceivedProcessor>();
        context.Services.AddSingleton<TokenCrossChainTransferredProcessor>();
        context.Services.AddSingleton<TokenIssuedProcessor>();
        context.Services.AddSingleton<TokenTransferredProcessor>();
        context.Services.AddSingleton<TokenUnApprovedProcessor>();
        context.Services.AddSingleton<TransactionFeeChargedProcessor>();
        context.Services.AddSingleton<TransferLimitChangedProcessor>();
        context.Services.AddSingleton<TransferSecurityThresholdChangedProcessor>();
        context.Services.AddSingleton<VirtualTransactionCreatedProcessor>();
        context.Services.AddSingleton<LoginGuardianAddedProcessor>();
        context.Services.AddSingleton<LoginGuardianRemovedProcessor>();
        context.Services.AddSingleton<LoginGuardianUnboundProcessor>();
        context.Services.AddSingleton<ManagerApprovedProcessor>();
        context.Services.AddSingleton<PlayedProcessor>();
        context.Services.AddSingleton<RegisteredProcessor>();
        context.Services.AddSingleton<TokenApprovedProcessor>();
        context.Services.AddSingleton<TokenBurnedProcessor>();
        context.Services.AddSingleton<TokenCreatedProcessor>();
        context.Services.AddSingleton<ContractDeployedProcessor>();
        context.Services.AddSingleton<GuardianAddedProcessor>();
        context.Services.AddSingleton<GuardianRemovedProcessor>();
        context.Services.AddSingleton<GuardianUpdatedProcessor>();
        context.Services.AddSingleton<InvitedProcessor>();
        context.Services.AddSingleton<BingoedProcessor>();
    }
}