using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo.Core
{
    public static class CommonHelper
    {
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random(); return random.Next(min, max);
        }
    }
}
