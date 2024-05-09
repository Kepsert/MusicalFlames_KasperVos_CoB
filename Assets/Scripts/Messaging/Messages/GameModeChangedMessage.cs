namespace Messaging.Messages
{
    public class GameModeChangedMessage : MessageBase
    {
        public GameMode GameMode;

        public GameModeChangedMessage(GameMode gameMode)
        {
            GameMode = gameMode;
        }
    }
}
