using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo.Data
{
    public interface  IBlobRepository
    {
        void SaveFiile(string filePath, Stream stream);
    }
}
