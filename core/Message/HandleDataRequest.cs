using Data.Components;
using Worlds;

namespace Data.Messages
{
    public struct HandleDataRequest
    {
        public readonly Entity entity;
        public IsDataRequest request;

        public HandleDataRequest(Entity entity, IsDataRequest request)
        {
            this.entity = entity;
            this.request = request;
        }
    }
}