using Unmanaged;
using Worlds;

namespace Data.Components
{
    [Component]
    public struct Name
    {
        public FixedString value;

        public Name(FixedString value)
        {
            this.value = value;
        }

        public Name(USpan<char> value)
        {
            this.value = new FixedString(value);
        }
    }
}