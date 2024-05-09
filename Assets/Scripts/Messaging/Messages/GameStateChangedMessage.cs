namespace Messaging.Messages
{
    public class GameStateChangedMessage : MessageBase
    {
        public GameState GameState;

        public GameStateChangedMessage(GameState gameState)
        {
            GameState = gameState;
        }
    }
}
