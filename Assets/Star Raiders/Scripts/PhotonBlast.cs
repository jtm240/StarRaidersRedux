using UnityEngine;
using System.Collections;
using System;

public class PhotonBlast : MonoBehaviour {

	public float _lifetime = 3f;

	void Update()
	{
		_lifetime -= Time.deltaTime;
		if (_lifetime <= 0)
			Destroy (gameObject);
	}

	void FixedUpdate () {
		rigidbody.AddForce (transform.forward * 500000f * Time.deltaTime);
	}

	void OnTriggerEnter (Collider col)
	{
		//Debug.Log (this.name); // Photon shot(Clone)
		//Debug.Log ("col: [" + col.gameObject.name + "]"); // also ps(clone
		//Debug.Log ("named: [" + this.name + "]");
		if (col.gameObject.name == this.name)
		{
			Destroy (col.gameObject);
			Destroy(this.gameObject);
			// TODO: play sound
		}
	}
}
