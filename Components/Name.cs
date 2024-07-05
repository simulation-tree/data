using Unmanaged;

namespace Data.Components
{
    public readonly struct Name
    {
        public readonly FixedString value;

        public Name(FixedString value)
        {
            this.value = value;
        }
    }
}