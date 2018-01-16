using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Task3.Entities
{
    sealed class StudentInfo
    {
        internal enum TestType
        {
            exam, quiz, homework
        }

        internal sealed class ScoreInfo
        {
            [JsonConverter(typeof(StringEnumConverter))]
            [BsonRepresentation(BsonType.String)]
            public TestType Type { get; set; }

            public double Score { get; set; }
        }

        //[BsonId]
        //public BsonObjectId StudentId { get; set; }
        [BsonElement("_id")]
        public int StudentId { get; set; }

        public string Name { get; set; }

        public IList<ScoreInfo> Scores { get; set; }

        public int FindLowesScoreIndex(TestType t)
        {
            int idx = -1;
            double scoreMin = Double.MaxValue;
            for (int i = 0; i < Scores.Count; ++i)
            {
                var s = Scores[i];
                if (s.Type == t && s.Score < scoreMin)
                {
                    idx = i;
                    scoreMin = s.Score;
                }
            }
            return idx;
        }
    }
    
}
