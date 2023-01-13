using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
public enum QuestState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WaitingForCompletion
}

[CreateAssetMenu(menuName = "Quest/Quest",fileName = "Quest_")]
public class Quest : ScriptableObject
{

    #region Event
    public delegate void TaskSuccessChangedHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    public delegate void CompletedHandler(Quest quest);
    public delegate void CancledHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup);
    #endregion
    [SerializeField] Category _category;
    [SerializeField] Sprite _icon;

    [Header("Text")]
    [SerializeField] string _codeName;
    [SerializeField] string _displayName;
    [SerializeField,TextArea] string _description;

    [Header("Task")]
    [SerializeField] TaskGroup[] _taskGroups;

    [Header("Reword")]
    [SerializeField] Reword[] _rewards;
    [SerializeField] string[] _rewardValues;

    [Header("Option")]
    [SerializeField] bool _useAutoComplete;
    [SerializeField] bool _isCancelable;

    [Header("Condition")] 
    [SerializeField] Condition[] _acceptionConditions;
    [SerializeField] Condition[] _cancelConditions;

    [Header("NPC_Dialogue")]
    [SerializeField] string[] _givingQuestDialogues;
    [SerializeField] string[] _finishedQuestDialogues;
    [SerializeField] string[] _unfinishedQuestDialogues;

    int _currentTaskGroupIndex;
    public Category Category => _category;
    public Sprite Icon => _icon;
    public string CodeName => _codeName;
    public string DisplayName => _displayName;
    public string Description => _description;
    public string[] GivingQuestDialogues => _givingQuestDialogues;
    public string[] FinishedQuestDialogues => _finishedQuestDialogues;
    public string[] UnFinishedQuestDialogues => _unfinishedQuestDialogues;
    public QuestState State { get; private set; }
    public TaskGroup CurrentTaskGroup => _taskGroups[_currentTaskGroupIndex];
    public IReadOnlyList<TaskGroup> TaskGroups => _taskGroups;
    public IReadOnlyList<Reword> Rewords => _rewards;
    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsComplatable => State == QuestState.WaitingForCompletion;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => _isCancelable && _cancelConditions.All(x => x.IsPass(this));
    public bool IsAcceptable => _acceptionConditions.All(x => x.IsPass(this));
    

    public event TaskSuccessChangedHandler _onTaskSuccessChanged;
    public event CompletedHandler _onCompleted;
    public event CancledHandler _onCanceled;
    public event NewTaskGroupHandler _onNewTaskGroup;

    public void OnRegister()
    {
        Debug.Assert(!IsRegistered, "This quest has already been registered");
        foreach(var taskGroup in _taskGroups)
        {
            taskGroup.Setup(this);
            foreach(var task in taskGroup.Tasks)
            {
                task._onSuccessChanged += OnSuccessChanged;
            }
        }
        State = QuestState.Running;
        CurrentTaskGroup.Start();
    }
    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(IsRegistered, "This quest has already been registered");
        Debug.Assert(!IsCancel, "This quest has been canceled");

        if (IsComplete) 
        {
            return;
        }

        CurrentTaskGroup.ReceiveReport(category, target, successCount);
        if(CurrentTaskGroup.IsAllTaskComplete)
        {
            if (_currentTaskGroupIndex + 1 == _taskGroups.Length)
            {
                State = QuestState.WaitingForCompletion;
                if(_useAutoComplete)
                {
                    Complete();
                }
            }
            else
            {
                var prevTaskGroup = _taskGroups[_currentTaskGroupIndex++];
                prevTaskGroup.End();
                CurrentTaskGroup.Start();
                _onNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTaskGroup);
            }
        }
        else
        {
            State = QuestState.Running;
        }
    }
    public void Complete()
    {
        CheckIsRunning();
        foreach (var taskGroup in _taskGroups)
        {
            taskGroup.Complete();
        }
        State = QuestState.Complete;

        int index = 0;
        foreach(var reword in _rewards)
        {
            reword.Quantity = int.Parse(_rewardValues[index++]);
            reword.Give(this);
        }
        _onCompleted?.Invoke(this);

        _onTaskSuccessChanged = null;
        _onCompleted = null;
        _onCanceled = null;
        _onNewTaskGroup = null;

    }
    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(IsCancelable, "This quest can't be canceled");
        State = QuestState.Cancel;
        _onCanceled?.Invoke(this);

    }
    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone._taskGroups = _taskGroups.Select(x => new TaskGroup(x)).ToArray();
        return clone;
    }
    
    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData
        {
            _codeName = this._codeName,
            _state = State,
            _taskGroupIndex = _currentTaskGroupIndex,
            _taskSuccessCounts = CurrentTaskGroup.Tasks.Select(x => x.CurrentSuccess).ToArray()
        };
    }
    public void LoadFrom(QuestSaveData saveData)
    {
        State = saveData._state;
        _currentTaskGroupIndex = saveData._taskGroupIndex;
        for(int n =0; n< _currentTaskGroupIndex; n++)
        {
            var taskGroup = _taskGroups[n];
            taskGroup.Start();
            taskGroup.Complete();
        }
        for(int n =0; n< saveData._taskSuccessCounts.Length;n++)
        {
            CurrentTaskGroup.Start();
            CurrentTaskGroup.Tasks[n].CurrentSuccess = saveData._taskSuccessCounts[n];
        }
    }
    void OnSuccessChanged(Task task, int currentSuccess, int prevSuccess)
        => _onTaskSuccessChanged?.Invoke(this, task, currentSuccess, prevSuccess);


    [Conditional("UNITY_EDITOR")]
    private void CheckIsRunning()
    {
        Debug.Assert(IsRegistered, "This quest has already been registered");
        Debug.Assert(!IsCancel, "This quest has been canceled.");
        Debug.Assert(!IsComplete, "This quest has already been completed");
    }
}
