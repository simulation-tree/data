using Worlds;

namespace Data.Components
{
    [Array]
    public struct BinaryData
    {
        public byte value;

        public BinaryData(byte value)
        {
            this.value = value;
        }
    }
}