using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillController : MonoBehaviour
{
    Animator _animator;
    CharacterCtrl _character;
    bool _isActiveSkill;
    InGameWindow _window;

    float _spendMP;

    Dictionary<KeyCode, SkillData> _skillDic = new Dictionary<KeyCode, SkillData>();
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _character = GetComponent<CharacterCtrl>();
    }
    void Start()
    {
        _window = UIManager._instance._inGameWindow;
        for (int n = 0; n < _window.SkillData.Length; n++)
        {
            _skillDic.Add(_window.SkillData[n]._keyCode, _window.SkillData[n]);
        }
    }
    //public void Skill_A(SkillData data)
    //{
    //    data.SetSkillData(_character.Level);
    //    if (data._canSkill)
    //    {
    //        if (Input.GetKeyUp(data._keyCode))
    //        {
    //            if (_character.CurMP >= _spendMP)
    //            {
    //                _character._isSkill = true;
    //                data._canSkill = false;
    //                _spendMP = data._spendMp;
    //                _character.SkillDam(_character._damage * data._damage);
    //                _character.CurMP -= _spendMP;
    //                StartCoroutine(data.StartCoolTime());
    //                _animator.SetTrigger("SkillA");
    //                _character.HitBoxActive(0);
    //                _character.WeaponTrail(true);
    //                AudioManager._instance.SkillSound(DefineEnumHelper.SkillKind.JumpAttack);
    //            }
    //            else
    //            {
    //                UIManager._instance._inGameWindow.Message("마나가 부족합니다.");
    //            }
    //        }
    //    }
    //}
    //public void Skill_B(SkillData data)
    //{
    //    CanUseSkill(data);
    //    data.SetSkillData(_character.Level);
    //    if (data._canSkill)
    //    {
    //        if (Input.GetKeyDown(data._keyCode) && !_isActiveSkill)
    //        {
    //            if (_character.CurMP >= data._spendMp)
    //            {
    //                _animator.SetTrigger("SkillB");
    //                _character._isSkill = true;
    //                StartCoroutine(StartWheelWind(data));
    //            }
    //            else
    //            {
    //                _window.Message("마나가 부족합니다.");
    //            }
    //        }
    //    }
    //}
    //public void Skill_C(SkillData data)
    //{
    //    if (CanUseSkill(data))
    //    {

    //    }
    //    data.SetSkillData(_character.Level);
    //    if (data._canSkill)
    //    {
    //        if (Input.GetKeyDown(data._keyCode))
    //        {
    //            if (_character.CurMP >= data._spendMp)
    //            {
    //                _character.CurMP -= _spendMP;
    //                data._canSkill = false;
    //                GameObject go = ObjectPoolingManager._instance.GetObject(DefineEnumHelper.PoolingObj.Buff, transform);
    //                go.transform.position = transform.position;
    //                go = Instantiate(ResoucePollManager._instance.GetEffect(DefineEnumHelper.PoolingObj.BuffEffect), transform);
    //                StartCoroutine(StartBuffeOn(data, 20, go));
    //                StartCoroutine(data.StartCoolTime());
    //            }
    //            else
    //            {
    //                _window.Message("마나가 부족합니다.");
    //            }
    //        }
    //    }
    //}
    IEnumerator StartWheelWind(SkillData data)
    {
        _isActiveSkill = true;
        data._canSkill = false;
        _animator.SetBool("SkillActive", _isActiveSkill);
        _character.SkillDam(_character._damage * data._damage);
        _character.WeaponTrail(true);
        while (Input.GetKey(data._keyCode) && _isActiveSkill)
        {
            yield return new WaitForSeconds(0.5f);
            if (_character.CurMP < data._spendMp)
            {
                _window.Message("마나가 부족합니다.");
                break;
            }
            _character.CurMP -= data._spendMp;
        }
        _isActiveSkill = false;
        _animator.SetBool("SkillActive", _isActiveSkill);
        _character.WeaponTrail(false);
        StartCoroutine(data.StartCoolTime());
        _animator.ResetTrigger("SkillB");
    }
    IEnumerator StartBuffeOn(SkillData data, float duration, GameObject effect)
    {
        _character.BuffDam(_character._damage * data._damage);
        yield return new WaitForSeconds(duration);
        Destroy(effect);
        _character.BuffDam();
    }

    bool CanUseSkill(SkillData data)
    {
        if (data._data._acquireLv > _character.Level)
        {
            UIManager._instance._inGameWindow.Message(string.Format("레벨이 부족합니다 (필요레벨 : {0})", data._data._acquireLv));
            return false;
        }
        return true;
    }
    public void UsedSkill(KeyCode key)
    {
        _skillDic.TryGetValue(key, out SkillData data);
        if (data != null)
        {
            data.SetSkillData(_character.Level);
            if (CanUseSkill(data))
            {
                if (data._canSkill)
                {
                    if (Input.GetKeyDown(data._keyCode))
                    {
                        SkillKind(data, (DefineEnumHelper.SkillKind)data._data._imageIndex);
                    }
                }
            }
        }
    }
    void SkillKind(SkillData data, DefineEnumHelper.SkillKind kind)
    {
        switch (kind)
        {
            case DefineEnumHelper.SkillKind.JumpAttack:
                if (_character.CurMP >= data._spendMp)
                {
                    _character.IsSkill = true;
                    data._canSkill = false;
                    _character.SkillDam(_character._damage * data._damage);
                    _character.CurMP -= data._spendMp;
                    StartCoroutine(data.StartCoolTime());
                    _animator.SetTrigger("SkillA");
                    _character.HitBoxActive(0);
                    _character.WeaponTrail(true);
                }
                return;
            case DefineEnumHelper.SkillKind.WheelWind:
                if (_character.CurMP >= data._spendMp)
                {
                    _animator.SetTrigger("SkillB");
                    _character.IsSkill = true;
                    StartCoroutine(StartWheelWind(data));
                }
                return;
            case DefineEnumHelper.SkillKind.Rage:
                if (_character.CurMP >= data._spendMp)
                {
                    _character.CurMP -= _spendMP;
                    data._canSkill = false;
                    GameObject go = ObjectPoolingManager._instance.GetObject(DefineEnumHelper.PoolingObj.Buff, transform);
                    go.transform.position = transform.position;
                    go = Instantiate(ResoucePollManager._instance.GetEffect(DefineEnumHelper.PoolingObj.BuffEffect), transform);
                    StartCoroutine(StartBuffeOn(data, 20, go));
                    StartCoroutine(data.StartCoolTime());
                }
                return;
        }
        _window.Message("마나가 부족합니다.");
    }
    public void SkillSound(int index)
    {
        AudioManager._instance.SkillSound((DefineEnumHelper.SkillKind)index);
    }
}
