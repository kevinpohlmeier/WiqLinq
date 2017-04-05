using Apr.TeamFoundation.Tracking.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Linq;
using System;
using Microsoft.TeamFoundation.Client;
using System.Net;

namespace Apr.TeamFoundation.Tracking.WiqLinq.Tests
{
    
    
    /// <summary>
    ///This is a test class for WorkItemStoreExtenderTest and is intended
    ///to contain all WorkItemStoreExtenderTest Unit Tests
    ///</summary>
	[TestClass()]
	public class WorkItemStoreExtenderTest
	{
		const string TFS_SERVER = "http://test-app:8080";

		private TestContext testContextInstance;

		private static TeamFoundationServer server;

		private WorkItemStore workItemStore;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}



		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext)
		{
			server = new TeamFoundationServer(TFS_SERVER, CredentialCache.DefaultNetworkCredentials);
			server.Authenticate();

			if (!server.HasAuthenticated)
				throw new InvalidOperationException("Not authenticated");
		}
		
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		public static void MyClassCleanup()
		{
			server.Dispose();
			server = null;
		}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		private WorkItemStore GetStore()
		{
			if (workItemStore == null)
			{
				workItemStore = new WorkItemStore(server);
			}
			return workItemStore;
		}


		/// <summary>
		///A test for WorkItems
		///</summary>
		[TestMethod()]
		public void StoreExtenderBasicTest()
		{
			WorkItemStore store = GetStore(); // TODO: Initialize to an appropriate value
			Type expectedType = typeof(IQueryable<WorkItem>);

			var actual = store.WorkItems();

			Assert.IsInstanceOfType(actual, expectedType);
		}

		/// <summary>
		///A test for WorkItems
		///</summary>
		[TestMethod()]
		public void StoreExtenderOverloadTest()
		{
			WorkItemStore store = GetStore(); // TODO: Initialize to an appropriate value
			DateTime asof = DateTime.MinValue; // TODO: Initialize to an appropriate value
			Type expectedType = typeof(IQueryable<WorkItem>);

			var actual = store.WorkItems(asof);

			Assert.IsInstanceOfType(actual, expectedType);

			string actualWiql = string.Join(" ", actual.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToArray());
			string expectedWiql = "SELECT [System.Id] FROM WORKITEMS   ASOF 1/1/0001 12:00:00 AM";

			Assert.AreEqual<string>(expectedWiql, actualWiql);
		}
	}
}
