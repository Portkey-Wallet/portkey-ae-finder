namespace PortkeyApp.Configs;

public static partial class ConfigFile
{
  public static string TestNetConfigurationJson = """
{
  "ContractInfos": [
    {
      "ChainId": "AELF",
      "GenesisContractAddress": "pykr77ft9UUKJZLVq15wCH8PinBSjVRQ12sD1Ayq92mKFsJ1i",
      "CAContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "AnotherCAContractAddress": "iupiTuL2cshxB9UNauXNXe9iyCcqka7jCotodcEHGpNXeLzqG",
      "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
      "NFTContractAddress": "2VTusxv6BN4SQDroitnWyLyQHWiwEhdWU76PPiGBqt5VbyF27J"
    },
    {
      "ChainId": "tDVW",
      "GenesisContractAddress": "2UKQnHcQvhBT6X6ULtfnuh3b9PVRvVMEroHHkcK4YfcoH1Z1x2",
      "CAContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "AnotherCAContractAddress": "2WzfRW6KZhAfh3gCZ8Akw4wcti69GUNc1F2sXNa2fgjndv59bE",
      "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
      "BingoGameContractAddress": "2CrjkQeeWYTnH9zFHmpuMtxv8ZTBDmHi31zzdo9SUNjmpxJ82T",
      "BeangoTownContractAddress": "oZHKLeudXJpZeKi55hA5KHgyv7eWBwPL4nCiCChNqPBc6Hb3F",
      "NFTContractAddress": "2ZpYFeE4yWjrcKLBoj1iwbfYnbo9hK7exvfGTdqcq77QSxpzNH"
    }
  ],
  "Inscriptions": [
    "ELEPHANT",
    "WANGHUANBBBBB",
    "SHERRYSUN",
    "TESTGGGR",
    "GGRRTEST-0"
  ],
  "InitialInfo": {
    "TokenInfoList": [
      {
        "ChainId": "AELF",
        "Symbol": "ELF",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 100000000000000000,
        "TokenName": "AElf Token",
        "IssueChainId": 9992731,
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1
      },
      {
        "ChainId": "AELF",
        "Symbol": "USDT",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 6,
        "TotalSupply": 100000000000000000,
        "TokenName": "Tether USD",
        "Issuer": "aeXhTqNwLWxCG6AzxwnYKrPMWRrzZBskW3HWVD9YREMx1rJxG",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "Symbol": "ETH",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 10000000000000000,
        "TokenName": "Ethereum",
        "Issuer": "aeXhTqNwLWxCG6AzxwnYKrPMWRrzZBskW3HWVD9YREMx1rJxG",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "Symbol": "BNB",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 100000000000000000,
        "TokenName": "BNB Token",
        "Issuer": "aeXhTqNwLWxCG6AzxwnYKrPMWRrzZBskW3HWVD9YREMx1rJxG",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "Symbol": "CPU",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "CPU Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "DISK",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "DISK Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "NET",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "NET Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "RAM",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "RAM Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "READ",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "READ Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "LockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "SHARE",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 100000000000000000,
        "TokenName": "SHARE Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "STORAGE",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "STORAGE Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "TRAFFIC",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "TRAFFIC Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "VOTE",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 100000000000000000,
        "TokenName": "VOTE Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "BlockHash": "6e57ae245d81ac0e36a915119b31f11f4585caf7bc8847922db004fe1e63621d",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "WRITE",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "WRITE Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "Symbol": "USDT",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 6,
        "TotalSupply": 100000000000000000,
        "TokenName": "Tether USD",
        "Issuer": "aeXhTqNwLWxCG6AzxwnYKrPMWRrzZBskW3HWVD9YREMx1rJxG",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "Symbol": "ETH",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 10000000000000000,
        "TokenName": "Ethereum",
        "Issuer": "aeXhTqNwLWxCG6AzxwnYKrPMWRrzZBskW3HWVD9YREMx1rJxG",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "Symbol": "BNB",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 100000000000000000,
        "TokenName": "BNB Token",
        "Issuer": "aeXhTqNwLWxCG6AzxwnYKrPMWRrzZBskW3HWVD9YREMx1rJxG",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "CPU",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "CPU Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "DISK",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "DISK Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "ELF",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 100000000000000000,
        "TokenName": "Native Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "NET",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "NET Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "RAM",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "RAM Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "READ",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "READ Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "STORAGE",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "STORAGE Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "TRAFFIC",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "TRAFFIC Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "cfd82f11f7fa149bddf0523c7293da2952734728cf01ef6843431becb0665783",
        "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
        "BlockHeight": 1,
        "Symbol": "WRITE",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 8,
        "TotalSupply": 50000000000000000,
        "TokenName": "WRITE Token",
        "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
        "IsBurnable": true,
        "IssueChainId": 9992731
      }
    ],
    "NFTProtocolInfoList": [
      {
        "ChainId": "AELF",
        "BlockHash": "",
        "PreviousBlockHash": "",
        "BlockHeight": 1,
        "Symbol": "SEED-0",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 0,
        "TotalSupply": 1000000000,
        "TokenName": "SEED",
        "Issuer": "2LNg7aSwwigGWaisUzKjSGdijV9Y6jdtJqrD2PWX3ZQQ2HqsSa",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "AELF",
        "BlockHash": "",
        "PreviousBlockHash": "",
        "BlockHeight": 1,
        "Symbol": "TEFSTVAAOADM-0",
        "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
        "Decimals": 0,
        "TotalSupply": 1,
        "TokenName": "Night",
        "Issuer": "Ko6vD15o3WRy2bgW1vzmKhgTtzfEWrPvUbqVDyf4U1fo4b6Pe",
        "IsBurnable": true,
        "IssueChainId": 1931928
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "",
        "PreviousBlockHash": "",
        "BlockHeight": 1,
        "Symbol": "SEED-0",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 0,
        "TotalSupply": 1000000000,
        "TokenName": "SEED",
        "Issuer": "2LNg7aSwwigGWaisUzKjSGdijV9Y6jdtJqrD2PWX3ZQQ2HqsSa",
        "IsBurnable": true,
        "IssueChainId": 9992731
      },
      {
        "ChainId": "tDVW",
        "BlockHash": "",
        "PreviousBlockHash": "",
        "BlockHeight": 1,
        "Symbol": "TEFSTVAAOADM-0",
        "TokenContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
        "Decimals": 0,
        "TotalSupply": 1,
        "TokenName": "Night",
        "Issuer": "Ko6vD15o3WRy2bgW1vzmKhgTtzfEWrPvUbqVDyf4U1fo4b6Pe",
        "IsBurnable": true,
        "IssueChainId": 1931928
      }
    ],
    "NFTProtocolInfoList": [
    ]
  },
  "CAHolderTransactionInfos": [
    {
      "ChainId": "AELF",
      "ContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
      "MethodName": "Transfer",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
      "MethodName": "CrossChainTransfer",
      "EventNames": [
        "CrossChainTransferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
      "MethodName": ".CrossChainTransfer",
      "EventNames": [
        "CrossChainTransferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
      "MethodName": "CrossChainReceiveToken",
      "EventNames": [
        "CrossChainReceived"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
      "MethodName": "Approve",
      "EventNames": [
        "Approved"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "2VTusxv6BN4SQDroitnWyLyQHWiwEhdWU76PPiGBqt5VbyF27J",
      "MethodName": "Transfer",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "ManagerTransfer",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "AddManagerInfo",
      "EventNames": [
        "ManagerInfoAdded"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "RemoveManagerInfo",
      "EventNames": [
        "ManagerInfoRemoved"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "RemoveOtherManagerInfo",
      "EventNames": [
        "ManagerInfoRemoved"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "UpdateManagerInfos",
      "EventNames": [
        "ManagerInfoUpdated"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "SocialRecovery",
      "EventNames": [
        "ManagerInfoSocialRecovered"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "ManagerForwardCall",
      "EventNames": [
        "CrossChainTransferred",
        ".CrossChainTransferred",
        "CrossChainReceived",
        "Transferred",
        "GuardianAdded",
        "GuardianRemoved",
        "GuardianUpdated",
        "LoginGuardianAdded",
        "LoginGuardianRemoved",
        "Approved"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "233wFn5JbyD4i8R5Me4cW4z6edfFGRn5bpWnGuY8fjR7b2kRsD",
      "MethodName": "ClaimToken",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "AddGuardian",
      "EventNames": [
        "GuardianAdded"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "RemoveGuardian",
      "EventNames": [
        "GuardianRemoved"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "UpdateGuardian",
      "EventNames": [
        "GuardianUpdated"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "UnsetGuardianForLogin",
      "EventNames": [
        "LoginGuardianRemoved"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "SetGuardianForLogin",
      "EventNames": [
        "LoginGuardianAdded"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "CreateCAHolder",
      "EventNames": [
        "CAHolderCreated"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "ManagerApprove",
      "EventNames": [
        "ManagerApproved"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "SetTransferLimit",
      "EventNames": [
        "TransferLimitChanged"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "4xWFvoLvi5anZERDuJvzfMoZsb6WZLATEzqzCVe8sQnCp2XGS",
      "MethodName": "TransferToken",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "ZHCSr8KsQptzYaodJLReDtn4XYWqLtQA4Bs3zB1KaaP6yCSWM",
      "MethodName": "CreateCryptoBox",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "ZHCSr8KsQptzYaodJLReDtn4XYWqLtQA4Bs3zB1KaaP6yCSWM",
      "MethodName": "TransferCryptoBoxes",
      "EventNames": [
        "Transferred"
      ],
      "MultiTransaction": true
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "ZHCSr8KsQptzYaodJLReDtn4XYWqLtQA4Bs3zB1KaaP6yCSWM",
      "MethodName": "RefundCryptoBox",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "iupiTuL2cshxB9UNauXNXe9iyCcqka7jCotodcEHGpNXeLzqG",
      "MethodName": "ManagerTransfer",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "iupiTuL2cshxB9UNauXNXe9iyCcqka7jCotodcEHGpNXeLzqG",
      "MethodName": "ManagerForwardCall",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "AELF",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "ReportPreCrossChainSyncHolderInfo",
      "EventNames": [
        "PreCrossChainSyncHolderInfoCreated"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
      "MethodName": "Transfer",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
      "MethodName": "CrossChainTransfer",
      "EventNames": [
        "CrossChainTransferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
      "MethodName": ".CrossChainTransfer",
      "EventNames": [
        "CrossChainTransferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
      "MethodName": "CrossChainReceiveToken",
      "EventNames": [
        "CrossChainReceived"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx",
      "MethodName": "Approve",
      "EventNames": [
        "Approved"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2ZpYFeE4yWjrcKLBoj1iwbfYnbo9hK7exvfGTdqcq77QSxpzNH",
      "MethodName": "Transfer",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "ManagerTransfer",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "AddManagerInfo",
      "EventNames": [
        "ManagerInfoAdded"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "RemoveManagerInfo",
      "EventNames": [
        "ManagerInfoRemoved"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "RemoveOtherManagerInfo",
      "EventNames": [
        "ManagerInfoRemoved"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "UpdateManagerInfos",
      "EventNames": [
        "ManagerInfoUpdated"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "SocialRecovery",
      "EventNames": [
        "ManagerInfoSocialRecovered"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "ManagerForwardCall",
      "EventNames": [
        "CrossChainTransferred",
        ".CrossChainTransferred",
        "CrossChainReceived",
        "Transferred",
        "GuardianAdded",
        "GuardianRemoved",
        "GuardianUpdated",
        "LoginGuardianAdded",
        "LoginGuardianRemoved",
        "Approved",
        "Played",
        "Bingoed",
        "Registered"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "CreateCAHolder",
      "EventNames": [
        "CAHolderCreated"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "AddGuardian",
      "EventNames": [
        "GuardianAdded"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "RemoveGuardian",
      "EventNames": [
        "GuardianRemoved"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "UpdateGuardian",
      "EventNames": [
        "GuardianUpdated"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "UnsetGuardianForLogin",
      "EventNames": [
        "LoginGuardianRemoved"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "SetGuardianForLogin",
      "EventNames": [
        "LoginGuardianAdded"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "ManagerApprove",
      "EventNames": [
        "ManagerApproved"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "SetTransferLimit",
      "EventNames": [
        "TransferLimitChanged"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2AgU8BfyKyrxUrmskVCUukw63Wk96MVfVoJzDDbwKszafioCN1",
      "MethodName": "TransferToken",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2UTqYoguUbCYyAmmHPPXBMUfNvHwtzHSx4Uet3pnXLkoJCbXmm",
      "MethodName": "CreateCryptoBox",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2UTqYoguUbCYyAmmHPPXBMUfNvHwtzHSx4Uet3pnXLkoJCbXmm",
      "MethodName": "TransferCryptoBoxes",
      "EventNames": [
        "Transferred"
      ],
      "MultiTransaction": true
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2UTqYoguUbCYyAmmHPPXBMUfNvHwtzHSx4Uet3pnXLkoJCbXmm",
      "MethodName": "RefundCryptoBox",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2WzfRW6KZhAfh3gCZ8Akw4wcti69GUNc1F2sXNa2fgjndv59bE",
      "MethodName": "ManagerTransfer",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2WzfRW6KZhAfh3gCZ8Akw4wcti69GUNc1F2sXNa2fgjndv59bE",
      "MethodName": "ManagerForwardCall",
      "EventNames": [
        "Transferred"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "238X6iw1j8YKcHvkDYVtYVbuYk2gJnK8UoNpVCtssynSpVC8hb",
      "MethodName": "ReportPreCrossChainSyncHolderInfo",
      "EventNames": [
        "PreCrossChainSyncHolderInfoCreated"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2CrjkQeeWYTnH9zFHmpuMtxv8ZTBDmHi31zzdo9SUNjmpxJ82T",
      "MethodName": "Bingo",
      "EventNames": [
        "Bingoed"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2CrjkQeeWYTnH9zFHmpuMtxv8ZTBDmHi31zzdo9SUNjmpxJ82T",
      "MethodName": "Play",
      "EventNames": [
        "Played"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "2CrjkQeeWYTnH9zFHmpuMtxv8ZTBDmHi31zzdo9SUNjmpxJ82T",
      "MethodName": "Register",
      "EventNames": [
        "Registered"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "oZHKLeudXJpZeKi55hA5KHgyv7eWBwPL4nCiCChNqPBc6Hb3F",
      "MethodName": "Bingo",
      "EventNames": [
        "Bingoed"
      ]
    },
    {
      "ChainId": "tDVW",
      "ContractAddress": "oZHKLeudXJpZeKi55hA5KHgyv7eWBwPL4nCiCChNqPBc6Hb3F",
      "MethodName": "Play",
      "EventNames": [
        "Played"
      ]
    }
  ]
}

""";
}