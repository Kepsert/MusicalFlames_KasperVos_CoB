namespace Messaging.Messages
{
    public class ChangeGameStateMessage : MessageBase
    {
        public GameState State;
        
        public ChangeGameStateMessage(GameState state)
        {
            State = state;
        }
    }
}
