using System;

namespace Messaging
{
    public class MessageBase
    {
        public Guid Uid { get; } = Guid.NewGuid();
    }
}
