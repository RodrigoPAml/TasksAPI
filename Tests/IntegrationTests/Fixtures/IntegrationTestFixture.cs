using IntegrationTests.Setup;

namespace IntegrationTests.Fixtures
{
    /// <summary>
    /// Fixture collection definition for integration tests
    /// </summary>
    [CollectionDefinition("Integration tests")]
    public class IntegrationTestFixture : ICollectionFixture<WebApiSetup>
    {
    }
}
