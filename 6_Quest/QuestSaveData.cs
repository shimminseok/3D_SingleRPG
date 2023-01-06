using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct QuestSaveData
{
    public string _codeName;
    public QuestState _state;
    public int _taskGroupIndex;
    public int[] _taskSuccessCounts;
}
