using UnityEngine;

[CreateAssetMenu(fileName = "SequenceGameSettings", menuName = "Settings/Sequence Game", order = 2)]
public class SequenceGameSettings : ScriptableObject
{
    [SerializeField, Min(2)] int _amountOfRounds = 2;
    public int AmountOfRounds { get { return _amountOfRounds; } }
    [SerializeField, Min(2)] int _startSequenceLength = 2;
    public int StartSequenceLength { get { return _startSequenceLength; } }
    [SerializeField, Min(1)] int _sequenceIncrementPerRound = 1;
    public int SequenceIncrementPerRound { get { return _sequenceIncrementPerRound; } }
    [SerializeField, Min(1)] int _swapBlockStartRound = 2;
    public int SwapBlockStartRound { get { return _swapBlockStartRound; } }
    [SerializeField, Min(2)] float _roundTimer = 10f;
    public float RoundTimer { get { return _roundTimer; } }
}
