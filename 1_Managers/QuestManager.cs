using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class QuestManager : MonoBehaviour
{
    #region Save Path
    const string kSaveRootPath = "questSystem";
    const string kActiveQuestsSavePath = "questSystem";
    const string kCompleteQusetsSavePath = "questSystem";
    const string kActiveAchievementsSavePath = "questSystem";
    const string kCompleteAchievementsSavePath = "questSystem";
    #endregion

    #region Event
    public delegate void QuestRegisteredHandler(Quest newQuest);
    public delegate void QuestCompletedHandler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);
    #endregion

    static QuestManager _instance;
    static bool _isApplicationQuitting;

    public static QuestManager Instance
    {
        get
        {
            if(!_isApplicationQuitting && _instance == null)
            {
                _instance = FindObjectOfType<QuestManager>();
                if(_instance == null)
                {
                    _instance = new GameObject("Quest System").AddComponent<QuestManager>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }
            return _instance;
        }
    }

    List<Quest> _activeQuests = new List<Quest>();
    List<Quest> _completedQuests = new List<Quest>();

    List<Quest> _activeAchievement = new List<Quest>();
    List<Quest> _completedAchievment = new List<Quest>();

    public IReadOnlyList<Quest> ActiveQuests => _activeQuests;
    public IReadOnlyList<Quest> CompletedQuests => _completedQuests;

    public IReadOnlyList<Quest> ActiveAcievements => _activeAchievement;
    public IReadOnlyList<Quest> CompletedAcievements => _completedAchievment;

    QuestDataBase _questDatabase;
    QuestDataBase _achievementDatabase;

    public event QuestRegisteredHandler _onQuestRegistered;
    public event QuestCompletedHandler _onQuestCompleted;
    public event QuestCanceledHandler _onQuestCanceled;

    public event QuestRegisteredHandler _onAchievementRegistered;
    public event QuestCompletedHandler _onAchievementCompleted;
    void Awake()
    {
        _questDatabase = Resources.Load<QuestDataBase>("QuestDatabase");
        _achievementDatabase = Resources.Load<QuestDataBase>("AchieventmentDataBase");

        if(!Load())
        {
            foreach (var achievement in _achievementDatabase.Quests)
                Register(achievement);
        }
    }
    //세이브 함수를 통해 세이브 시켜주면됨.

    public Quest Register(Quest quest)
    {
        var newQuest = quest.Clone();
        if(newQuest is Achievement)
        {
            newQuest._onCompleted += OnAchievementCompleted;
            _activeAchievement.Add(newQuest);
            newQuest.OnRegister();
            _onAchievementRegistered?.Invoke(newQuest);
        }
        else
        {
            newQuest._onCompleted += OnQuestCompleted;
            newQuest._onCanceled += OnQuestCanceled;

            _activeQuests.Add(newQuest);

            newQuest.OnRegister();
            _onQuestRegistered?.Invoke(newQuest);
        }

        return newQuest;
    }
    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(_activeQuests, category, target, successCount);
        ReceiveReport(_activeAchievement, category, target, successCount);
    }

    public void ReceiveReport(Category category, TaskTarget target, int successCount)
        => ReceiveReport(category.CodeName, target.Value, successCount);
    void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
    {
        foreach(var quest in quests.ToArray())
        {
            quest.ReceiveReport(category, target, successCount);
        }
    }
    public bool ContainsInActiveQuests(Quest quest) => _activeQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInCompleteQuests(Quest quest) => _completedQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInActiveAchievements(Quest quest) => _activeAchievement.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInCompleteAchievements(Quest quest) => _completedAchievment.Any(x => x.CodeName == quest.CodeName);

    public void Save()
    {
        var root = new JObject();
        root.Add(kActiveQuestsSavePath, CreateSaveDatas(_activeQuests));
        root.Add(kCompleteQusetsSavePath, CreateSaveDatas(_completedQuests));
        root.Add(kActiveAchievementsSavePath, CreateSaveDatas(_activeAchievement));
        root.Add(kCompleteAchievementsSavePath, CreateSaveDatas(_completedAchievment));

        PlayerPrefs.SetString(kSaveRootPath, root.ToString());
        PlayerPrefs.Save();
    }
    bool Load()
    {
        if (PlayerPrefs.HasKey(kSaveRootPath))
        {
            var root = JObject.Parse(PlayerPrefs.GetString(kSaveRootPath));
            LoadSavaDatas(root[kActiveQuestsSavePath], _questDatabase, LoadActiveQuest);
            LoadSavaDatas(root[kCompleteQusetsSavePath], _questDatabase, LoadCompletedQuest);

            LoadSavaDatas(root[kActiveAchievementsSavePath], _achievementDatabase, LoadActiveQuest);
            LoadSavaDatas(root[kCompleteAchievementsSavePath], _achievementDatabase, LoadCompletedQuest);

            return true;
        }
        else
            return false;
    }
    JArray CreateSaveDatas(IReadOnlyList<Quest> quests)
    {
        var saveDatas = new JArray();
        foreach(var quest in quests)
        {
            saveDatas.Add(JObject.FromObject(quest.ToSaveData()));
        }
        return saveDatas;
    }
    void LoadSavaDatas(JToken datasToken, QuestDataBase database, System.Action<QuestSaveData,Quest> onSuccess)
    {
        var datas = datasToken as JArray;
        foreach (var data in datas)
        {
            var saveData = data.ToObject<QuestSaveData>();
            var quest = database.FindQuestBy(saveData._codeName);
            onSuccess.Invoke(saveData, quest);
        }
    }
    void LoadActiveQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = Register(quest);
        newQuest.LoadFrom(saveData);
    }
    void LoadCompletedQuest(QuestSaveData saveData, Quest quest)
    {
        var newQuest = quest.Clone();
        newQuest.LoadFrom(saveData);
        if(newQuest is Achievement)
        {
            _completedAchievment.Add(newQuest);
        }
        else
        {
            _completedQuests.Add(newQuest);
        }
    }
    #region CallBack
    void OnQuestCompleted(Quest quest)
    {
        _activeQuests.Remove(quest);
        _completedQuests.Add(quest);

        _onQuestCompleted?.Invoke(quest);
    }
    void OnQuestCanceled(Quest quest)
    {
        _activeQuests.Remove(quest);
        _onQuestCanceled?.Invoke(quest);

        Destroy(quest, Time.deltaTime);
    }
    void OnAchievementCompleted(Quest achievement)
    {
        _activeAchievement.Remove(achievement);
        _completedAchievment.Add(achievement);

        _onAchievementCompleted?.Invoke(achievement);
    }
    #endregion
}
