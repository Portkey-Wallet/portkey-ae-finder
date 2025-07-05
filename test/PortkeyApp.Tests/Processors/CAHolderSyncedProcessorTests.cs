using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AeFinder.Sdk.Processor;
using AeFinder.Sdk.Entities;
using AElf;
using AElf.Types;
using Microsoft.Extensions.DependencyInjection;
using Portkey.Contracts.CA;
using PortkeyApp.Entities;
using PortkeyApp.Processors;
using Shouldly;
using Xunit;
using Volo.Abp.ObjectMapping;
using PortkeyApp.Common;

namespace PortkeyApp.Tests.Processors
{
    public class CAHolderSyncedProcessorTests : PortkeyAppTestBase
    {
        private readonly CAHolderSyncedProcessor _processor;
        private readonly AeFinder.Sdk.IRepository<CAHolderIndex> _caHolderRepository;
        private readonly AeFinder.Sdk.IRepository<CAHolderManagerIndex> _caHolderManagerRepository;

        public CAHolderSyncedProcessorTests()
        {
            _processor = GetRequiredService<CAHolderSyncedProcessor>();
            _caHolderRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderIndex>>();
            _caHolderManagerRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderManagerIndex>>();
        }

        [Fact]
        public async Task ProcessAsync_Should_Create_CAHolderIndex()
        {
            // Arrange
            var chainId = "AELF";
            var caHash = HashHelper.ComputeFrom("test-ca-hash");
            var caAddress = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
            var creator = Address.FromPublicKey(new byte[32] { 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 });
            var createChainId = 9992731;
            
            var logEvent = new CAHolderSynced
            {
                Creator = creator,
                CaHash = caHash,
                CaAddress = caAddress,
                CreateChainId = createChainId,
                ManagerInfosAdded = new ManagerInfoList(),
                ManagerInfosRemoved = new ManagerInfoList(),
                LoginGuardiansAdded = new LoginGuardianList(),
                LoginGuardiansUnbound = new LoginGuardianList()
            };
            
            var context = new LogEventContext
            {
                ChainId = chainId,
                Transaction = new AeFinder.Sdk.Processor.Transaction
                {
                    TransactionId = "test-transaction-id",
                    To = "test-to-address",
                    MethodName = "SyncCAHolder",
                    Params = "test-params",
                    Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                    ExtraProperties = new Dictionary<string, string>()
                },
                Block = new AeFinder.Sdk.Processor.Block
                {
                    BlockHash = "test-block-hash",
                    BlockHeight = 12345L,
                    BlockTime = DateTime.UtcNow
                }
            };

            // Act
            await _processor.ProcessAsync(logEvent, context);

            // Assert
            var caHolderIndex = await _caHolderRepository.GetAsync(IdGenerateHelper.GetId(chainId, caAddress.ToBase58()));
            caHolderIndex.ShouldNotBeNull();
            caHolderIndex.CAHash.ShouldBe(caHash.ToHex());
            caHolderIndex.CAAddress.ShouldBe(caAddress.ToBase58());
            caHolderIndex.Creator.ShouldBe(creator.ToBase58());
        }
    }
}