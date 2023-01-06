using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System.Linq;
public class ObjectPoolingManager : MonoBehaviour
{
    static ObjectPoolingManager _uniqueInstance;

    public static ObjectPoolingManager _instance => _uniqueInstance;

    [SerializeField] Transform[] _root;

    List<Queue<GameObject>> _poolingMonsterObjectQueue = new List<Queue<GameObject>>();
    List<Queue<GameObject>> _poolingObjQueList = new List<Queue<GameObject>>();

    public List<Queue<GameObject>> MonsterObject => _poolingMonsterObjectQueue;

    private void Awake()
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
        Initialize(10, DefineEnumHelper.PoolingObj.HPParticle);
        Initialize(2, DefineEnumHelper.PoolingObj.Buff);
        Initialize(5, DefineEnumHelper.PoolingObj.FootStepEffect);
        Initialize(20, DefineEnumHelper.MonsterObj.Slime);
        Initialize(20, DefineEnumHelper.MonsterObj.SoldierSlime);
        Initialize(20, DefineEnumHelper.MonsterObj.VikingSlime);
        Initialize(20, DefineEnumHelper.MonsterObj.KingSlime);
    }
    public void Initialize(int initCount, DefineEnumHelper.PoolingObj kind)
    {
        Queue<GameObject> objQueue = new Queue<GameObject>();
        for (int n = 0; n < initCount; n++)
        {
            objQueue.Enqueue(CreateNewObj(kind));
        }
        _poolingObjQueList.Add(objQueue);
    }
    public void Initialize(int initCount, DefineEnumHelper.MonsterObj kind)
    {
        Queue<GameObject> objQueue = new Queue<GameObject>();
        for (int n = 0; n < initCount; n++)
        {
            objQueue.Enqueue(CreateNewObj(kind));
        }
        _poolingMonsterObjectQueue.Add(objQueue);
    }
    public GameObject CreateNewObj(DefineEnumHelper.PoolingObj kind)
    {
        var newObj = Instantiate(ResoucePollManager._instance.GetEffect(kind));
        newObj.transform.SetParent(_root[(int)kind]);
        newObj.gameObject.SetActive(false);
        return newObj;
    }
    public GameObject CreateNewObj(DefineEnumHelper.MonsterObj kind)
    {
        var newObj = Instantiate(ResoucePollManager._instance.GetMonsterObject(kind));
        newObj.transform.SetParent(transform);
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    public GameObject GetObject(DefineEnumHelper.PoolingObj kind, Transform parent = null)
    {
        if (_poolingObjQueList[(int)kind].Count > 0)
        {
            GameObject obj = _poolingObjQueList[(int)kind].Dequeue();
            obj.transform.SetParent(parent);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObj(kind);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            Destroy(newObj,2);
            return newObj;
        }
    }
    public GameObject GetObject(DefineEnumHelper.MonsterObj kind, Transform parent = null)
    {
        if (_poolingMonsterObjectQueue[(int)kind].Count > 0)
        {
            GameObject obj = _poolingMonsterObjectQueue[(int)kind].Dequeue();
            obj.transform.SetParent(parent);
            obj.gameObject.SetActive(true);
            return obj;
        }
        return null;
    }
    public void ReturnObj(DefineEnumHelper.PoolingObj kind, GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_root[(int)kind]);
        _poolingObjQueList[(int)kind].Enqueue(obj);
    }
    public void ReturnObj(DefineEnumHelper.MonsterObj kind, GameObject obj)
    {
        obj.transform.SetParent(transform);
        _poolingMonsterObjectQueue[(int)kind].Enqueue(obj);
        MonsterController mon = obj.GetComponent<MonsterController>();
        StartCoroutine(mon.Respawn());
    }
}
