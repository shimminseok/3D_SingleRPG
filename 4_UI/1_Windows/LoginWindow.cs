using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginWindow : MonoBehaviour
{
    [SerializeField] InputField _inputID;
    [SerializeField] InputField _inputPW;



    public void CheackLogin()
    {
        // 임시
        StartCoroutine(LoaddingScene("IngameScene"));
        // 씬이동
    }
    public void SucceedLogin()
    {
        //디비에서 확인해야됨
    }
    public void FailLogin()
    {

    }

    //임시
    public IEnumerator LoaddingScene(string sceneName)
    {
        GameObject go = Instantiate(ResoucePollManager._instance.GetWindowObj(DefineEnumHelper.WindowObj.LoadingWindow));
        LoadingWindow wnd = go.GetComponent<LoadingWindow>();
        StartCoroutine(wnd.OpenWindow());
        AudioManager._instance.BGMSoundController(DefineEnumHelper.BGMKind.LoadingScene);
        yield return new WaitForSeconds(1);
        AsyncOperation aOper = SceneManager.LoadSceneAsync(sceneName);
        while (!aOper.isDone)
        {
            wnd.SetLoaddingProgress(aOper.progress);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        wnd.SetLoaddingProgress(aOper.progress);
        Scene _curScene = SceneManager.GetActiveScene();
        //if (_curScene.name.Equals("LoginScene"))
        //{
        //    LoginScene();
        //}
        //else if (_curScene.name.Equals("InGameScene"))
        //{
        //    InGameScene();
        //}
        yield return new WaitForSeconds(1);
        while (wnd != null)
        {
            yield return null;
        }
    }

}
