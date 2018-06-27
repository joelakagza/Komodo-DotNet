using Komodo.Core.Data;
using Komodo.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo.Data
{

    public static class RepositoryFactory
    {
        public static Core.Data.IRepository<T> GetDataRepository<T>() where T : EntityBase
        {
            string typeName = ConfigurationManager.AppSettings["IDataRepository"];
            Type repoType = Type.GetType(typeName);
            object repoInstance = Activator.CreateInstance(repoType);
            return repoInstance as IRepository<T>;
        }

        public static IBlobRepository GetBlobRepository()
        {
            string typeName = ConfigurationManager.AppSettings["IBlobRepository"];
            Type repoType = Type.GetType(typeName);
            object repoInstance = Activator.CreateInstance(repoType);
            return repoInstance as IBlobRepository;
        }
    }
}