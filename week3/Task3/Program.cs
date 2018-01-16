using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Task3.Dal;
using Task3.Entities;

namespace Task3
{
    public static class Program
    {
        [MTAThread]
        static void Main (string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Press a key...");
            Console.ReadLine();
        }

        [MTAThread]
        public static async Task MainAsync (string[] args)
        {
            SetupConventions();
            
            var dbCtx = new MongoDbContext("mongodb://localhost:27017", "school");

            var students = await dbCtx.Students.Find<StudentInfo>(Builders<StudentInfo>.Filter.Empty).ToListAsync();
            int count = 0;
            var tasks = new List<Task<UpdateResult>>();
            foreach (var student in students)
            {
                //Console.WriteLine(student.ToBsonDocument());
                //Console.WriteLine("----");
                var index = student.FindLowesScoreIndex(StudentInfo.TestType.homework);
                if (index > -1)
                {
                    student.Scores.RemoveAt(index);                    
                    var task = dbCtx.Students.UpdateOneAsync(Builders<StudentInfo>.Filter.Eq("_id", student.StudentId), Builders<StudentInfo>.Update.Set(st => st.Scores, student.Scores));
                    tasks.Add(task);
                    count++;
                }
            }

            Console.WriteLine("Waiting...");
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"{count} updated");
        }

        static void SetupConventions ()
        {
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
            ConventionRegistry.Register("myConventions", pack, t => true);
        }
    }

    
}
