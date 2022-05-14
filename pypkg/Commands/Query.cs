using Newtonsoft.Json;
using pypkg.API;

namespace pypkg.Commands
{
    [JsonArray]
    public class QueryResults
    {
        public QueryResult[] Results;
    }

    
    public class QueryResult //: IDictionary<string,string>
    {
        public string description;
        public string name;
        public string scope;
         public string[] versions;
    }


    public class QueryCommand : Command
    {
        public string Name = "query";

        public class WALLY_API_IS_FUCKED_EXCEPTION : Exception
        {
            public WALLY_API_IS_FUCKED_EXCEPTION()
            {
                Logger.Log("Wally API is currently fucked and is not readable in C#. Please check back later.",Logger.InfoType.Exception);
                
            }
        }

        public async Task<CommandStatus> Execute(string[] args)
        {
            if (true)
            {
                throw new WALLY_API_IS_FUCKED_EXCEPTION();
                return CommandStatus.Failure;
            }
            
            
            string ArgQuery = args[0];
            string FormattedQuery = string.Format(APIUrl.PackageQuery, ArgQuery);
            Logger.Log($"Searching for {ArgQuery}");
            var Response = await Manager.HttpClient.GetAsync(FormattedQuery);
            Response = Response.EnsureSuccessStatusCode();

            string JsonResponse = await Response.Content.ReadAsStringAsync();
            //Logger.Log(JsonResponse);


            QueryResults ManifestVersions = JsonConvert.DeserializeObject<QueryResults>(JsonResponse);
            int ResultCount = ManifestVersions.Results.Length;

            Logger.Log($"Found {ResultCount} results for '{ArgQuery}'");

            foreach (QueryResult result in ManifestVersions.Results)
            {
                string Scope = result.scope;
                string Name = result.name;
                string Description = result.name;
                int VersionsAvailable = result.versions.Length;
                string StringVersions = string.Join("\n", result.versions);

                Logger.Log($@"
                    {Scope}/{Name} - {Description}

                    {VersionsAvailable} versions available:
                        {StringVersions}
                ");
            }

            return CommandStatus.Success;
        }
    }
}
