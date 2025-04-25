using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Unmanaged;

namespace Data
{
    /// <summary>
    /// Describes where an embedded resource can be loaded from.
    /// </summary>
    public readonly struct EmbeddedResource
    {
        /// <summary>
        /// The original address of the resource.
        /// </summary>
        public readonly Address address;

        private readonly GCHandle assembly;

        /// <summary>
        /// The assembly where this resource is found.
        /// </summary>
        public readonly Assembly Assembly => (Assembly)(assembly.Target ?? throw new InvalidOperationException("Assembly has been garbage collected, and is no longer available"));

        /// <summary>
        /// Creates a new embedded resource location.
        /// </summary>
        public EmbeddedResource(Assembly assembly, Address address)
        {
            this.assembly = GCHandle.Alloc(assembly, GCHandleType.Weak);
            this.address = address;
        }

        /// <summary>
        /// Creates a new binary reader with the contents of this embedded resource.
        /// </summary>
        public readonly ByteReader CreateByteReader()
        {
            Assembly assembly = Assembly;
            string resourcePath = $"{assembly.GetName().Name}.{address.ToString().Replace('/', '.')}";
            System.IO.Stream stream = assembly.GetManifestResourceStream(resourcePath) ?? throw new Exception($"Embedded resource at `{resourcePath}` could not be found");
            stream.Position = 0;
            return new(stream);
        }

        /// <summary>
        /// Retrieves the <see cref="Address"/> for <typeparamref name="T"/> data.
        /// </summary>
        public static Address GetAddress<T>() where T : unmanaged, IEmbeddedResource
        {
            return default(T).Address;
        }
    }
}