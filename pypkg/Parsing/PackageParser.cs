namespace pypkg.Parsing
{
    public struct ScopeName
    {
        public string Scope;
        public string Name;
    }

    public class PackageParser
    {
        public static ScopeName Parse(string Package)
        {
            string[] Split = Package.Split("/");
            ScopeName ParsedPackage = new ScopeName();
            ParsedPackage.Name = Split[0];
            ParsedPackage.Name = Split[1];

            return ParsedPackage;
        }

    }
}
