using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Because the input doesn't always equal the value of the "button" (due to them swapping" we keep track of where in the array each value is, then return it based on player input
/// </summary>
public class InputValueHelper
{
    List<int> _inputValues = new List<int>();
    int _amountOfValues;

    public InputValueHelper(int amountOfValues)
    {
        _amountOfValues = amountOfValues;

        InitiateInputValues(_amountOfValues);
    }

    /// <summary>
    /// Initiate a list with the amount of values required. 
    /// </summary>
    /// <param name="amountOfValues"></param>
    public void InitiateInputValues(int amountOfValues)
    {
        _inputValues.Clear();

        for (int i = 1; i < amountOfValues + 1; i++)
        {
            _inputValues.Add(i);
        }
    }

    /// <summary>
    /// When the Input Visualizers on screen swap, the values also need to swap behind the screen. Pick 2 random values, swap them, and return them.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public List<int> SwapTwoRandomValuesInList(int min, int max)
    {
        List<int> indices = new List<int>(2);
        while (indices.Count < 2)
        {
            int index = Random.Range(min, max);
            if (!indices.Contains(index))
            {
                indices.Add(index);
            }
        }

        ListValueSwapper.Swap<int>(_inputValues, indices[0], indices[1]);

        return indices;
    }

    /// <summary>
    /// Due to input swaps, the playerinput will not always align with the actual value. Get the corect value
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetInputValue(int index)
    {
        index--;
        int inputValue = _inputValues[index];
        return inputValue;
    }
}
