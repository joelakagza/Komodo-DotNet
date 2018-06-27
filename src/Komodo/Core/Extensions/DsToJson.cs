using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Komodo.Core.Extensions
{
    public static class DsToJson
    {
        public static string Ds2Json(this DataSet ds)
        {
            return JsonConvert.SerializeObject(ds, Formatting.Indented);
        }
    }
}
