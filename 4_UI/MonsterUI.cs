using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Image _hp;
    MonsterController _owner;
    void Awake()
    {
        _owner = transform.parent.gameObject.GetComponent<MonsterController>();
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
        HP();
    }

    void HP()
    {
        _hp.fillAmount = _owner._curHp / _owner._maxHp;
    }
}
