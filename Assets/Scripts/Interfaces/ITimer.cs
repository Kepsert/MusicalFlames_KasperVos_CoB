using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimer
{
    float Duration { get; set; }
    float ElapsedTime { get; set; }

    void Reset();
    void Refresh();
}
