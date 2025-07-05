using System.Threading.Tasks;
using Xunit;
using Shouldly;
using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using AElf.Types;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using PortkeyApp.Common;
using System.Collections.Generic;
using System;

namespace PortkeyApp.Tests.Processors;

public class TokenTransferredProcessorTests : PortkeyAppTestBase
{
    private readonly TokenTransferredProcessor _processor;
    private readonly IRepository<CAHolderIndex> _caHolderRepository;

    public TokenTransferredProcessorTests()
    {
        _processor = GetRequiredService<TokenTransferredProcessor>();
        _caHolderRepository = GetRequiredService<IRepository<CAHolderIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Update_FromAndTo_TokenBalance()
    {
        // Arrange
        var chainId = "AELF";
        var symbol = "ELF";
        var transferAmount = 50L;
        
        // 创建有效的测试地址
        var fromAddress = Address.FromPublicKey(new byte[33]);
        var toAddress = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
        
        // 创建CAHolderIndex实体
        var fromCAHolder = new CAHolderIndex
        {
            Id = IdGenerateHelper.GetId(chainId, fromAddress.ToBase58()),
            CAHash = "from-hash",
            CAAddress = fromAddress.ToBase58(),
            Creator = "creator-address"
        };
        await GetRequiredService<IRepository<CAHolderIndex>>().AddOrUpdateAsync(fromCAHolder);
        
        var toCAHolder = new CAHolderIndex
        {
            Id = IdGenerateHelper.GetId(chainId, toAddress.ToBase58()),
            CAHash = "to-hash",
            CAAddress = toAddress.ToBase58(),
            Creator = "creator-address"
        };
        await GetRequiredService<IRepository<CAHolderIndex>>().AddOrUpdateAsync(toCAHolder);
        
        // 创建TokenInfoIndex实体  
        var tokenInfoIndex = new TokenInfoIndex
        {
            Id = IdGenerateHelper.GetId(chainId, symbol),
            Symbol = symbol,
            Supply = 1000000,
            TotalSupply = 1000000,
            TokenName = "ELF Token",
            Decimals = 8,
            IsBurnable = true,
            IssueChainId = 1,
            Type = TokenType.Token,
            TokenContractAddress = "token-contract-address"
        };
        await GetRequiredService<IRepository<TokenInfoIndex>>().AddOrUpdateAsync(tokenInfoIndex);
        
        // 创建From地址的TokenBalance
        var fromBalance = new CAHolderTokenBalanceIndex
        {
            Id = IdGenerateHelper.GetId(chainId, fromAddress.ToBase58(), symbol),
            CAAddress = fromAddress.ToBase58(),
            Balance = 200,
            TokenInfo = tokenInfoIndex
        };
        await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>().AddOrUpdateAsync(fromBalance);
        
        // 创建To地址的TokenBalance
        var toBalance = new CAHolderTokenBalanceIndex
        {
            Id = IdGenerateHelper.GetId(chainId, toAddress.ToBase58(), symbol),
            CAAddress = toAddress.ToBase58(),
            Balance = 100,
            TokenInfo = tokenInfoIndex
        };
        await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>().AddOrUpdateAsync(toBalance);
        
        // Act - 测试转账
        var logEvent = new Transferred
        {
            Symbol = symbol,
            Amount = transferAmount,
            From = fromAddress,
            To = toAddress
        };
        var context = new LogEventContext 
        { 
            ChainId = chainId,
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = "test-block-hash",
                BlockHeight = 12345L,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = "test-transaction-id",
                From = fromAddress.ToBase58(),
                To = "test-to-address",
                MethodName = "Transfer",
                Params = "test-params",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                }
            }
        };
        
        await _processor.ProcessAsync(logEvent, context);
        
        // Assert - 验证转账后的余额变化
        var updatedFromBalance = await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, fromAddress.ToBase58(), symbol));
        var updatedToBalance = await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, toAddress.ToBase58(), symbol));
        
        updatedFromBalance.ShouldNotBeNull();
        updatedFromBalance.Balance.ShouldBe(150); // 200 - 50 = 150
        
        updatedToBalance.ShouldNotBeNull();
        updatedToBalance.Balance.ShouldBe(150); // 100 + 50 = 150
    }

    [Fact]
    public async Task ProcessAsync_Should_Handle_NonCAHolder_Transfer()
    {
        // Arrange
        var chainId = "AELF";
        var symbol = "ELF";
        var transferAmount = 30L;
        
        // 创建有效的测试地址（非CAHolder）
        var fromAddress = Address.FromPublicKey(new byte[33]);
        var toAddress = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
        
        // 创建TokenInfoIndex实体  
        var tokenInfoIndex = new TokenInfoIndex
        {
            Id = IdGenerateHelper.GetId(chainId, symbol),
            Symbol = symbol,
            Supply = 1000000,
            TotalSupply = 1000000,
            TokenName = "ELF Token",
            Decimals = 8,
            IsBurnable = true,
            IssueChainId = 1,
            Type = TokenType.Token,
            TokenContractAddress = "token-contract-address"
        };
        await GetRequiredService<IRepository<TokenInfoIndex>>().AddOrUpdateAsync(tokenInfoIndex);
        
        // 创建From地址的TokenBalance（使用原始地址）
        var fromBalance = new CAHolderTokenBalanceIndex
        {
            Id = IdGenerateHelper.GetId(chainId, fromAddress.ToBase58(), symbol),
            CAAddress = fromAddress.ToBase58(),
            Balance = 80,
            TokenInfo = tokenInfoIndex
        };
        await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>().AddOrUpdateAsync(fromBalance);
        
        // 创建To地址的TokenBalance（使用原始地址）
        var toBalance = new CAHolderTokenBalanceIndex
        {
            Id = IdGenerateHelper.GetId(chainId, toAddress.ToBase58(), symbol),
            CAAddress = toAddress.ToBase58(),
            Balance = 20,
            TokenInfo = tokenInfoIndex
        };
        await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>().AddOrUpdateAsync(toBalance);
        
        // Act - 测试非CAHolder转账
        var logEvent = new Transferred
        {
            Symbol = symbol,
            Amount = transferAmount,
            From = fromAddress,
            To = toAddress
        };
        var context = new LogEventContext 
        { 
            ChainId = chainId,
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = "test-block-hash-2",
                BlockHeight = 12346L,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = "test-transaction-id-2",
                From = fromAddress.ToBase58(),
                To = "test-to-address",
                MethodName = "Transfer",
                Params = "test-params",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                }
            }
        };
        
        await _processor.ProcessAsync(logEvent, context);
        
        // Assert - 验证转账后的余额变化
        var updatedFromBalance = await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, fromAddress.ToBase58(), symbol));
        var updatedToBalance = await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, toAddress.ToBase58(), symbol));
        
        updatedFromBalance.ShouldNotBeNull();
        updatedFromBalance.Balance.ShouldBe(50); // 80 - 30 = 50
        
        updatedToBalance.ShouldNotBeNull();
        updatedToBalance.Balance.ShouldBe(50); // 20 + 30 = 50
    }
} 