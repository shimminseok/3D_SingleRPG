using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskState
{
    Inactive,
    Running,
    Complete
}

[CreateAssetMenu(menuName = "Quest/Task/Task",fileName ="Task_")]
public class Task : ScriptableObject
{
    #region Event
    public delegate void StateChangedHandler(Task task, TaskState currentState, TaskState prevState);
    public delegate void SuccessChangedHandler(Task task, int currentSuccess, int prevSuccess);
    #endregion
    [SerializeField] Category _category;
    [Header("Text")]
    [SerializeField] string _codeName;
    [SerializeField] string _description;

    [Header("Action")]
    [SerializeField] TaskAction _action;

    [Header("Target")]
    [SerializeField] TaskTarget[] _targets;

    [Header("Setting")]
    [SerializeField] InitialSuccessValue _initialSuccessValue;
    [SerializeField] int _needSuccessToComplate;
    //Task가 완료되었어도 계속 성공횟수를 보고하는 옵션
    [SerializeField] bool _canReceiveReportDuringCompletion;

    TaskState _state;
    int _currentSuccess;

    public event StateChangedHandler _onSateChanged;
    public event SuccessChangedHandler _onSuccessChanged;
    public int CurrentSuccess 
    {
        get => _currentSuccess;
        set
        {
            int prevSuccess = _currentSuccess;
            _currentSuccess = Mathf.Clamp(value, 0, _needSuccessToComplate);
            if (_currentSuccess != prevSuccess)
            {
                State = _currentSuccess == _needSuccessToComplate ? TaskState.Complete : TaskState.Running;
                _onSuccessChanged?.Invoke(this, _currentSuccess, prevSuccess);
            }
        }
    }
    public Category Category => _category;
    public string CodeName => _codeName;
    public string Description => _description;
    public int NeedSuccessToComplete => _needSuccessToComplate;
    public TaskState State
    {
        get => _state;
        set
        {
            var prevState = _state;
            _state = value;
            _onSateChanged?.Invoke(this, _state, prevState);
        }
    }
    public bool IsComplete => State == TaskState.Complete;
    public Quest Owner { get; private set; }

    public void Setup(Quest owner)
    {
        Owner = owner;
    }
    public void Start()
    {
        State = TaskState.Running;
        if(_initialSuccessValue)
        {
            CurrentSuccess = _initialSuccessValue.GetValue(this);
        }
    }
    public void End()
    {
        _onSateChanged = null;
        _onSuccessChanged = null;
    }
    public void ReceieveReport(int successCount)
    {
        CurrentSuccess = _action.Run(this, CurrentSuccess, successCount);
    }
    public void Complete()
    {
        CurrentSuccess = _needSuccessToComplate;
    }
    public bool IsTarget(string category ,object target)
        => Category == category && 
        _targets.Any(x => x.IsEqual(target)) &&
        (!IsComplete || (IsComplete && _canReceiveReportDuringCompletion));
}
