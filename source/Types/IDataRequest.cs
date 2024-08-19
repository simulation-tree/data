using Simulation;
using Unmanaged;

namespace Data
{
    public interface IDataRequest : IEntity
    {
        FixedString Address { get; }
        DataRequest.DataStatus Status { get; }
    }
}
