// This Script changes the player's HP when the player get's hit by a projectile

using System.Collections;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.gameObject.Equals(gameObject.transform.root.gameObject))
        {
            return;
        }
        if(other.CompareTag("CharacterProjectile"))
        {
            MonsterController owner = GetComponent<MonsterController>();
            float dam = other.gameObject.GetComponent<ProjectileScript>().Damage - owner._defence > 0 ? other.gameObject.GetComponent<ProjectileScript>().Damage - owner._defence : 1;
            gameObject.GetComponent<HPScript>().ChangeHP(-Mathf.RoundToInt(dam), other.ClosestPoint(transform.position),Color.green);
            gameObject.GetComponent<HPScript>().GetHitEffect(other.ClosestPoint(transform.position));
            owner._currentState = SlimeAnimationState.Attack;
            owner._target = other.transform.parent.gameObject;
            owner.HittingMe((int)dam);
        }
        else if(other.CompareTag("MonsterProjectile"))
        {
            CharacterCtrl owner = GetComponent<CharacterCtrl>();
            float dam = other.gameObject.GetComponent<ProjectileScript>().Damage - owner._defence > 0 ? other.gameObject.GetComponent<ProjectileScript>().Damage - owner._defence : 1;
            gameObject.GetComponent<HPScript>().ChangeHP(-Mathf.RoundToInt(dam), other.ClosestPoint(transform.position), Color.green);
            gameObject.GetComponent<CharacterCtrl>().HittingMe((int)dam);
        }
    }
}
