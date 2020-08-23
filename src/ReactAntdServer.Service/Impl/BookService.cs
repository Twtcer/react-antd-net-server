using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using ReactAntdServer.Model;
using ReactAntdServer.Model.Config;
using ReactAntdServer.Service;
using ReactAntdServer.Service.Base;

namespace ReactAntdServer.Services
{
    public class BookService:BaseContextService<Book>
    { 
        public BookService(IBookstoreDatabaseSettings settings):base(settings,settings.ProductsCollectionName)
        { 
        }

        public List<Book> Get() =>
            Collection.Find(book => true).ToList();

        public Book Get(string id) =>
            Collection.Find(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            Collection.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookNew) =>
            Collection.ReplaceOne(book => book.Id == id, bookNew);

        public void Remove(string id) =>
            Collection.DeleteOne(b => b.Id == id);

        public void Remove(Book bookIn) =>
          Collection.DeleteOne(book => book.Id == bookIn.Id);
    }
}
