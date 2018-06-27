using Komodo.Data;

namespace Komodo.Core.Domain
{
    public partial class Locator : EntityBase
    {
        public string tcId { get; set; }
        public string keyName { get; set; }
        public string page { get; set; }
        public string byName { get; set; }
        public string byValue { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string url { get; set; }
    }
}
