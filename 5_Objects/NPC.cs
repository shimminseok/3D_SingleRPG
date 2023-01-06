using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject _window;
    [SerializeField] protected string _name;
    [SerializeField] protected Text _txtDialogue;
    [SerializeField] protected Text _txtName;
    [SerializeField] protected Button[] _buttons;
    //юс╫ц
    [SerializeField] protected string[] _dialogue;

    Animator _animator;
    public bool _isInterPlay;
    public GameObject _cameraRoot;

    protected bool _isNext = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public IEnumerator StartDialogue()
    {
        ActiveButton(false);
        for (int n = 0; n < _dialogue.Length; n++)
        {
            _txtDialogue.text = string.Empty;
            string temp = string.Format(_dialogue[n], GameManager._instance._character._name);
            for (int m = 0; m < temp.Length; m++)
            {
                yield return new WaitForSeconds(0.1f);
                if (Input.anyKey)
                {
                    if (m < _dialogue[n].Length)
                    {
                        m = _dialogue[n].Length;
                        _txtDialogue.text = string.Empty;
                        _txtDialogue.text = _dialogue[n];
                        break;
                    }
                }
                _txtDialogue.text += temp[m];
            }
            _isNext = false;
            yield return new WaitUntil(() => _isNext == true);
        }
        ActiveButton(true);

    }
    public void ActiveButton(bool b)
    {
        for (int n = 0; n < _buttons.Length; n++)
        {
            _buttons[n].gameObject.SetActive(b);
        }
    }
}
