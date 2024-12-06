using Worlds;

namespace Data.Components
{
    [Component]
    public struct IsData
    {
        public uint version;

        public IsData(uint version)
        {
            this.version = version;
        }
    }
}