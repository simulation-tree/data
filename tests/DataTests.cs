using Types;
using Worlds;
using Worlds.Tests;

namespace Data.Tests
{
    public abstract class DataTests : WorldTests
    {
        static DataTests()
        {
            MetadataRegistry.Load<DataMetadataBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<DataSchemaBank>();
            return schema;
        }
    }
}