using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBase : MonoBehaviour
{
    [SerializeField] GameObject _hitBox;
    [SerializeField] public float _damage;
    [SerializeField] public float _defence;
    [SerializeField] float _finalDam;
    [SerializeField] float _finalDef;
    [SerializeField] int _lv = 1;
    public string _name {get;set;}
    public GameObject _target;
    public bool _isDie;

    public float _skillDam { get; private set; }
    public float _mountItemDam { get; private set; }
    public float _buffDam { get; private set; }

    public float _mountItemDef { get; private set; }
    public float _maxHp { get; protected set; }
    public float FinalDam
    {
        get { return _finalDam; }
        set
        {
            _finalDam = value;
        }
    }
    public float FinalDef
    { 
        get { return _finalDef; }
        set
        {
            _finalDef = value;
        }
    }
    public int Level
    {
        get { return _lv; }
        set
        {
            _lv = value;
        }
    }
    public void HitBoxActive(int b = 0) => _hitBox.SetActive(System.Convert.ToBoolean(b));

    public float MountItemDam(float dam) => _mountItemDam = dam;
    public float MountItemDef(float def) => _mountItemDef = def;
    public float SkillDam(float dam = 0) => _skillDam = dam;
    public float BuffDam(float dam = 0) => _buffDam = dam;


    public float FinalDamage(float addDam = 0) => (FinalDam = Mathf.RoundToInt(_damage + addDam));
    public float FinalDefence(float addDef = 0) => (_finalDef = _defence + addDef);
    public abstract void Attack();
    public abstract void EndAttack();
    public void AttackColliderControll()
    {
        _hitBox.SetActive(true);
    }
    public void AttackSound(int n)
    {
        AudioManager._instance.AttackSound(n);
    }
}
