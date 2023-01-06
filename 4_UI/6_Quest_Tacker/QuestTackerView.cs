using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestTackerView : MonoBehaviour
{
    [SerializeField] QuestTacker _questTackerPrefab;
    [SerializeField] CategoryColor[] _categoryColors;


    void Start()
    {
        QuestManager.Instance._onQuestRegistered += CreateQuestTracker;
        foreach( var quest in QuestManager.Instance.ActiveQuests)
        {
            CreateQuestTracker(quest);
        }
    }
    void OnDestroy()
    {
        if(QuestManager.Instance)
        {
            QuestManager.Instance._onQuestRegistered -= CreateQuestTracker;
        }
    }
    void CreateQuestTracker(Quest quest)
    {
        var categoryColor = _categoryColors.FirstOrDefault(x => x.category == quest.Category);
        var color = categoryColor.category == null ? Color.white : categoryColor.color;
        Instantiate(_questTackerPrefab, transform).Setup(quest, color);
    }
    [System.Serializable]
    struct CategoryColor
    {
        public Category category;
        public Color color;
    }
}
