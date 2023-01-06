using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] Quest[] _quests;

    private void Start()
    {
        foreach (var quest in _quests)
        {
            if (quest.IsAcceptable && !QuestManager.Instance.ContainsInCompleteQuests(quest))
            {
                QuestManager.Instance.Register(quest);
            }
        }
    }

}
