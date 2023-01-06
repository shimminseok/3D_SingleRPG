// this script controls the HP and Instantiates an HP Particle

using UnityEngine;
using System.Collections;

public class HPScript : MonoBehaviour {

	public GameObject _hitEffect;
	public Vector3 _offset;
	public Vector3 DefaultForce = new Vector3(0f,1f,0f);
	public float DefaultForceScatter = 0.5f;

	public void ChangeHP(float Delta, Vector3 Position, Color color)
	{
		GameObject NewHPP = ObjectPoolingManager._instance.GetObject(DefineEnumHelper.PoolingObj.HPParticle);
		NewHPP.transform.position = Position + _offset;
		NewHPP.transform.rotation = transform.rotation;
		NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;
		
		TextMesh TM  = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();

		if (Delta > 0f)
		{
			TM.text = "+" + Delta.ToString();
			TM.color = color;
		}
		else
		{
			color = Color.red;
			TM.text = Delta.ToString();
			TM.color = color;
		}
		NewHPP.GetComponent<Rigidbody>().AddForce( new Vector3(DefaultForce.x + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.y + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.z + Random.Range(-DefaultForceScatter,DefaultForceScatter)));
	}
	public void GetHitEffect(Vector3 position)
    {
        GameObject GetHitEffect = Instantiate(_hitEffect, position, transform.rotation) as GameObject;

	}
}
