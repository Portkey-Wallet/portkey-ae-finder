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

public class TokenCrossChainReceivedProcessorTests : PortkeyAppTestBase
{
    private readonly AeFinder.Sdk.IRepository<CAHolderIndex> _caHolderRepository;
    private readonly AeFinder.Sdk.IRepository<CAHolderTokenBalanceIndex> _caHolderTokenBalanceRepository;
    private readonly AeFinder.Sdk.IRepository<CAHolderTransactionIndex> _caHolderTransactionRepository;
    private readonly TokenCrossChainReceivedProcessor _processor;

    public TokenCrossChainReceivedProcessorTests()
    {
        _caHolderRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderIndex>>();
        _caHolderTokenBalanceRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderTokenBalanceIndex>>();
        _caHolderTransactionRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderTransactionIndex>>();
        _processor = GetRequiredService<TokenCrossChainReceivedProcessor>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Handle_CrossChainReceived_Event()
    {
        // Arrange
        var chainId = "AELF";
        var symbol = "ELF";
        var amount = 1000000000;
        var fromChainId = 1866392;
        var fromAddress = Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("AAA".PadLeft(64, '0')));
        var toAddress = Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("BBB".PadLeft(64, '0')));

        // Pre-create a TokenInfoIndex to avoid blockchain service call
        var tokenInfoIndex = new TokenInfoIndex
        {
            Id = IdGenerateHelper.GetId(chainId, symbol),
            Symbol = symbol,
            TokenName = "ELF Token",
            TotalSupply = 1000000000,
            Decimals = 8,
            Type = TokenType.Token,
            TokenContractAddress = "TokenContract"
        };
        await GetRequiredService<AeFinder.Sdk.IRepository<TokenInfoIndex>>().AddOrUpdateAsync(tokenInfoIndex);

        // Create a CAHolderIndex for the toAddress to ensure balance modification
        var caHolderIndex = new CAHolderIndex
        {
            Id = IdGenerateHelper.GetId(chainId, toAddress.ToBase58()),
            CAAddress = toAddress.ToBase58(),
            CAHash = "test-hash",
            Creator = fromAddress.ToBase58(),
            OriginChainId = chainId
        };
        await GetRequiredService<AeFinder.Sdk.IRepository<CAHolderIndex>>().AddOrUpdateAsync(caHolderIndex);

        var logEvent = new CrossChainReceived
        {
            From = fromAddress,
            To = toAddress,
            Symbol = symbol,
            Amount = amount,
            FromChainId = fromChainId
        };

        var context = CreateLogEventContext(chainId, logEvent);

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert - Check if balance was modified correctly
        var balanceIndex = await GetRequiredService<AeFinder.Sdk.IRepository<CAHolderTokenBalanceIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, toAddress.ToBase58(), symbol));
        
        balanceIndex.ShouldNotBeNull();
        balanceIndex.Balance.ShouldBe(amount);
        balanceIndex.CAAddress.ShouldBe(toAddress.ToBase58());
        balanceIndex.TokenInfo.Symbol.ShouldBe(symbol);
    }

    private LogEventContext CreateLogEventContext(string chainId, CrossChainReceived logEvent)
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
                To = "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE", // Token contract address
                MethodName = "CrossChainReceiveToken", // Method name that matches config
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                },
                Params = "test-params"
            }
        };
    }
} 