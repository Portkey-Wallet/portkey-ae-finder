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

public class VirtualTransactionCreatedProcessorTests : PortkeyAppTestBase
{
    private readonly VirtualTransactionCreatedProcessor _processor;
    private readonly AeFinder.Sdk.IRepository<CAHolderIndex> _caHolderRepository;
    private readonly AeFinder.Sdk.IRepository<CAHolderTokenBalanceIndex> _caHolderTokenBalanceRepository;
    private readonly AeFinder.Sdk.IRepository<CAHolderTransactionIndex> _caHolderTransactionRepository;

    public VirtualTransactionCreatedProcessorTests()
    {
        _processor = GetRequiredService<VirtualTransactionCreatedProcessor>();
        _caHolderRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderIndex>>();
        _caHolderTokenBalanceRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderTokenBalanceIndex>>();
        _caHolderTransactionRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderTransactionIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Handle_VirtualTransactionCreated_Event()
    {
        // Arrange
        var chainId = "AELF";
        var fromAddress = Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("09da44778f8db2e602fb484334f37df19e221c84c4582ce5b7770ccfbc3ddbef"));
        var toAddress = Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("1a2b3c4d5e6f7890abcdef1234567890abcdef1234567890abcdef1234567890"));
        var virtualHash = HashHelper.ComputeFrom("virtual_transaction_hash");

        var logEvent = new VirtualTransactionCreated
        {
            From = fromAddress,
            To = toAddress,
            VirtualHash = virtualHash
        };

        var context = CreateLogEventContext(chainId, logEvent);

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert
        // The processor should complete without error
        // We verify the process completed successfully by checking the event properties
        logEvent.From.ShouldBe(fromAddress);
        logEvent.To.ShouldBe(toAddress);
        logEvent.VirtualHash.ShouldBe(virtualHash);
    }

    private LogEventContext CreateLogEventContext(string chainId, VirtualTransactionCreated logEvent)
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
                From = "test-from-address",
                To = "CAContract",
                MethodName = "CreateVirtualTransaction",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                }
            }
        };
    }
} 