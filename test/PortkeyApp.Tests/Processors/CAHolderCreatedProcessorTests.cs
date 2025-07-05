using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using AElf.Types;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using PortkeyApp.Common;
using AElf;

namespace PortkeyApp.Tests.Processors;

public class CAHolderCreatedProcessorTests : PortkeyAppTestBase
{
    private readonly CAHolderCreatedProcessor _processor;
    private readonly IRepository<CAHolderIndex> _caHolderRepository;

    public CAHolderCreatedProcessorTests()
    {
        _processor = GetRequiredService<CAHolderCreatedProcessor>();
        _caHolderRepository = GetRequiredService<IRepository<CAHolderIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Create_CAHolderIndex()
    {
        // Arrange
        var chainId = "AELF";
        var caAddress = Address.FromPublicKey(new byte[33]);
        var manager = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
        var creator = Address.FromPublicKey(new byte[32] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
        var caHash = new global::AElf.Types.Hash { Value = Google.Protobuf.ByteString.CopyFromUtf8("test-ca-hash") };
        
        var logEvent = new CAHolderCreated
        {
            CaAddress = caAddress,
            CaHash = caHash,
            Manager = manager,
            Creator = creator,
            ExtraData = "test-extra-data",
            Platform = 1
        };
        var context = new LogEventContext
        {
            ChainId = chainId,
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = HashHelper.ComputeFrom("block_hash").ToHex(),
                BlockHeight = 100,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = HashHelper.ComputeFrom("transaction_id").ToHex(),
                From = "test-from-address",
                To = "CAContract",
                MethodName = "CreateCAHolder",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                },
                Params = "test-params"
            }
        };

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert - 验证CAHolderIndex是否正确创建
        var caHolderIndex = await _caHolderRepository.GetAsync(
            IdGenerateHelper.GetId(chainId, caAddress.ToBase58()));
        
        caHolderIndex.ShouldNotBeNull();
        caHolderIndex.CAAddress.ShouldBe(caAddress.ToBase58());
        caHolderIndex.CAHash.ShouldBe(caHash.ToHex());
        caHolderIndex.Creator.ShouldBe(creator.ToBase58());
        caHolderIndex.OriginChainId.ShouldBe(chainId);
        // Check if ManagerInfosNew is set correctly (BlockHeight < ResetManagerInfoHeight)
        caHolderIndex.ManagerInfosNew.ShouldNotBeNull();
        caHolderIndex.ManagerInfosNew.Count.ShouldBe(1);
        caHolderIndex.ManagerInfosNew[0].Address.ShouldBe(manager.ToBase58());
        caHolderIndex.ManagerInfosNew[0].ExtraData.ShouldBe("test-extra-data");
        // ManagerInfos should be null since BlockHeight < ResetManagerInfoHeight
        caHolderIndex.ManagerInfos.ShouldBeNull();
        
        // 验证CAHolderManagerIndex是否正确创建
        var managerIndex = await GetRequiredService<IRepository<CAHolderManagerIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, manager.ToBase58()));
        
        managerIndex.ShouldNotBeNull();
        managerIndex.Manager.ShouldBe(manager.ToBase58());
        managerIndex.CAAddresses.ShouldContain(caAddress.ToBase58());
    }

    [Fact]
    public async Task ProcessAsync_Should_Not_Create_Duplicate_CAHolderIndex()
    {
        // Arrange
        var chainId = "AELF";
        var caAddress = Address.FromPublicKey(new byte[33]);
        var manager = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
        var creator = Address.FromPublicKey(new byte[32] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
        var caHash = new global::AElf.Types.Hash { Value = Google.Protobuf.ByteString.CopyFromUtf8("test-ca-hash") };
        
        // 预先创建CAHolderIndex
        var existingCAHolder = new CAHolderIndex
        {
            Id = IdGenerateHelper.GetId(chainId, caAddress.ToBase58()),
            CAAddress = caAddress.ToBase58(),
            CAHash = caHash.ToHex(),
            Creator = creator.ToBase58(),
            OriginChainId = chainId
        };
        await _caHolderRepository.AddOrUpdateAsync(existingCAHolder);
        
        var logEvent = new CAHolderCreated
        {
            CaAddress = caAddress,
            CaHash = caHash,
            Manager = manager,
            Creator = creator,
            ExtraData = "test-extra-data",
            Platform = 1
        };
        var context = new LogEventContext
        {
            ChainId = chainId,
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = HashHelper.ComputeFrom("block_hash2").ToHex(),
                BlockHeight = 101,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = HashHelper.ComputeFrom("transaction_id2").ToHex(),
                From = "test-from-address",
                To = "CAContract",
                MethodName = "CreateCAHolder",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                },
                Params = "test-params"
            }
        };

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert - 验证CAHolderIndex没有被重复创建
        var caHolderIndex = await _caHolderRepository.GetAsync(
            IdGenerateHelper.GetId(chainId, caAddress.ToBase58()));
        
        caHolderIndex.ShouldNotBeNull();
        caHolderIndex.CAAddress.ShouldBe(caAddress.ToBase58());
        // 验证ManagerInfos没有被设置（因为已存在）
        caHolderIndex.ManagerInfos.ShouldBeNull();
    }

    [Fact]
    public async Task ProcessAsync_Should_Update_Existing_CAHolderManagerIndex()
    {
        // Arrange
        var chainId = "AELF";
        var caAddress1 = Address.FromPublicKey(new byte[33]);
        var caAddress2 = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
        var manager = Address.FromPublicKey(new byte[32] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
        var creator = Address.FromPublicKey(new byte[32] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 });
        var caHash = new global::AElf.Types.Hash { Value = Google.Protobuf.ByteString.CopyFromUtf8("test-ca-hash") };
        
        // 预先创建CAHolderManagerIndex
        var existingManagerIndex = new CAHolderManagerIndex
        {
            Id = IdGenerateHelper.GetId(chainId, manager.ToBase58()),
            Manager = manager.ToBase58(),
            CAAddresses = new List<string> { caAddress1.ToBase58() }
        };
        await GetRequiredService<IRepository<CAHolderManagerIndex>>().AddOrUpdateAsync(existingManagerIndex);
        
        var logEvent = new CAHolderCreated
        {
            CaAddress = caAddress2,
            CaHash = caHash,
            Manager = manager,
            Creator = creator,
            ExtraData = "test-extra-data",
            Platform = 1
        };
        var context = new LogEventContext
        {
            ChainId = chainId,
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = HashHelper.ComputeFrom("block_hash3").ToHex(),
                BlockHeight = 102,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = HashHelper.ComputeFrom("transaction_id3").ToHex(),
                From = "test-from-address",
                To = "CAContract",
                MethodName = "CreateCAHolder",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                },
                Params = "test-params"
            }
        };

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert - 验证CAHolderManagerIndex被正确更新
        var managerIndex = await GetRequiredService<IRepository<CAHolderManagerIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, manager.ToBase58()));
        
        managerIndex.ShouldNotBeNull();
        managerIndex.Manager.ShouldBe(manager.ToBase58());
        managerIndex.CAAddresses.Count.ShouldBe(2);
        managerIndex.CAAddresses.ShouldContain(caAddress1.ToBase58());
        managerIndex.CAAddresses.ShouldContain(caAddress2.ToBase58());
    }
} 