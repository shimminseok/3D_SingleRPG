using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	public ObjectBase _owner => transform.GetComponentInParent<ObjectBase>();
	public float Damage => Mathf.RoundToInt(_owner.FinalDamage(_owner._mountItemDam + _owner._skillDam + _owner._buffDam));
}
