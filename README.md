# WiqLinq
WiqLinq is a Linq-to-WIQL library, which makes it easy to build complex WIQL queries to TFS Server (WIQL is a work item query language, used to query TFS workitems database). 

I got this code from https://wiqlinq.codeplex.com/ and modified it to work with TFS 2017.

**Example Query**

The solution contains a Unit Test project that will help with examples, but here is one that highlights several key features:

~~~~

var server = new TfsTeamProjectCollection(new Uri("http://tfs2017:8080/tfs/DefaultCollection"));
WorkItemStore store = server.GetService<WorkItemStore>();

var query = store.WorkItems()
	.Where(wi => wi.Field(CoreFieldReferenceNames.Title).Contains("test"))
	.Where(wi => wi.Field(CoreFieldReferenceNames.AreaPath).Under(@"MyProject\MyArea"))
	.Where(wi => wi.Field(CoreFieldReferenceNames.AssignedTo).Ever("Kevin Pohlmeier"))
	.OrderByDescending(wi => wi.Id);
		
// Dump the generated WIQL in LinqPad		
query.ToString().Dump();

// Dump some fields from the results in LinqPad
// NOTE: I do a .ToList() first to get the WorkItems in memory, 
// since selecting columns in the WIQL doesn't actually prevent the data in the other columns from being queried.
query.ToList()
	.Select(wi => new{ wi.Id, wi.Title, wi.AreaPath })
	.Dump();

~~~~

**Using it in LinqPad**

1. Build the project, and add Additional References in LinqPad (press F4) for the following dlls from your bin folder 
    1. Apr.TeamFoundation.Tracking.WiqLinq.dll
    2. Microsoft.TeamFoundation.Client.dll
    3. Microsoft.TeamFoundation.WorkItemTracking.Client.dll
2. Add the following Additional Namespace Imports in LinqPad 
    1. Apr.TeamFoundation.Tracking.Linq
    2. Microsoft.TeamFoundation.Client
    3. Microsoft.TeamFoundation.WorkItemTracking.Client
3. Add the following dlls from your bin folder into the LinqPad installation directory
    1. Microsoft.WITDataStore32.dll
    2. Microsoft.WITDataStore64.dll
    
NOTE: If you try to include the dlls from step 3 in your LinqPad Additional References instead, you will get an error saying;
"CS0009 Metadata file 'xxx\Microsoft.WITDataStore32.dll' could not be opened -- PE image doesn't contain managed metadata."
