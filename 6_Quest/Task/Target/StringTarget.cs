using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Quest/Task/Target/String", fileName = "Target_")]
public class StringTarget : TaskTarget
{
    [SerializeField] string _value;
    public override object Value => _value;

    public override bool IsEqual(object target)
    {
        string targetAsString = target.ToString();
        if (targetAsString == null)
        {
            return false;
        }
        return _value == targetAsString;
    }
}
