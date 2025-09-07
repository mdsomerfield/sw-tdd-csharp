using Xunit;

namespace Wordle.E2E.Tests.Framework;

[CollectionDefinition("E2ETest")]
public class E2ETestCollection : ICollectionFixture<RealServerTestFixture>
{
}