using MongoDB.Bson.Serialization.Conventions;

namespace M101DotNet.WebApp.App_Start
{
    public class BsonConfig
    {
        public static void ConfigureBson()
        {
            /*var conv = new ConventionPack();
            conv.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("camelCase", conv, t => true);*/
        }
    }
}