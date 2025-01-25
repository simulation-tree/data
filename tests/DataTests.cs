using Types;
using Worlds;
using Worlds.Tests;

namespace Data.Tests
{
    public abstract class DataTests : WorldTests
    {
        static DataTests()
        {
            TypeRegistry.Load<Data.TypeBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<Data.SchemaBank>();
            return schema;
        }
    }
}