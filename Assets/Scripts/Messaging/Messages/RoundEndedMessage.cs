namespace Messaging.Messages
{
    public class RoundEndedMessage : MessageBase
    {
        public int RoundNumber;
        public RoundEndedMessage(int roundNumber = 0)
        {
            RoundNumber = roundNumber;
        }
    }
}