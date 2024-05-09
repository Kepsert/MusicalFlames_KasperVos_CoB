using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceHelper<T>
{
    List<T> _sequenceList = new List<T>();
    int _sequenceLength = 3;
    int _sequenceIncrement = 1;
    int _sequenceIndex = 0;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="sequenceLength"></param>
    /// <param name="sequenceIncrement"></param>
    public SequenceHelper(int sequenceLength, int sequenceIncrement)
    {
        _sequenceLength = sequenceLength;
        _sequenceIncrement = sequenceIncrement;
    }

    /// <summary>
    /// Create a brand new sequence using the default sequence length
    /// </summary>
    /// <param name="sequenceGeneratorAction"></param>
    /// <returns></returns>
    public List<T> GenerateSequence(Func<T> sequenceGeneratorAction)
    {
        // Create a brand new sequence so clear the old one first
        _sequenceList.Clear();

        for (int i = 0; i < _sequenceLength; i++)
        {
            T sequenceItem = sequenceGeneratorAction();
            _sequenceList.Add(sequenceItem);
        }
        return _sequenceList;
    }

    /// <summary>
    /// Create a brand new sequence using a given sequencelength
    /// </summary>
    /// <param name="sequenceGeneratorAction"></param>
    /// <param name="sequenceLength"></param>
    /// <returns></returns>
    public List<T> GenerateSequence(Func<T> sequenceGeneratorAction, int sequenceLength)
    {
        // Create a brand new sequence so clear the old one first
        _sequenceList.Clear();

        for (int i = 0; i < sequenceLength; i++)
        {
            T sequenceItem = sequenceGeneratorAction();
            _sequenceList.Add(sequenceItem);
        }
        return _sequenceList;
    }

    /// <summary>
    /// Add new items to a sequence based on _sequenceIncrement
    /// </summary>
    /// <param name="sequenceAdditionAction"></param>
    /// <returns></returns>
    public List<T> AddToSequence(Func<T> sequenceAdditionAction)
    {
        _sequenceIndex = 0;
        for (int i = 0; i < _sequenceIncrement; i++)
        {
            T sequenceItem = sequenceAdditionAction();
            _sequenceList.Add(sequenceItem);
        }
        return _sequenceList;
    }

    /// <summary>
    /// Add new items to a sequence based on a given sequenceIncrement
    /// </summary>
    /// <param name="sequenceAdditionAction"></param>
    /// <param name="sequenceIncrement"></param>
    /// <returns></returns>
    public List<T> AddToSequence(Func<T> sequenceAdditionAction, int sequenceIncrement)
    {
        _sequenceIndex = 0;
        for (int i = 0; i < sequenceIncrement; i++)
        {
            T sequenceItem = sequenceAdditionAction();
            _sequenceList.Add(sequenceItem);
        }
        return _sequenceList;
    }

    /// <summary>
    /// Check to see if the previous input was the last one for the sequence
    /// </summary>
    /// <returns></returns>
    public bool IsFinalSequenceInput()
    {
        if (_sequenceIndex == _sequenceList.Count)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Compare which player input with the item in _sequenceList at _sequenceIndex
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public bool CheckSequenceInput(T input)
    {
        if (_sequenceIndex < _sequenceList.Count)
        {
            if (EqualityComparer<T>.Default.Equals(input, _sequenceList[_sequenceIndex]))
            {
                _sequenceIndex++;
                return true;
            }
            else
            {
                ResetSequence();
                return false;
            }
        }
        return false;
    }

    public void ResetSequence()
    {
        _sequenceIndex = 0;
    }
}
