using AeFinder.Sdk.Processor;
using AElf;
using AElf.CSharp.Core;
using AElf.Types;

namespace PortkeyApp.Processors;

public abstract class CAHolderTransactionEventBase<TEvent> : LogEventProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected Address ConvertVirtualAddressToContractAddress(
        Hash virtualAddress,
        Address contractAddress)
    {
        return Address.FromPublicKey(contractAddress.Value.Concat<byte>((IEnumerable<byte>) virtualAddress.Value.ToByteArray().ComputeHash()).ToArray<byte>());
    }

    protected Dictionary<string, long> GetTransactionFee(Dictionary<string, string> extraProperties)
    {
        return new Dictionary<string, long>();
    }
}