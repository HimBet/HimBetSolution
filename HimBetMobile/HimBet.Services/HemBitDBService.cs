using HemBit.Model;
 
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HemBit.Services
{
    public class HemBitDBService<T> where T : IPersistant
    {
        protected readonly IMongoCollection<T> _items;

        public HemBitDBService(IHemBitDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _items = database.GetCollection<T>(typeof(T).Name);
        }

        public List<T> Get() =>
            _items.Find(item => true).ToList();

        public T Get(string id) =>
            _items.Find<T>(item => item.Id == id).FirstOrDefault();


        public T Create(T item)
        {
            _items.InsertOne(item);
            return item;
        }

        public void BulkInsert(IEnumerable<T> items)
        {
            string keyname = "_id";

            #region Get Primary Key Name 
            PropertyInfo[] props = typeof(T).GetProperties();

            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    BsonIdAttribute authAttr = attr as BsonIdAttribute;
                    if (authAttr != null)
                    {
                        keyname = prop.Name;
                    }
                }
            }
            #endregion

            var bulkOps = new List<WriteModel<T>>();


            foreach (var entry in items)
            {
                var filter = Builders<T>.Filter.Eq(keyname, entry.GetType().GetProperty(keyname).GetValue(entry, null));

                var upsertOne = new ReplaceOneModel<T>(filter, entry) { IsUpsert = true };

                bulkOps.Add(upsertOne);
            }
            _items.BulkWrite(bulkOps);

        }

        public void Update(string id, T itemIn) =>
            _items.ReplaceOne(item => item.Id == id, itemIn);

        public void Remove(T itemIn) =>
            _items.DeleteOne(item => item.Id == itemIn.Id);

        public void Remove(string id) =>
            _items.DeleteOne(item => item.Id == id);
    }
}