using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestTacker : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] TaskDescriptor _taskDescriptorPrefab;

    Dictionary<Task, TaskDescriptor> _taskDesciptorByTask = new Dictionary<Task, TaskDescriptor>();

    Quest _targetQuest;

    private void OnDestroy()
    {
        if (_targetQuest != null)
        {
            _targetQuest._onNewTaskGroup -= UpdateTaskDescriptos;
            _targetQuest._onCompleted -= DestroySelf;
        }
        foreach (var tuple in _taskDesciptorByTask)
        {
            var task = tuple.Key;
            task._onSuccessChanged -= UpdateText;
        }
    }
    public void Setup(Quest targetQuest, Color titleColor)
    {
        _targetQuest = targetQuest;
        _text.text = targetQuest.Category == null ?
            targetQuest.DisplayName : $"[{targetQuest.Category.DisplayName}] {targetQuest.DisplayName}";
        _text.color = titleColor;

        targetQuest._onNewTaskGroup += UpdateTaskDescriptos;
        targetQuest._onCompleted += DestroySelf;

        var taskGroups = targetQuest.TaskGroups;
        UpdateTaskDescriptos(targetQuest, taskGroups[0]);

        if(taskGroups[0] != targetQuest.CurrentTaskGroup)
        {
            for(int n = 1; n< taskGroups.Count; n++)
            {
                var taskGroup = taskGroups[n];
                UpdateTaskDescriptos(targetQuest, taskGroup, taskGroups[n - 1]);
                if (taskGroup == targetQuest.CurrentTaskGroup)
                    break;
            }
        }

    }

    private void UpdateTaskDescriptos(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroups = null)
    {
        foreach (var task in currentTaskGroup.Tasks)
        {
            var taskDesciptor = Instantiate(_taskDescriptorPrefab,transform);
            taskDesciptor.UpdateText(task);
            task._onSuccessChanged += UpdateText;

            _taskDesciptorByTask.Add(task, taskDesciptor);
        }
        if(prevTaskGroups != null)
        {
            foreach (var task in prevTaskGroups.Tasks)
            {
                var taskDesriptor = _taskDesciptorByTask[task];
                taskDesriptor.UpdateTextUsingStrikeThrough(task);
            }
        }
    }
    void UpdateText(Task task, int currentSuccess,int prvSuccess)
    {
        _taskDesciptorByTask[task].UpdateText(task);
    }
    void DestroySelf(Quest quest)
    {
        Destroy(gameObject);
    }
}
