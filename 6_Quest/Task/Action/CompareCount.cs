using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/CompareCount", fileName = "Compare Count")]

public class CompareCount : TaskAction
{
    //현재 레벨이 목표레벨보다 낮으면 현재레벨을 리턴
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount > currentSuccess ? successCount : currentSuccess;
    }
}
