using AeFinder.Sdk.Processor;
using PortkeyApp.GraphQL;
using PortkeyApp.Processors;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace PortkeyApp;

public class PortkeyAppModule: AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<PortkeyAppModule>(); });
        context.Services.AddSingleton<ISchema, AppSchema>();
        
        // Add your LogEventProcessor implementation.
        context.Services.AddTransient<ILogEventProcessor, BingoedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, CAHolderAccelerateCreationProcessor>();
        context.Services.AddTransient<ILogEventProcessor, CAHolderCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, CAHolderSyncedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ContractDeployedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, GuardianAddedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, GuardianRemovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, GuardianUpdatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, InvitedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, LoginGuardianAddedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, LoginGuardianRemovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, LoginGuardianUnboundProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ManagerAddedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ManagerApprovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ManagerRemovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ManagerSocialRecoveredProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ManagerUpdatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, PlayedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, RegisteredProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TokenApprovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TokenBurnedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TokenCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TokenCrossChainReceivedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TokenCrossChainTransferredProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TokenIssuedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TokenTransferredProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TokenUnApprovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TransactionFeeChargedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TransferLimitChangedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TransferSecurityThresholdChangedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, VirtualTransactionCreatedProcessor>();
        
    }
}