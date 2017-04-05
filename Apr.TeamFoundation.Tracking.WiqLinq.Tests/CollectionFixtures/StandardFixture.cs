using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Apr.TeamFoundation.Tracking.WiqLinq.Tests.CollectionFixtures
{
    public class StandardFixture
    {
        const string TFS_SERVER = "http://tfs2017:8080/tfs/DPathCollection";

        public WorkItemStore WorkItemStore { get; }

        public StandardFixture()
        {
            var server = new TfsTeamProjectCollection(new Uri(TFS_SERVER));
            server.Authenticate();

            if (!server.HasAuthenticated)
            {
                throw new InvalidOperationException("Not authenticated");
            }

            WorkItemStore = server.GetService<WorkItemStore>();
        }
    }
}