using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameSaveData
{
    public string _userID;
    public string _userPW;
    public string _nickName;
    public int _level;
    public float _curHp;
    public float _curMp;
    public int _curEx;
    public float[] _curPos;
    public int[] _amountItem;
    public int[] _inventoryItem;
    public int _money;
}
