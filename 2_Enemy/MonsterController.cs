using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SlimeAnimationState { Idle, Roaming, TargetOn, Attack, Die }
public enum WolkType { Wlak, Run }
public class MonsterController : ObjectBase
{


    [SerializeField] int _id;
    [SerializeField] DefineEnumHelper.MonsterObj _monsterKind;
    [SerializeField] List<DefineEnumHelper.MonsterDropItem> _dropItemID;
    [SerializeField] Face _faces;
    [SerializeField] GameObject _smileBody;


    //MonsterInfo
    int _gold;
    int _getEx;

    Animator _animator;
    NavMeshAgent _agent;

    Material _faceMaterial;
    Vector3 _originPos;
    Vector3 _goalPos;

    [SerializeField] SlimeAnimationState _currentState = SlimeAnimationState.Idle;
    //WolkType _wolkType = WolkType.Wlak;

    public float _curHp;
    public UnityEngine.Events.UnityEvent _onDead;
    float _maxDistance = 30;
    bool _isArrive = true;

    public SlimeAnimationState CurState(SlimeAnimationState state) => _currentState = state;
    public float HittingMe(int dam) => (_curHp -= (dam));

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _faceMaterial = _smileBody.GetComponent<Renderer>().materials[1];
        _curHp = _maxHp;
    }
    void OnEnable()
    {
        InitData();
        StartCoroutine(MonsterStateChange());
        StartCoroutine(MonsterAction());
    }
    void InitData()
    {
        if (DataTableManager._instance._monsterDataDic.TryGetValue(_id, out stDataTable.stMonsterInitData data))
        {
            transform.position = _originPos = RandomNavSphere(GameManager._instance.MonsterSpawnPoints(_monsterKind).position, 70);
            _target = null;
            _name = data._name;
            Level = data.Level;
            _maxHp = _curHp = data._hp;
            _damage = data._dam;
            _defence = data._def;
            _agent.speed = data._speed;
            _agent.stoppingDistance = data._attDis;
            _getEx = data._ex;
            _gold = Random.Range(data._minMoney, data._maxMoney);
            FinalDamage();
            _isDie = false;
            _currentState = SlimeAnimationState.Idle;
            FinalDefence();
            GetComponent<CapsuleCollider>().enabled = true;
            _animator.applyRootMotion = false;
        }
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        //InitData();
        ObjectPoolingManager._instance.GetObject(_monsterKind, GameManager._instance.MonsterSpawnPoints(_monsterKind));
    }
    void Update()
    {
        if (_curHp <= 0)
        {
            _curHp = 0;
            _currentState = SlimeAnimationState.Die;
        }
    }
    public void Dead()
    {
        _isDie = true;
        _animator.applyRootMotion = true;
        _animator.SetTrigger("Damage");
        _animator.SetInteger("DamageType", 2);
        GetComponent<CapsuleCollider>().enabled = false;
        ObjectPoolingManager._instance.ReturnObj(_monsterKind, gameObject);
        GameManager._instance.GetExperience(_getEx);
        GameManager._instance._character.CurMoney += _gold;
        SetFace(_faces.damageFace);
        //È¹µæÈ®·ü 30%
        int acquisitionProbability = Random.Range(0, 100);
        if (acquisitionProbability >= 70)
        {
            if (_dropItemID.Count > 0)
            {
                UIManager._instance._inventoryWindow.AddItem((int)_dropItemID[0]);
                UIManager._instance._inGameWindow.Message(string.Format("{0}À» È¹µæÇÏ¿´½À´Ï´Ù.", DataTableManager._instance._itemDataDic[(int)_dropItemID[0]]._name));
            }
        }
        QuestManager.Instance.ReceiveReport("Kill", gameObject, 1);

    }
    void SetFace(Texture tex)
    {
        _faceMaterial.SetTexture("_MainTex", tex);
    }
    Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist + origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);
        return navHit.position;
    }

    IEnumerator MonsterStateChange()
    {
        while (!_isDie)
        {
            yield return new WaitForSeconds(0.1f);
            if (_target != null)
            {
                if (Vector3.Distance(_target.transform.position, transform.position) < _agent.stoppingDistance)
                    _currentState = SlimeAnimationState.Attack;
                else
                    _currentState = SlimeAnimationState.TargetOn;
            }
            else
            {
                if (_goalPos != Vector3.zero)
                {
                    if (Vector3.Distance(_goalPos,transform.position) > _agent.stoppingDistance) // ¸ñÀûÁö¿¡ µµÂø ¾ÈÇßÀ»¶§
                    {
                        _currentState = SlimeAnimationState.Roaming;
                    }
                    else                                                                        // ¸ñÀûÁö¿¡ µµÂø ÇßÀ»¶§
                    {
                        _currentState = SlimeAnimationState.Idle;
                    }
                }
                else
                {
                    _currentState = SlimeAnimationState.Idle;
                }
            }
        }
    }
    IEnumerator MonsterAction()
    {
        while (!_isDie)
        {
            switch (_currentState)
            {
                case SlimeAnimationState.Idle:
                    _agent.isStopped = true;
                    _animator.SetFloat("Speed",0);
                    _isArrive = true;
                    yield return new WaitForSeconds(3);
                    _goalPos = RandomNavSphere(_originPos, 10);
                    break;
                case SlimeAnimationState.Roaming:
                    MonsterRoaming();
                    break;
                case SlimeAnimationState.TargetOn:
                    TargetOn();
                    break;
                case SlimeAnimationState.Attack:
                    MonsterAttack();
                    break;
                case SlimeAnimationState.Die:
                    MonsterDie();
                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void MonsterRoaming()
    {
        _isArrive = false;
        _agent.isStopped = false;
        _agent.SetDestination(_goalPos);
        _animator.SetFloat("Speed", 1);
    }
    void TargetOn()
    {
        _agent.isStopped = false;
        _animator.ResetTrigger("Attack");
        _animator.SetFloat("Speed", 1);
        _agent.SetDestination(_target.transform.position);
        SetFace(_faces.jumpFace);
        if (Vector3.Distance(_target.transform.position, transform.position) < _agent.stoppingDistance)
        {
            _currentState = SlimeAnimationState.Attack;
        }
        if (Vector3.Distance(_target.transform.position, transform.position) > _maxDistance)
        {
            _target = null;
        }
    }
    void MonsterAttack()
    {
        _agent.isStopped = true;
        SetFace(_faces.attackFace);
        _animator.SetTrigger("Attack");

    }
    void MonsterDie()
    {
        _agent.isStopped = true;
        _onDead.Invoke();
    }

    public override void Attack()
    {
        _animator.SetTrigger("Attack");
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    public override void EndAttack()
    {
        Vector3 dir = _target.transform.position - transform.position;
        dir = dir.normalized;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
        HitBoxActive(0);
        _animator.ResetTrigger("Attack");
    }
}
