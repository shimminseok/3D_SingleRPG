using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public enum StatType { Level, Damege, Defence};
public class CharacterCtrl : ObjectBase
{
    [SerializeField] float _moveSpeed;
    [SerializeField] Transform[] _footStepRoots;
    [Range(0.0f, 0.3f)]
    [SerializeField] float _rotationSmoothTime = 0.12f;
    [SerializeField] float SpeedChangeRate = 10.0f;
    [SerializeField] MeleeWeaponTrail _weaponTrail;
    public UnityEngine.Events.UnityEvent _onLevelUp;

    float _curHp;
    float _curMp;
    float _curEx;
    int _curMoney = 1000000;
    public float _maxMp { get; private set; }
    public float _maxEx { get; private set; }

    float _speed;
    float _animationBlend;
    float _targetRotation = 0.0f;
    float _rotationVelocity;

    bool _isBattleMode = true;
    bool _isMove;
    bool _isAttack = false;
    bool _isSkill = false;
    Vector3 _moveVec;

    Animator _animator;
    CharacterController _controller;
    SkillController _skillCtrl;

    public float CurHP
    {
        get { return _curHp; }
        set
        {
            _curHp = value;
            UIManager._instance._inGameWindow.HpBarViewer(CurHP, _maxHp);
        }
    }
    public float CurMP
    {
        get { return _curMp; }
        set
        {
            _curMp = value;
            UIManager._instance._inGameWindow.MpBarViewer(CurMP, _maxMp);
        }
    }
    public float CurEX
    {
        get { return _curEx; }
        set
        {
            _curEx = value;
            if (CurEX > _maxEx)
            {
                _onLevelUp.Invoke();
            }
            UIManager._instance._inGameWindow.UpdateExBarViewer(CurEX, _maxEx);
        }
    }
    public bool IsSkill
    {
        get { return _isSkill; }
        set
        {
            _isSkill = value;
            if (_isSkill)
            {
                _isAttack = false;
                _isMove = false;
                _animator.SetFloat("Speed", 0);
            }
            else
            {
                _isAttack = true;
                _isMove = true;
            }
        }
    }
    public int CurMoney
    {
        get { return _curMoney; }
        set
        {
            _curMoney = value;
            UIManager._instance._inventoryWindow.MoneyText = _curMoney.ToString();
        }
    }
    public float HittingMe(int dam) => (CurHP -= dam);
    public void WeaponTrail(bool b) => _weaponTrail.Emit = b;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _skillCtrl = GetComponent<SkillController>();
    }   
    void Start()
    {
        InitData();
    }
    void Update()
    {
        Attack();
    }
    void OnGUI()
    {
        Event keyEvent = Event.current;
        KeyCode key = keyEvent.keyCode;
        if (Input.anyKeyDown)
        {
            _skillCtrl.UsedSkill(key);
        }
    }
    void FixedUpdate()
    {
        if (!_isAttack && !_isSkill)
        {
            Move();
        }
    }
    void InitData()
    {
        //임시
        _name = "테스트";
        CurHP = _maxHp = Mathf.RoundToInt((Level * (1000 / Mathf.Log(Level + 1))));
        CurMP = _maxMp = Mathf.RoundToInt((Level * (1000 / Mathf.Log(Level + 1))));
        _damage = Mathf.Round((Level * 2) + (Level + (24 + (0.25f * Level) * 3 - 20)));
        _defence = Mathf.Round((Level + ((0.25f * Level) * 1)));
        _maxEx = (Level - 1) * Level * 100;
        FinalDamage(_mountItemDam + _skillDam + _buffDam);
        FinalDefence(_mountItemDef);
    }
    public int GetValue(StatType type)
    {
        int value = 0;
        switch(type)
        {
            case StatType.Level:
                value = Level;
                break;
            case StatType.Damege:
                value = (int)FinalDam;
                break;
            case StatType.Defence:
                value= (int)FinalDef;
                break;
        }
        return value;
    }
    public void LevelUp()
    {
        Level++;
        _curEx -= _maxEx;
        InitData();
        UIManager._instance._characterInfoWindow.UpdateCharacterInfo();
        UIManager._instance._inGameWindow.LevelText = Level.ToString();
        AudioManager._instance.EffectSound(DefineEnumHelper.SFXSound.LevelUP);
        for(int n=0; n< UIManager._instance._inGameWindow.SkillData.Length;n++)
        {
            UIManager._instance._inGameWindow.SkillData[n].SetSkillData(Level);
        }
        QuestManager.Instance.ReceiveReport("Achieve", StatType.Level, Level);
    }
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector3(x, z);
        float targetSpeed = _moveSpeed;
        if (move == Vector2.zero)
        {
            targetSpeed = 0.0f;
        }
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);
        }
        else
        {
            _speed = targetSpeed;
        }
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        _animationBlend = _animationBlend < 0.01f ? 0 : _animationBlend;
        Vector3 inputDirection = new Vector3(x, 0.0f, z).normalized;
        if (x != 0 || z != 0)
        {
            _isMove = true;
            GetComponent<InteractionNPC>()._cam2.gameObject.SetActive(false);
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
            Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            _animationBlend = Mathf.Lerp(_animationBlend, 0, Time.deltaTime * SpeedChangeRate);
            _animationBlend = 0;
            _isMove = false;
        }
        _moveVec = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        _moveVec.y += Physics.gravity.y;
        _controller.Move(_moveVec * _speed * Time.deltaTime);
        _animator.SetFloat("Speed", _animationBlend);

    }
    public float ClampValue(float value, float maxValue)
    {
        return Mathf.Clamp(value, 0, maxValue);
    }
    public override void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !_isMove && _isBattleMode && !EventSystem.current.IsPointerOverGameObject())
        {
            WeaponTrail(true);
            _animationBlend = 0;
            _isAttack = true;
            _animator.SetTrigger("OnAttack");
        }
        else if(Input.GetMouseButtonDown(0) &&EventSystem.current.IsPointerOverGameObject())
        {
            AudioManager._instance.EffectSound(DefineEnumHelper.SFXSound.UIClick);
        }
    }
    public override void EndAttack()
    {
        HitBoxActive(0);
        WeaponTrail(false);
        _animator.ResetTrigger("OnAttack");
        _isAttack = false;
        _isSkill = false;
        if (_target != null)
            _target = _target.GetComponent<MonsterController>()._isDie ? null : _target;
        SkillDam();
    }
    public void FootStep(int n)
    {
        GameObject go = ObjectPoolingManager._instance.GetObject(DefineEnumHelper.PoolingObj.FootStepEffect, _footStepRoots[n]);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one * 2;
        StartCoroutine(RetrurnFootStep(go));
        AudioManager._instance.FootStepSound();
    }

    IEnumerator RetrurnFootStep(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        ObjectPoolingManager._instance.ReturnObj(DefineEnumHelper.PoolingObj.FootStepEffect, go);
    }
}
