using Worlds;

namespace Data.Components
{
    [ArrayElement]
    public struct BinaryData
    {
        public byte value;

        public BinaryData(byte value)
        {
            this.value = value;
        }
    }
}