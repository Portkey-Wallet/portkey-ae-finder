using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AeFinder.Sdk.Processor;
using AeFinder.Sdk.Entities;
using AElf;
using AElf.Types;
using Portkey.Contracts.CA;
using Microsoft.Extensions.DependencyInjection;
using PortkeyApp.Entities;
using PortkeyApp.Processors;
using PortkeyApp.Common;
using Shouldly;
using Xunit;
using Google.Protobuf.WellKnownTypes;

namespace PortkeyApp.Tests.Processors;

public class TransferLimitChangedProcessorTests : PortkeyAppTestBase
{
    private readonly AeFinder.Sdk.IRepository<CAHolderIndex> _caHolderRepository;
    private readonly AeFinder.Sdk.IRepository<CAHolderTokenBalanceIndex> _caHolderTokenBalanceRepository;
    private readonly AeFinder.Sdk.IRepository<CAHolderTransactionIndex> _caHolderTransactionRepository;
    private readonly TransferLimitChangedProcessor _processor;

    public TransferLimitChangedProcessorTests()
    {
        _caHolderRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderIndex>>();
        _caHolderTokenBalanceRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderTokenBalanceIndex>>();
        _caHolderTransactionRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderTransactionIndex>>();
        _processor = GetRequiredService<TransferLimitChangedProcessor>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Handle_TransferLimitChanged_Event()
    {
        // Arrange
        var chainId = "AELF";
        var symbol = "ELF";
        var singleLimit = 1000000000;
        var dailyLimit = 10000000000;
        var caHash = HashHelper.ComputeFrom("test_ca_hash");

        var logEvent = new TransferLimitChanged
        {
            CaHash = caHash,
            Symbol = symbol,
            SingleLimit = singleLimit,
            DailyLimit = dailyLimit
        };

        var context = CreateLogEventContext(chainId, logEvent);

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert
        var transactionIndex = await _caHolderTransactionRepository.GetAsync(
            IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId));
        
        transactionIndex.ShouldNotBeNull();
        transactionIndex.Timestamp.ShouldBe(context.Block.BlockTime.ToTimestamp().Seconds);
        transactionIndex.FromAddress.ShouldNotBe(context.Transaction.From);
        transactionIndex.TransactionId.ShouldBe(context.Transaction.TransactionId);
        transactionIndex.Status.ShouldBe(context.Transaction.Status);
        transactionIndex.MethodName.ShouldBe(context.Transaction.MethodName);
    }

    private LogEventContext CreateLogEventContext(string chainId, TransferLimitChanged logEvent)
    {
        return new LogEventContext
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
                From = Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("09da44778f8db2e602fb484334f37df19e221c84c4582ce5b7770ccfbc3ddbef")).ToBase58(),
                To = "CAContract",
                MethodName = "SetTransferLimit",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                }
            }
        };
    }
} 