// Fades out the Text of the HPParticle and Destroys it 

using UnityEngine;
using System.Collections;

public class HPParticleScript : MonoBehaviour {

	public float Alpha =1f;
	public float FadeSpeed = 1f;

	public GameObject HPLabel;

    // Set a Variable
    private void OnEnable()
    {
		Alpha = 1;
		Color CurrentColor = HPLabel.GetComponent<TextMesh>().color;
		HPLabel.GetComponent<TextMesh>().color = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, Alpha);
	}
    void Start () 
	{
		HPLabel = gameObject.transform.Find("HPLabel").gameObject;
	}

	void FixedUpdate () 
	{
		Alpha = Mathf.Lerp(Alpha,0f,FadeSpeed * Time.deltaTime);

		Color CurrentColor = HPLabel.GetComponent<TextMesh>().color;
		HPLabel.GetComponent<TextMesh>().color = new Color(CurrentColor.r,CurrentColor.g,CurrentColor.b,Alpha);

		if (Alpha < 0.005f)
		{
			ObjectPoolingManager._instance.ReturnObj(DefineEnumHelper.PoolingObj.HPParticle,gameObject);
		}
	}
}
