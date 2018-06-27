using Komodo.Core.Model;

namespace Komodo.Core.Core.Model
{
    public class TestModel
    {
        public TestModel()
        {
            ConfigModel = new ConfigModel();
        }

        public string Type { get; set; }
        public string Category { get; set; }
        public ConfigModel ConfigModel { get; set; }
    }

}
