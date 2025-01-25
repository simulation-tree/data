using Types;
using Unmanaged.Tests;
using Worlds;

namespace Data.Tests
{
    public abstract class DataTests : UnmanagedTests
    {
        static DataTests()
        {
            TypeRegistry.Load<Data.TypeBank>();
        }

        public static World CreateWorld()
        {
            return new(CreateSchema());
        }

        public static Schema CreateSchema()
        {
            Schema schema = new();
            schema.Load<Data.SchemaBank>();
            return schema;
        }
    }
}