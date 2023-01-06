using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SlimeAnimationState { Idle, Walk, Attack, Die }
public enum WolkType { Wlak, Run }
public class MonsterController : ObjectBase
{
    public Face _faces;
    public GameObject _smileBody;
    public SlimeAnimationState _currentState;

    [SerializeField] int _id;
    [SerializeField] DefineEnumHelper.MonsterObj _monsterKind;
    [SerializeField] List<DefineEnumHelper.MonsterDropItem> _dropItemID;


    //MonsterInfo
    int _gold;
    int _getEx;

    Animator _animator;
    NavMeshAgent _agent;

    Material _faceMaterial;
    Vector3 _originPos;

    WolkType _wolkType = WolkType.Wlak;

    public float _curHp;
    public UnityEngine.Events.UnityEvent _onDead;

    public float HittingMe(int dam) => (_curHp -= (dam));

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _faceMaterial = _smileBody.GetComponent<Renderer>().materials[1];
        _curHp = _maxHp;
    }
    void Start()
    {
        InitData();
    }
    void InitData()
    {
        if (DataTableManager._instance._monsterDataDic.TryGetValue(_id, out stDataTable.stMonsterInitData data))
        {
            transform.localPosition = _originPos = RandomNavSphere(GameManager._instance.MonsterSpawnPoints(_monsterKind).position, 70);
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
            _wolkType = WolkType.Wlak;
            FinalDefence();
            GetComponent<CapsuleCollider>().enabled = true;

        }
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        InitData();
        ObjectPoolingManager._instance.GetObject(_monsterKind, GameManager._instance.MonsterSpawnPoints(_monsterKind));
    }
    void Update()
    {
        if (_curHp <= 0)
        {
            _curHp = 0;
            _currentState = SlimeAnimationState.Die;
        }
        if (!_isDie)
        {
            switch (_currentState)
            {
                case SlimeAnimationState.Idle:
                    StartCoroutine(StopAgent());
                    SetFace(_faces.Idleface);
                    break;
                case SlimeAnimationState.Walk:
                    _agent.isStopped = false;
                    if (_wolkType == WolkType.Wlak)
                    {
                        if (_agent.remainingDistance < _agent.stoppingDistance)
                        {
                            _currentState = SlimeAnimationState.Idle;
                        }
                    }
                    else
                    {
                        if (_agent.remainingDistance < _agent.stoppingDistance)
                        {
                            _currentState = SlimeAnimationState.Attack;
                        }
                        else if (_agent.remainingDistance > 30)
                        {
                            _agent.SetDestination(_originPos);
                            _wolkType = WolkType.Wlak;
                        }
                        else
                        {
                            _agent.SetDestination(_target.transform.position);
                        }
                    }
                    _animator.SetFloat("Speed", _agent.velocity.magnitude);
                    break;
                case SlimeAnimationState.Attack:
                    _agent.SetDestination(_target.transform.position);
                    if (_agent.remainingDistance < _agent.stoppingDistance)
                    {
                        _agent.isStopped = true;
                        Attack();
                    }
                    else
                    {
                        _wolkType = WolkType.Run;
                        _currentState = SlimeAnimationState.Walk;
                    }
                    break;
                case SlimeAnimationState.Die:
                    _agent.isStopped = true;
                    if (!_isDie)
                    {
                        _onDead.Invoke();
                    }
                    break;
            }
        }
    }
    public void Dead()
    {
        _isDie = true;
        _animator.SetTrigger("Damage");
        _animator.SetInteger("DamageType", 2);
        GetComponent<CapsuleCollider>().enabled = false;
        ObjectPoolingManager._instance.ReturnObj(_monsterKind, gameObject);
        GameManager._instance.GetExperience(_getEx);
        GameManager._instance._character.CurMoney += _gold;
        SetFace(_faces.damageFace);
        //È¹µæÈ®·ü 30%
        int acquisitionProbability = Random.Range(0, 100);
        if (acquisitionProbability >= 30)
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
    IEnumerator StopAgent()
    {
        if (_agent.isStopped)
        {
            yield break;
        }
        _agent.isStopped = true;
        _animator.SetFloat("Speed", 0);
        yield return new WaitForSeconds(3f);
        if (_currentState == SlimeAnimationState.Attack)
        {
            yield break;
        }
        _currentState = SlimeAnimationState.Walk;
        _agent.SetDestination(RandomNavSphere(_originPos, 10));
        SetFace(_faces.WalkFace);
    }
    Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist + origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);
        return navHit.position;
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
