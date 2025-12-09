using AElf.EntityMapping.Elasticsearch.Linq;
using Nest;

namespace PortkeyApp.Entities;

[NestedAttributes("Guardians")]
public class Guardian
{
    public int Type { get; set; }
    [Keyword] public string VerifierId { get; set; }
    [Keyword] public string IdentifierHash { get; set; }

    [Keyword] public string Salt { get; set; }
    public bool IsLoginGuardian { get; set; }
    [Keyword] public string TransactionId { get; set; }
}