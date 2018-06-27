using NUnit.Engine.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Komodo.Core.Listeners
{
    public class KomodoNUnitListener
    {
        [Extension]
        [ExtensionProperty("Format", "custom")]
        public class CustomResultWriterFactory : IResultWriter
        {
            public void CheckWritability(string outputPath)
            {
                // throw new NotImplementedException();
            }

            public void WriteResultFile(XmlNode resultNode, string outputPath)
            {
                // throw new NotImplementedException();
            }

            public void WriteResultFile(XmlNode resultNode, TextWriter writer)
            {
                // throw new NotImplementedException();
            }
        }
    }
}
