using System.Collections.Generic;
using UnityEngine;

public class ListValueSwapper : MonoBehaviour
{
    public static void Swap<T>(List<T> list, int indexA, int indexB)
    {
        T temp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = temp;
    }
}
