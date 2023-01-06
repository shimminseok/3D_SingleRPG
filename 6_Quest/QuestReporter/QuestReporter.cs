using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestReporter : MonoBehaviour
{
    [SerializeField] Category _category;
    [SerializeField] TaskTarget _target;
    [SerializeField] int _successCount;
    [SerializeField] string[] _colliderTag;

    void OnTriggerEnter(Collider other)
    {
        ReportIfPassCondition(other);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        ReportIfPassCondition(collision);
    }
    public void Report()
    {
        QuestManager.Instance.ReceiveReport(_category, _target, _successCount);
    }
    void ReportIfPassCondition(Component other)
    {
        if (_colliderTag.Any(x => other.CompareTag(x)))
        {
            Report();
        }
    }

}
