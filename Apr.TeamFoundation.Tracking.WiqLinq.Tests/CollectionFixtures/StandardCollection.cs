using Xunit;

namespace Apr.TeamFoundation.Tracking.WiqLinq.Tests.CollectionFixtures
{
    [CollectionDefinition(TestCollectionType.Standard)]
    public class StandardCollection : ICollectionFixture<StandardFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}