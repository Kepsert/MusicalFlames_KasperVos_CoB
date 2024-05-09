using UnityEngine;

namespace Messaging.Messages
{
    public class InjectUIMessage : MessageBase
    {
        public Object Object;

        public InjectUIMessage(Object objectToInject)
        {
            Object = objectToInject;
        }
    }
}
