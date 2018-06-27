using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo.Core
{
    [SetUpFixture]
    public class SetupClass
    {
        [OneTimeSetUp]
        private void RunBeforeAnyTests()
        {
            var dir = Path.GetDirectoryName(typeof(SetupClass).Assembly.Location);
            Environment.CurrentDirectory = dir;

            // or
            Directory.SetCurrentDirectory(dir);
        }
    }
}
