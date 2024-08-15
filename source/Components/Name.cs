using Unmanaged;

namespace Data.Components
{
    public struct Name
    {
        public FixedString value;

        public Name(FixedString value)
        {
            this.value = value;
        }
    }
}