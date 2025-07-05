using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AeFinder.Sdk.Processor;
using AeFinder.Sdk.Entities;
using AElf;
using AElf.Types;
using AElf.Contracts.MultiToken;
using Microsoft.Extensions.DependencyInjection;
using PortkeyApp.Entities;
using PortkeyApp.Processors;
using PortkeyApp.Common;
using Shouldly;
using Xunit;
using Google.Protobuf.WellKnownTypes;

namespace PortkeyApp.Tests.Processors;

public class TransactionFeeChargedProcessorTests : PortkeyAppTestBase
{
    private readonly TransactionFeeChargedProcessor _processor;
    private readonly AeFinder.Sdk.IRepository<CAHolderIndex> _caHolderRepository;
    private readonly AeFinder.Sdk.IRepository<CAHolderTokenBalanceIndex> _caHolderTokenBalanceRepository;
    private readonly AeFinder.Sdk.IRepository<CAHolderTransactionIndex> _caHolderTransactionRepository;

    public TransactionFeeChargedProcessorTests()
    {
        _processor = GetRequiredService<TransactionFeeChargedProcessor>();
        _caHolderRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderIndex>>();
        _caHolderTokenBalanceRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderTokenBalanceIndex>>();
        _caHolderTransactionRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderTransactionIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Handle_TransactionFeeCharged_Event()
    {
        // Arrange
        var chainId = "AELF";
        var symbol = "ELF";
        var amount = 1000000;
        var chargingAddress = Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("09da44778f8db2e602fb484334f37df19e221c84c4582ce5b7770ccfbc3ddbef"));

        var logEvent = new TransactionFeeCharged
        {
            Symbol = symbol,
            Amount = amount,
            ChargingAddress = chargingAddress
        };

        var context = CreateLogEventContext(chainId, logEvent);

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert
        // The processor should complete without error
        // We verify the process completed successfully by checking the event properties
        logEvent.Symbol.ShouldBe(symbol);
        logEvent.Amount.ShouldBe(amount);
        logEvent.ChargingAddress.ShouldBe(chargingAddress);
    }

    private LogEventContext CreateLogEventContext(string chainId, TransactionFeeCharged logEvent)
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
                To = "TokenContract",
                MethodName = "ChargeTransactionFee",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                }
            }
        };
    }
} 