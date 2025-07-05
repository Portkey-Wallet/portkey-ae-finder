using System.Threading.Tasks;
using Xunit;
using Shouldly;
using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using AeFinder.Sdk.Entities;
using AElf.Types;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Portkey.Contracts.CA;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using PortkeyApp.Common;
using System.Collections.Generic;
using System;
using Volo.Abp.ObjectMapping;
using AElf;

namespace PortkeyApp.Tests.Processors
{
    public class CAHolderAccelerateCreationProcessorTests : PortkeyAppTestBase
    {
        private readonly CAHolderAccelerateCreationProcessor _processor;
        private readonly IRepository<CAHolderIndex> _caHolderRepository;
        private readonly IRepository<CAHolderManagerIndex> _caHolderManagerRepository;

        public CAHolderAccelerateCreationProcessorTests()
        {
            _processor = GetRequiredService<CAHolderAccelerateCreationProcessor>();
            _caHolderRepository = GetRequiredService<IRepository<CAHolderIndex>>();
            _caHolderManagerRepository = GetRequiredService<IRepository<CAHolderManagerIndex>>();
        }

        [Fact]
        public async Task ProcessAsync_Should_Create_CAHolderIndex()
        {
            // Arrange
            var chainId = "AELF";
            var caHash = HashHelper.ComputeFrom("test-ca-hash");
            var caAddress = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
            var creator = Address.FromPublicKey(new byte[32] { 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 });
            var manager = Address.FromPublicKey(new byte[32] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 26, 27, 28, 29, 30, 31, 32 });
            var createChainId = 9992731;
            var extraData = "test-extra-data";
            
            var logEvent = new PreCrossChainSyncHolderInfoCreated
            {
                CaHash = caHash,
                CaAddress = caAddress,
                Creator = creator,
                Manager = manager,
                CreateChainId = createChainId,
                ExtraData = extraData
            };
            
            var context = new LogEventContext
            {
                ChainId = chainId,
                Transaction = new AeFinder.Sdk.Processor.Transaction
                {
                    TransactionId = "test-transaction-id",
                    To = "test-to-address",
                    MethodName = "CreateCAHolder",
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
            caHolderIndex.OriginChainId.ShouldBe(ChainHelper.ConvertChainIdToBase58(createChainId));
            
            var managers = caHolderIndex.GetManagerInfos(chainId, 12345L);
            managers.Count.ShouldBe(1);
            managers[0].Address.ShouldBe(manager.ToBase58());
            managers[0].ExtraData.ShouldBe(extraData);
            
            // Check CAHolderManagerIndex
            var managerIndex = await _caHolderManagerRepository.GetAsync(IdGenerateHelper.GetId(chainId, manager.ToBase58()));
            managerIndex.ShouldNotBeNull();
            managerIndex.Manager.ShouldBe(manager.ToBase58());
            managerIndex.CAAddresses.ShouldContain(caAddress.ToBase58());
        }
    }
} 