using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event_Type
{
    GameInit,
    GameEnd,
    LevelUP,
    MountItemChange,
    HealthChange,
    ManaChange,
    ExperienceChange,
    ValueChange,
    Dead
}
public class EventManager : MonoBehaviour
{
    static EventManager _uniqueInstance;
    public static EventManager _instance => _uniqueInstance;

    public delegate void OnEvent(Event_Type eventType, Component Sender, string param = null);
    public delegate void ChangeValue(Event_Type type,float curValue, float maxValue);

    public Dictionary<Event_Type, List<OnEvent>> _eventLiteners = new Dictionary<Event_Type, List<OnEvent>>();

    public event ChangeValue _onChangeValue;

    void Awake()
    {
        if (_instance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddLitener(Event_Type type, OnEvent eventLiteners)
    {
        if (_eventLiteners.TryGetValue(type, out List<OnEvent> listenerList))
        {
            listenerList.Add(eventLiteners);
            return;
        }
        listenerList = new List<OnEvent>();
        listenerList.Add(eventLiteners);
        _eventLiteners.Add(type, listenerList);
    }
    public void AddLitener(Event_Type type, ChangeValue liteners)
    {
        _onChangeValue += liteners;
    }
    public void PostNotification(Event_Type eventType, Component sender, string param = null)
    {
        if (!_eventLiteners.TryGetValue(eventType, out List<OnEvent> listenerlist))
        {
            return;
        }
        for (int n = 0; n < listenerlist.Count; n++)
        {
            listenerlist?[n](eventType, sender, param);
        }
    }
    public void PostNotification(Event_Type type, float curValue, float maxValue)
    {
        _onChangeValue?.Invoke(type, curValue, maxValue);
    }
    public void RemoveEvent(Event_Type eventType) => _eventLiteners.Remove(eventType);
    public void RemoveRedundancies()
    {
        Dictionary<Event_Type, List<OnEvent>> newListeners = new Dictionary<Event_Type, List<OnEvent>>();
        foreach(KeyValuePair<Event_Type, List<OnEvent>> item in _eventLiteners)
        {
            for(int n = item.Value.Count - 1; n >= 0; n--)
            {
                if(item.Value[n].Equals(null))
                {
                    item.Value.RemoveAt(n);
                }
            }
            if(item.Value.Count > 0)
            {
                newListeners.Add(item.Key, item.Value);
            }
        }
        _eventLiteners = newListeners;
    }
    void OnLevelWasLoaded()
    {
        RemoveRedundancies();
    }
}
