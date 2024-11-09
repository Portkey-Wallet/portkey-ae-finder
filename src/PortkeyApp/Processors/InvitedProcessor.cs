using AeFinder.Sdk.Processor;
using Google.Protobuf.WellKnownTypes;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class InvitedProcessor: LogEventProcessorBase<Invited>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(Invited logEvent, LogEventContext context)
    {
        if (logEvent.ProjectCode.IsNullOrEmpty())
        {
            return;
        }

        var indexId = string.Empty;
        if (logEvent.MethodName == CommonConstants.CreateCAHolder)
        {
            indexId = IdGenerateHelper.GetId(logEvent.MethodName, logEvent.ProjectCode, logEvent.CaHash.ToHex());
        }
        else
        {
            indexId = IdGenerateHelper.GetId(logEvent.MethodName, logEvent.ProjectCode, logEvent.ReferralCode,
                logEvent.CaHash.ToHex());
        }

        var inviteIndex = await GetEntityAsync<InviteIndex>(indexId);
        if (inviteIndex != null)
        {
            return;
        }

        inviteIndex = new InviteIndex
        {
            Id = indexId,
            CaHash = logEvent.CaHash.ToHex(),
            ProjectCode = logEvent.ProjectCode,
            ReferralCode = logEvent.ReferralCode,
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
        };

        inviteIndex.MethodName = logEvent.MethodName;
        await SaveEntityAsync(inviteIndex);
    }
    
}