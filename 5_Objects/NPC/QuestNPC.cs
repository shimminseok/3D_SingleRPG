using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestNPC : NPC
{
    [SerializeField] List<Quest> _quests = new List<Quest>();
    [SerializeField] string[] _noQuest;

    bool _isSuccess;
    Quest _curQuest;
    Quest _completedQuest;
    public Quest CurQuset => _curQuest;
    public List<Quest> QuestList => _quests;
    public bool HasQuest { get; private set; } = true;
    void Awake()
    {
        _txtName.text = _name;
        _buttons[0].onClick.AddListener(() => CheakQuestState());
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            _isNext = true;
        }
    }
    void CheakQuestState()
    {
        ActiveButton(false);
        if (_curQuest is null)
        {
            if (GiveQuest())
            {
                StartCoroutine(Dialogue(_curQuest.GivingQuestDialogues));
            }
            else
            {
                StartCoroutine(Dialogue(_noQuest));
            }
        }
        else
        {
            if (QuestComplete())
            {
                StartCoroutine(Dialogue(_completedQuest.FinishedQuestDialogues));
            }
            else
            {
                StartCoroutine(Dialogue(_curQuest.UnFinishedQuestDialogues));

            }
        }
    }
    IEnumerator Dialogue(string[] text)
    {
        for (int n = 0; n < text.Length; n++)
        {
            _txtDialogue.text = string.Empty;
            for (int m = 0; m < text[n].Length; m++)
            {
                yield return new WaitForSeconds(0.1f);
                if (Input.anyKey)
                {
                    if (m < text[n].Length)
                    {
                        m = text[n].Length;
                        _txtDialogue.text = string.Empty;
                        _txtDialogue.text = text[n];
                        break;
                    }
                }
                _txtDialogue.text += text[n][m];
            }
            _isNext = n > text.Length ? true : false;
            yield return new WaitUntil(() => _isNext == true);
        }
        _txtDialogue.transform.parent.parent.gameObject.SetActive(false);
        _isInterPlay = false;
    }
    public bool RemainQuest()
    {
        return false;
    }
    public bool GiveQuest()
    {
        foreach (var quest in _quests)
        {
            if (quest.IsAcceptable && !QuestManager.Instance.ContainsInCompleteQuests(quest))
            {
                _curQuest = QuestManager.Instance.Register(quest);
                return true;
            }
        }
        return false;
    }
    bool QuestComplete()
    {
        if (QuestManager.Instance.ContainsInActiveQuests(_curQuest))
        {
            if (_curQuest.CurrentTaskGroup.IsAllTaskComplete)
            {
                _curQuest.Complete();
                _completedQuest = _curQuest;
                _curQuest = null;
                _quests.RemoveAt(0);
                return true;
            }
        }
        return false;
    }
}
