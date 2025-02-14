using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Unmanaged;

namespace Data
{
    public readonly struct EmbeddedResource
    {
        public readonly Address address;

        private readonly GCHandle assembly;

        public readonly Assembly Assembly => (Assembly)(assembly.Target ?? throw new InvalidOperationException("Assembly has been garbage collected, and is no longer available"));

        public EmbeddedResource(Assembly assembly, Address address)
        {
            this.assembly = GCHandle.Alloc(assembly, GCHandleType.Weak);
            this.address = address;
        }

        /// <summary>
        /// Creates a new binary reader with the contents of this embedded resource.
        /// </summary>
        public readonly BinaryReader CreateBinaryReader()
        {
            Assembly assembly = Assembly;
            string[] names = assembly.GetManifestResourceNames();
            string resourcePath = $"{assembly.GetName().Name}.{address.ToString().Replace('/', '.')}";
            System.IO.Stream stream = assembly.GetManifestResourceStream(resourcePath) ?? throw new Exception($"Embedded resource at `{resourcePath}` could not be found");
            stream.Position = 0;
            return new(stream);
        }
    }
}