// pypupy - 2022

namespace pypkg.API
{
    public struct APIUrl
    {
        public static string URL = "http://api.wally.run/";
        public static string PackageZIP = URL + "v1/package-contents/{0}/{1}/{2}";
        public static string PackageMetadata = URL + "v1/package-metadata/{0}/{1}";
        public static string PackageQuery = URL + "v1/package-search?query={0}";
        public static string PackagePublish = URL + "api/v1/publish";

        // Silence VS2022's dumb err:
        public APIUrl() { }
    }
}
