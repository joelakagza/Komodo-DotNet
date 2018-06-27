using Komodo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Komodo.Data
{
    public class AZTRepository<T> : IRepository<T> where T :  EntityBase
    {
        private CloudTableClient tableClient;
        private CloudTable table;
        private TableQuery<DynamicTableEntity> query;

        public readonly IRepository<T> _context;
        public AZTRepository()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(typeof(T).Name);
            query = new TableQuery<DynamicTableEntity>();
        }

        public IQueryable<T> Table
        {
            get
            {
                return new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, typeof(T).Name)).Select(t=>t).AsQueryable();
            }
        }

        public IQueryable<T> TableNoTracking
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var item in entities)
                Delete(item);
        }

        public void Delete(T entity)
        {
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Delete(entity);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public T GetById(object id)
        {
            return new TableQuery<T>()
                .Where(TableQuery.CombineFilters( 
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, typeof(T).Name) 
                    ,TableOperators.And , 
               TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id.ToString())))
               .Select(t => t).FirstOrDefault();
        }

        public void Insert(IEnumerable<T> entities)
        {
            foreach (var item in entities)
                Insert(item);
        }

        public void Insert(T entity)
        {
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(entity);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public void Update(T entity)
        {
            TableOperation insertOperation = TableOperation.InsertOrMerge(entity);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public static List<T> ItemDataFlatToObject(IEnumerable<DynamicTableEntity> itemDataFlats) 
        {
            List<T> lst = new List<T>();
            var grps1 = from t in itemDataFlats
                        group t by new { t.PartitionKey, t.RowKey, t.Timestamp } into g
                        select new { g.Key.RowKey, h = g.FirstOrDefault().Properties.ToList().ToDictionary(x => x.Key, x => x.Value.PropertyAsObject.ToString()) };

            ConstructorInfo ctor = typeof(T).GetConstructors().First();
            var obj = Activator.CreateInstance<T>();

            lst = grps1.Select(t => DictionaryToObject(t.h)).ToList();
            
            return lst;
        }

        public static T DictionaryToObject(IDictionary<string, string> dict)
        {
            //var t = new T();
            ConstructorInfo ctor = typeof(T).GetConstructors().First();
            var obj = Activator.CreateInstance<T>();

            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, string> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property is...
                Type tPropertyType = obj.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables...
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                try
                {
                    // ...and change the type
                    if (newT.Name != "Int32" && item.Value != null)
                    {
                        object newA = Convert.ChangeType(item.Value, newT);
                        obj.GetType().GetProperty(property.Name).SetValue(obj, newA, null);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            return obj;
        }

    }
}
