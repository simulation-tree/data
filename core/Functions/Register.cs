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
    }
}