using System.Collections.Generic;
using UnityEngine;

public class InputValueHelper
{
    List<int> _inputValues = new List<int>();
    int _amountOfValues;

    public InputValueHelper(int amountOfValues)
    {
        _amountOfValues = amountOfValues;

        InitiateInputValues(_amountOfValues);
    }

    public void InitiateInputValues(int amountOfValues)
    {
        _inputValues.Clear();

        for (int i = 1; i < amountOfValues + 1; i++)
        {
            _inputValues.Add(i);
        }
    }

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

    public int GetInputValue(int index)
    {
        index--;
        int inputValue = _inputValues[index];
        return inputValue;
    }
}
