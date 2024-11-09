using AeFinder.Sdk;

namespace PortkeyApp.GraphQL;

public class AppSchema : AppSchema<Query>
{
    public AppSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}