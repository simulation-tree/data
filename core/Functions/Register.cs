using System;

namespace Data.Functions
{
    public readonly struct Register
    {
        private readonly Action<Address> function;

        public Register(Action<Address> function)
        {
            this.function = function;
        }

        public readonly void Invoke(Address address)
        {
            function(address);
        }

        public readonly void Invoke<T>() where T : unmanaged, IEmbeddedResource
        {
            T template = default;
            function(template.Address);
        }
    }
}