using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystemTest : MonoBehaviour
{
    [SerializeField] Quest _quest;
    [SerializeField] Category _category;
    [SerializeField] TaskTarget _target;

    void Start()
    {
        var questSystem = QuestManager.Instance;
        questSystem._onQuestRegistered += (_quest) =>
        {
            print($"New Quest:{_quest.CodeName} Registered");
            print($"Active Quest Count : {questSystem.ActiveQuests.Count}");
        };

        questSystem._onQuestCompleted += (_quest) =>
        {
            print($"New Quest:{_quest.CodeName} Completed");
            print($"Active Quest Count : {questSystem.CompletedQuests.Count}");
        };

        var newQuest = questSystem.Register(_quest);
        newQuest._onTaskSuccessChanged += (quest, task, currentSuccess, prevSuccess) =>
        {
            print($"Quest:{quest.CodeName}, Task:{task.CodeName}, CurrentSuccess:{currentSuccess}");
        };

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            QuestManager.Instance.ReceiveReport(_category, _target, 1);
        }
    }
}
