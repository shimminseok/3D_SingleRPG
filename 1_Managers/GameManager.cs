using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

public class GameManager : MonoBehaviour
{
    static GameManager _uniqueInstance;

    public delegate int Calculate(int a, int b);

    
    public static GameManager _instance => _uniqueInstance;

    public CharacterCtrl _character
    {
        get { return Character.GetComponent<CharacterCtrl>(); }
    }
    [SerializeField] GameObject _characterPrefab;
    [SerializeField] Transform[] _monsterSpawnPoints;

    public GameObject Character { get; private set; }
    public Transform MonsterSpawnPoints(DefineEnumHelper.MonsterObj index) => _monsterSpawnPoints[(int)index];

    List<System.Tuple<int, string>> test = new List<System.Tuple<int, string>>();
    void Awake()
    {
        Calculate Calc = (a, b) => a + b;
        if (_uniqueInstance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        test.Add(new System.Tuple<int, string>(1,"a"));
        test.Add(new System.Tuple<int, string>(2,"b"));
        test.Add(new System.Tuple<int, string>(3,"c"));
        test.Add(new System.Tuple<int, string>(4,"d"));

        for(int n = 0; n< test.Count;n++)
        {
            Debug.LogFormat("{0} : {1}", test[n].Item1, test[n].Item2);
        }
        foreach(var temp in test)
        {
            Debug.LogFormat("{0} : {1}", temp.Item1, temp.Item2);
        }
    }
    private void OnEnable()
    {
        Character = Instantiate(_characterPrefab);
    }
    void Start()
    {
        GameStart();
    }
    void GameStart()
    {
        for (int n = 0; n < ObjectPoolingManager._instance.MonsterObject.Count; n++)
        {
            for (int m = 0; m < ObjectPoolingManager._instance.MonsterObject[n].Count; m++)
            {
                ObjectPoolingManager._instance.GetObject((DefineEnumHelper.MonsterObj)n, MonsterSpawnPoints((DefineEnumHelper.MonsterObj)n));
            }
        }
        AudioManager._instance.BGMSoundController(DefineEnumHelper.BGMKind.IngameScene);
    }
    public void GetExperience(float ex)
    {
        _character.CurEX += ex;
        UIManager._instance._inGameWindow.UpdateExBarViewer(_character.CurEX, _character._maxEx);
    }
    public void GetGold(int money)
    {
        _character.CurMoney += money;
    }

    public void SceneConttroller(string SceneName)
    {
        //StartCoroutine(LoaddingScene(SceneName));
    }
    //public IEnumerator LoaddingScene(string sceneName)
    //{
    //    GameObject go = Instantiate(ResoucePollManager._instance.GetWindowObj(DefineEnumHelper.WindowObj.LoadingWindow));
    //    LoadingWindow wnd = go.GetComponent<LoadingWindow>();
    //    StartCoroutine(wnd.OpenWindow());
    //    //AudioManager._instance.BGMSoundController(DefineEnumHelper.CurScene.LoadingScene);
    //    yield return new WaitForSeconds(1);
    //    AsyncOperation aOper = SceneManager.LoadSceneAsync(sceneName);
    //    while (!aOper.isDone)
    //    {
    //        wnd.SetLoaddingProgress(aOper.progress);
    //        yield return null;
    //    }
    //    wnd.SetLoaddingProgress(aOper.progress);
    //    Scene _curScene = SceneManager.GetActiveScene();
    //    //if (_curScene.name.Equals("LoginScene"))
    //    //{
    //    //    LoginScene();
    //    //}
    //    //else if (_curScene.name.Equals("InGameScene"))
    //    //{
    //    //    InGameScene();
    //    //}
    //    yield return new WaitForSeconds(1);
    //    while (wnd != null)
    //    {
    //        yield return null;
    //    }
    //}

    JArray CreateSaveData()
    {
        var saveData = new JArray();

        return saveData;
    }
    public void GameSave()
    {
        //게임 세이브

    }
}
