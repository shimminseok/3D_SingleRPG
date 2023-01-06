using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reword/Experience", fileName = "ExperienceReword")]
public class ExperienceReword: Reword
{
    public override void Give(Quest quest)
    {
        GameManager._instance.GetExperience(Quantity);
    }
}
