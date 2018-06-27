using CsvHelper.Configuration;
using Komodo.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo.Web.Mapping
{
    public sealed class LocatorMap : ClassMap<Locator>
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
