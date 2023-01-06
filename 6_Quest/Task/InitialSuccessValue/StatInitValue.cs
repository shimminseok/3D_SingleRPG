using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/InitialSuccessValue/InitialSuccessStatValue", fileName = "InitialSuccessStatValue")]
public class StatInitValue : InitialSuccessValue
{
    [SerializeField] StatType _statType;

    public override int GetValue(Task task)
        => GameManager._instance._character.GetValue(_statType);
}
