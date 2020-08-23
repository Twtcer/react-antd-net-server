using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using ReactAntdServer.Model.Config;
using ReactAntdServer.Model.Data;

namespace ReactAntdServer.Service.Impl
{
    public class ProductService : BaseContextService<Product>
    { 
        public ProductService(IBookstoreDatabaseSettings settings):base(settings,settings.ProductsCollectionName)
        { 
             
        }

        public List<Product> Get() =>
          Collection.Find(a=>true).ToList();

        public Product Get(string id) =>
            Collection.Find(product => product.Id == id).FirstOrDefault();

        public Product Create(Product product)
        {
            Collection.InsertOne(product);
            return product;
        }

        public void Update(string id, Product productNew) =>
            Collection.ReplaceOne(product => product.Id == id, productNew);

        public void Remove(string id) =>
            Collection.DeleteOne(b => b.Id == id);

        public void Remove(Product productIn) =>
          Collection.DeleteOne(product => product.Id == productIn.Id);

    }
}
