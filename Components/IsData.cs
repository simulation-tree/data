using System;
using Unmanaged;

namespace Data.Components
{
    public struct IsData
    {
        public FixedString name;

        public IsData(FixedString name)
        {
            this.name = name;
        }

        public IsData(ReadOnlySpan<char> name)
        {
            this.name = new(name);
        }
    }
}