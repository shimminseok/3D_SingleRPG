using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reword/Gold", fileName = "GoldReword")]
public class GoldReword : Reword
{
    public override void Give(Quest quest)
    {
        GameManager._instance.GetGold(Quantity);
    }
}
