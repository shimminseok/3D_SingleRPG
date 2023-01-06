using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/CompareCount", fileName = "Compare Count")]

public class CompareCount : TaskAction
{
    //���� ������ ��ǥ�������� ������ ���緹���� ����
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount > currentSuccess ? successCount : currentSuccess;
    }
}
