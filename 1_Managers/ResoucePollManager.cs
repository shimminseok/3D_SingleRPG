using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResoucePollManager : MonoBehaviour
{
    static ResoucePollManager _uniqueInstance;
    public static ResoucePollManager _instance => _uniqueInstance;

    [SerializeField] GameObject[] _effects;
    [SerializeField] GameObject[] _windows;
    [SerializeField] Sprite[] _loadingWindowImage;
    [SerializeField] Sprite[] _itemImage;
    [SerializeField] Sprite[] _questItemImage;
    [SerializeField] Sprite[] _skillImage;
    [SerializeField] GameObject[] _uiFrepabs;
    [SerializeField] GameObject[] _monsterPrefabs;

    void Awake()
    {
        if (_uniqueInstance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public GameObject GetEffect(DefineEnumHelper.PoolingObj kind)
    {
        return _effects[(int)kind];
    }
    public GameObject GetWindowObj(DefineEnumHelper.WindowObj kind)
    {
        return _windows[(int)kind];
    }
    public GameObject GetMonsterObject(DefineEnumHelper.MonsterObj kind )
    {
        return _monsterPrefabs[(int)kind];
    }
    public GameObject GetUIObject(int key)
    {
        return _uiFrepabs[key];
    }
    public Sprite GetItemImage(int key)
    {
        return _itemImage[key];
    }
    public Sprite GetQuestItemImage(int key)
    {
        return _questItemImage[key];
    }
    public Sprite GetSkillImage(int index)
    {
        return _skillImage[index];
    }
    public Sprite GetLoadingWindowImage(int index)
    {
        return _loadingWindowImage[index];
    }
}

