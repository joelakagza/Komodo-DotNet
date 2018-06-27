using CsvHelper.Configuration;

namespace Komodo.Core.Domain
{
    public class Locator
    {
        public Locator()
        { }

        public string KeyName { get; set; }
        public string ByName { get; set; }
        public string ByValue { get; set; }
        public string ByType { get; set; }
        public string Page { get; set; }
        public string Description { get; set; }
        public string Browser { get; set; }
    }

    public sealed class LocatorMap : CsvClassMap<Locator>
    {
        public LocatorMap()
        {
            Map(m => m.KeyName).Index(0);
            Map(m => m.ByName).Index(1);
            Map(m => m.ByValue).Index(2);// ("byValue");
            Map(m => m.ByType).Index(3);// ("byType");
            Map(m => m.Description).Ignore();
        }
    }
}
