using AElf;
using AElf.Types;

namespace PortkeyApp.Common;

public static class StringExtensions
{
    public static Address ToAddress(this string? address) => address == null ? Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("09da44778f8db2e602fb484334f37df19e221c84c4582ce5b7770ccfbc3ddbef")) : Address.FromBase58(address);
}