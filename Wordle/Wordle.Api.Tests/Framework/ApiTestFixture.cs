using Xunit;

namespace Wordle.Api.Tests.Framework
{
    [CollectionDefinition("ApiTest")]
    public class GlobalCollectionFixture : ICollectionFixture<ApiTestFixture>
    {
    }

    public class ApiTestFixture
    {
        public TestApplicationClient Factory => new();

        public IJsonClient CreateJsonClient()
        {
            var client = Factory.CreateClient();
            return new JsonClient(client);
        }
    }
}
    
