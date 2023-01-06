using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidePlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            MonsterController owner = GetComponentInParent<MonsterController>();
            owner._target = other.gameObject;
            owner._currentState = SlimeAnimationState.Attack;
        }
    }
}
