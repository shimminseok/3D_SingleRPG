using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillData : MonoBehaviour
{
    [SerializeField] int _key;
    public KeyCode _keyCode;

    [SerializeField] Image _skillImage;
    [SerializeField] Sprite _nullImage;
    [SerializeField] Image _coolTimeImage;
    [SerializeField] Text _keyCodeTxt;
    [SerializeField] Text _coolTimeTxt;


    string _skillName;
    float _coolTime;
    public bool _canSkill = true;
    float _cheakTime = 0;
    public float _spendMp { get; private set; }
    public float _damage { get; private set; }

    public stDataTable.stSkillData _data { get; private set; } = new stDataTable.stSkillData();
    public Sprite SkillImage
    {
        get { return _skillImage.sprite; }
        set
        {
            _skillImage.sprite = value;
        }
    }

    void Awake()
    {
        _keyCodeTxt.text = _keyCode.ToString();
        _coolTimeImage.fillAmount = 0;
        _coolTimeTxt.gameObject.SetActive(false);
        //임시
    }
    private void Start()
    {
        SetSkillData(GameManager._instance._character.Level);
    }
    public void SetSkillData(int curLv = 1)
    {
        if (DataTableManager._instance._skillDataDic.TryGetValue(_key,out stDataTable.stSkillData data))
        {
            _data = data;
            if (curLv < _data._acquireLv)
            {
                _skillImage.sprite = _nullImage;
            }
            else
            {
                _skillImage.sprite = ResoucePollManager._instance.GetSkillImage(data._imageIndex);
                _skillName = _data._name;
                _spendMp = _data._spendMp;
                _damage = _data._dam;
                _coolTime = _data._coolTime;
            }
        }
    }
    public IEnumerator StartCoolTime()
    {
        _coolTimeTxt.gameObject.SetActive(true);
        _coolTimeImage.fillAmount = 1;
        float time = _coolTime;
        while (!_canSkill)
        {
            _cheakTime += Time.deltaTime;
            _coolTimeImage.fillAmount -= Time.deltaTime / _coolTime;
            if (_cheakTime >= _coolTime)
            {
                _cheakTime = 0;
                _canSkill = true;
                _coolTimeTxt.gameObject.SetActive(false);
            }
            if (time > 1)
            {
                _coolTimeTxt.text = (time -= Time.deltaTime).ToString("F0") + "초";
            }
            else
            {
                _coolTimeTxt.text = (time -= Time.deltaTime).ToString("F1") + "초";
            }
            yield return null;
        }
    }
}

