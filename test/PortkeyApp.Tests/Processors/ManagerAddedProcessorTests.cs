using System.Threading.Tasks;
using Xunit;
using Shouldly;
using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using AElf.Types;
using System;
using System.Collections.Generic;
using Google.Protobuf;
using PortkeyApp.Common;

namespace PortkeyApp.Tests.Processors
{
    public class ManagerAddedProcessorTests : PortkeyAppTestBase
    {
        private readonly ManagerAddedProcessor _processor;
        private readonly IRepository<CAHolderIndex> _caHolderRepository;

        public ManagerAddedProcessorTests()
        {
            _processor = GetRequiredService<ManagerAddedProcessor>();
            _caHolderRepository = GetRequiredService<IRepository<CAHolderIndex>>();
        }

        [Fact]
        public async Task ProcessAsync_Should_Update_CAHolderIndex()
        {
            // Arrange
            var chainId = "AELF";
            var caAddress = Address.FromPublicKey(new byte[33]);
            var caHash = new Hash { Value = ByteString.CopyFromUtf8("test_ca_hash") };
            var manager = Address.FromPublicKey(new byte[33]);
            
            // Pre-create CAHolderIndex
            var caHolderIndex = new CAHolderIndex
            {
                Id = IdGenerateHelper.GetId(chainId, caAddress.ToBase58()),
                CAHash = caHash.ToHex(),
                CAAddress = caAddress.ToBase58(),
                Creator = manager.ToBase58(),
                ManagerInfos = new List<PortkeyApp.Entities.ManagerInfo>(),
                ManagerInfosNew = new List<PortkeyApp.Entities.ManagerInfo>()
            };
            await _caHolderRepository.AddOrUpdateAsync(caHolderIndex);
            
            var logEvent = new ManagerInfoAdded
            {
                CaHash = caHash,
                CaAddress = caAddress,
                Manager = manager,
                Platform = 1
            };
            var context = new LogEventContext
            {
                ChainId = chainId,
                Transaction = new AeFinder.Sdk.Processor.Transaction
                {
                    TransactionId = "test-transaction-id",
                    To = "test-to-address",
                    MethodName = "AddManager",
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
            var updatedIndex = await _caHolderRepository.GetAsync(IdGenerateHelper.GetId(chainId, caAddress.ToBase58()));
            updatedIndex.ShouldNotBeNull();
            var managers = updatedIndex.GetManagerInfos(chainId, 12345L);
            managers.Count.ShouldBe(1);
            managers[0].Address.ShouldBe(manager.ToBase58());
        }
    }
} 