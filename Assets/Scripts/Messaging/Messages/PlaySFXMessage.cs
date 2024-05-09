namespace Messaging.Messages
{
    public class PlaySFXMessage : MessageBase
    {
        public string Name;
        public float Pitch;

        public PlaySFXMessage(string name, float pitch = 1)
        {
            Name = name;
            Pitch = pitch;
        }
    }
}
