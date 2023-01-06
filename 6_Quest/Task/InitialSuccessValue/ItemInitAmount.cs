using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/InitialSuccessValue/InitialSuccessItemAmount", fileName = "InitialItemAmount")]

public class ItemInitAmount : InitialSuccessValue
{
    [SerializeField] TaskTarget _target;
    public override int GetValue(Task task)
    {
        return UIManager._instance._inventoryWindow.CheakQuestItem(_target);
    }
}
