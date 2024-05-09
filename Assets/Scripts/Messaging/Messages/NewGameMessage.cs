namespace Messaging.Messages
{
    public class NewGameMessage : MessageBase
    {
        public SequenceGameSettings SequenceGameSettings;
        public NewGameMessage(SequenceGameSettings sequenceGameSettings = null)
        {
            SequenceGameSettings = sequenceGameSettings;
        }
    }
}