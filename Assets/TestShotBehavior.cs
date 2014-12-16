using UnityEngine;
using System.Collections;

public class TestShotBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//_velocity = velocity * transform.forward;
	}

	void FixedUpdate()
	{
		//Physics.IgnoreCollision (
		//rigidbody.AddRelativeForce (Vector3.forward * 50000 * Time.deltaTime);


		//_newPos += transform.forward * _velocity.magnitude * Time.deltaTime;
		//rigidbody.AddRelativeForce (Vector3.forward * 10000 * Time.deltaTime);
	}
}
