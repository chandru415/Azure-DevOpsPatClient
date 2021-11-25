using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevOpsPatClient
{
    class Program
    {
        //============= Config [Edit these with your settings] =====================
        internal const string vstsCollectionUrl = "https://dev.azure.com/org/"; //change to the URL of your VSTS account; NOTE: This must use HTTPS
        // internal const string vstsCollectioUrl = "http://myserver:8080/tfs/DefaultCollection" alternate URL for a TFS collection
        internal const string pat = "PAT_GOES_HERE";  // change to the generated value

        internal const string project = "PAT_GOES_HERE";  // change to the generated value
        //==========================================================================

        //Console application to execute a user defined work item query
        static void Main(string[] args)
        {
            //Prompt user for credential
            VssConnection connection = new VssConnection(new Uri(vstsCollectionUrl), new VssBasicCredential(string.Empty, pat));

            //create http client and query for resutls
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            Wiql query = new Wiql() { Query = @"SELECT [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State], [System.CreatedDate], [Microsoft.VSTS.Scheduling.StartDate], [Microsoft.VSTS.Common.ClosedDate] from WorkItems where [System.TeamProject] = @Project and [System.WorkItemType] = 'Task' and [System.State] = 'Done'" };
            WorkItemQueryResult queryResults = witClient.QueryByWiqlAsync(query).Result;


            //Display reults in console
            if (queryResults == null || queryResults.WorkItems.Count() == 0)
            {
                Console.WriteLine("Query did not find any results");
            }
            else
            {
                var ids = new List<int>();
                foreach (var item in queryResults.WorkItems)
                {
                    if (ids.Count < 200)
                    {
                        ids.Add(item.Id);
                    }

                }

                List<WorkItem> workItemsBatch = witClient.GetWorkItemsAsync(project, ids).Result;

                foreach (var item in workItemsBatch)
                {
                    foreach (var dic in item.Fields)
                    {

                        /** Your logic */
                    }
                }
                Console.WriteLine(workItemsBatch.Count);
                Console.ReadLine();
            }
        }
    }
}
