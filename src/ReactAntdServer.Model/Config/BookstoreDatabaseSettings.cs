using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactAntdServer.Model.Config
{
    public class BookstoreDatabaseSettings: IBookstoreDatabaseSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string BooksCollectionName { get; set; }
        public string ManagersCollectionName { get; set; }
        public string ProductsCollectionName { get; set; }
    }

    public interface IBookstoreDatabaseSettings
    {
         string ManagersCollectionName { get; set; }
        string BooksCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string ProductsCollectionName { get; set; }
    }
}
