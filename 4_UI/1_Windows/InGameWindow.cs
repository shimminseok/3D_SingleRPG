using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameWindow: MonoBehaviour
{
    [SerializeField] GameObject _optionWindow;

    [SerializeField] Text _level;
    [SerializeField] Text _name;
    [SerializeField] Image _hpBar;
    [SerializeField] Image _mpBar;
    [SerializeField] Text _hpTxt;
    [SerializeField] Text _mpTxt;
    [SerializeField] Text _message;
    [SerializeField] GameObject _interationImage;
    [SerializeField] Slider _exBar;
    [SerializeField] SkillData[] _skillData;

    CharacterCtrl _character;
    Animator _animator;
    public SkillData[] SkillData => _skillData;

    KeyCode _openOptionKey;
    public string LevelText
    {
        get { return _level.text; }
        set
        {
            _level.text = string.Format("Lv : {0}", value);
            _animator.SetTrigger("LevelUp");
        }
    }
    void Start()
    {
        _character = GameManager._instance._character;
        _name.text = _character._name;
        _level.text = string.Format("Lv : {0}", _character.Level);
        _hpBar.fillAmount = _character.CurHP / _character._maxHp;
        _hpTxt.text = string.Format("{0} / {1}", _character.CurHP, _character._maxHp);

        _mpBar.fillAmount = _character.CurMP / _character._maxMp;
        _mpTxt.text = string.Format("{0} / {1}", _character.CurMP, _character.CurMP);

        _animator = GetComponent<Animator>();

        _openOptionKey = HotKeyManager._instance.OpenOptionKey;
        StartCoroutine(HotKeyManager._instance.OpenWindow(_optionWindow, _openOptionKey));
    }
    public void HpBarViewer(float curHp, float maxHp)
    {
        _hpBar.fillAmount = curHp / maxHp;
        _hpTxt.text = string.Format("{0} / {1}", curHp, maxHp);
        UIManager._instance._characterInfoWindow.UpdateCharacterInfo();
    }
    public void MpBarViewer(float curMp, float maxMp)
    {
        _mpBar.fillAmount = curMp / maxMp;
        _mpTxt.text = string.Format("{0} / {1}", curMp, maxMp);
        UIManager._instance._characterInfoWindow.UpdateCharacterInfo();
    }
    public void UpdateExBarViewer(float curEx, float maxEx)
    {
        _exBar.value = curEx;
        _exBar.maxValue = _character._maxEx;
    }
    public void Message(string msg)
    {
        _message.text = string.Empty;
        _message.text = msg;
        _animator.SetTrigger("OpenText");
    }
    public void InterationNpc(bool b)
    {
        _interationImage.SetActive(b);
        _animator.SetBool("Interation", b);
    }

    public void TestButton()
    {
        _character.LevelUp();
    }


}
