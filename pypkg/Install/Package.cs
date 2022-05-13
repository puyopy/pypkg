// pypupy - 2022

namespace pypkg.Install
{
    public struct Package
    {
        public string name;
        public string registry;
        public string realm;
        public string description;
        public string license;
        public string version;
        public string[] authors;
        public string[] include;
        public string[] exclude;
    }
}
