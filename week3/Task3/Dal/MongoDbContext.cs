using MongoDB.Driver;
using Task3.Entities;

namespace Task3.Dal
{
    sealed class MongoDbContext
    {
        readonly string _connStr;
        readonly string _dbName;

        public MongoDbContext(string connStr, string dbName)
        {
            _connStr = connStr;
            _dbName = dbName;
        }

        IMongoClient _client;
        public IMongoClient Client
        {
            get { return _client ?? (_client = new MongoClient(_connStr)); }
        }

        IMongoDatabase _db;
        public IMongoDatabase Db
        {
            get { return _db ?? (_db = Client.GetDatabase(_dbName)); }
        }

        IMongoCollection<StudentInfo> _students;
        public IMongoCollection<StudentInfo> Students
        {
            get { return _students ?? (_students = Db.GetCollection<StudentInfo>("students")); }
        }
    }
}
