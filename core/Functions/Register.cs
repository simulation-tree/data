using System;

namespace Data.Functions
{
    /// <summary>
    /// A function that registers an embedded resource.
    /// </summary>
    public readonly struct Register
    {
        private readonly Action<Address> function;

        /// <summary>
        /// Creates a new register function.
        /// </summary>
        public Register(Action<Address> function)
        {
            this.function = function;
        }

        /// <summary>
        /// Invokes the function.
        /// </summary>
        public readonly void Invoke(Address address)
        {
            function(address);
        }

        /// <summary>
        /// Invokes the function with the <typeparamref name="T"/> embedded resource.
        /// </summary>
        public readonly void Invoke<T>() where T : unmanaged, IEmbeddedResource
        {
            T template = default;
            function(template.Address);
        }
    }
}