using System.Collections.Generic;

namespace Data
{
    public interface IEmbeddedResources
    {
        IEnumerable<Address> Addresses { get; }
    }
}
