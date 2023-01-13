using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    GameObject _owner;
    void Awake()
    {
        _owner = transform.root.gameObject;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            AudioManager._instance.AttackSound(5);
            CharacterCtrl ctrl = _owner.GetComponent<CharacterCtrl>();
            ctrl._target = other.gameObject;
            if(!ctrl._target.Equals(other.gameObject) && ctrl._target != null)
            {
                return;
            }
            _owner.transform.LookAt(other.transform);

        }
        else if(other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
