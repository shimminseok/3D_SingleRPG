using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/GameObject", fileName = "Target_")]
public class GameObjectTarget : TaskTarget
{

    [SerializeField] GameObject _value;
    public override object Value => _value;

    public override bool IsEqual(object target)
    {
        var targetAsGameObject = target as GameObject;
        if(targetAsGameObject == null)
        {
            return false;
        }
        return targetAsGameObject.name.Contains(_value.name);
    }
}
