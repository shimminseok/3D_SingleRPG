using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskDescriptor : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] Color _normalColor;
    [SerializeField] Color _taskCompleteColor;
    [SerializeField] Color _taskSuccessCountColor;

    [SerializeField] Color _strikeThroughColor;

    public void UpdateText(string text)
    {
        _text.text = text;
    }
    public void UpdateText(Task task)
    {
        if(task.IsComplete)
        {
            var colorCode = ColorUtility.ToHtmlStringRGB(_taskCompleteColor);
            _text.text = BuildText(task, colorCode, colorCode);
        }
        else
        {
            _text.text = BuildText(task, ColorUtility.ToHtmlStringRGB(_normalColor), ColorUtility.ToHtmlStringRGB(_taskSuccessCountColor));
        }
    }
    public void UpdateTextUsingStrikeThrough(Task task)
    {
        var colorCode = ColorUtility.ToHtmlStringRGB(_strikeThroughColor);
        _text.text = BuildText(task, colorCode, colorCode);
    }
    string BuildText(Task task, string textColorCode, string successCountColorCode)
    {
        return $"<color=#{textColorCode}>¡Ü {task.Description} <color=#{successCountColorCode}>{task.CurrentSuccess}</color> / {task.NeedSuccessToComplete}</color>";
    }
}
