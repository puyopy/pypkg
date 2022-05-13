// pypupy - 2022

namespace pypkg.Install
{
    public class PackageVersion
    {
        public IDictionary<string, string> dependencies;

        // hopefully this converts well.
        public IDictionary<string, string> devdependencies;
        public Package package;
    }
}
