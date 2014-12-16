using UnityEngine;
using System.Collections;

public class TestLocation : MonoBehaviour {

	private Transform _target;
	// Use this for initialization
	void Start () {
		var obj = GameObject.FindGameObjectWithTag("Enemy");
		if (obj) {
			_target = obj.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Space))
			PrintDirection ();

		if (Input.GetKeyUp (KeyCode.Backspace))
			print (transform.eulerAngles);

		if (Input.GetKeyUp (KeyCode.LeftControl))
			PrintAB();
	}

	void PrintDirection()
	{
		var foo = _target.InverseTransformPoint (transform.position);
		print (foo);

		// x is lateral displacement
		// z is forward/back
//		if (foo.z > 0)
//		{
//			print ("Target is in front");
//		}
//		else
//		{
//			print ("Target is in back");
//		}
	}

	void PrintAB()
	{
		var AB = _target.position - transform.position; // hmm this might not work, hopefull r/u/f are relative
		var relPos = Vector3.zero;
		relPos.x = Vector3.Dot (AB, transform.right.normalized);
		relPos.y = Vector3.Dot (AB, transform.up.normalized);
		relPos.z = Vector3.Dot (AB, transform.forward.normalized);
		
		//Debug.Log (relPos);
		Debug.DrawLine(transform.position, AB, Color.red, 3f);

		var xDesc = string.Empty;
		var yDesc = string.Empty;
		var zDesc = string.Empty;

		// yes, it shows on radar ships BEHIND you
		// http://youtu.be/FwoZQQO9mqg?t=6m30s
		// errr maybe not, computer damaged :-/
		// here! http://youtu.be/j5yXCq-7do4?t=2m10s

		if (Mathf.Abs (relPos.z) < 1f)
		{
			zDesc = "centered";
		} else if (relPos.z > 0)
		{
			zDesc = "port side";
		}
		else {
			zDesc = "starboard side";
		}

		if (Mathf.Abs (relPos.y) < 1f)
		{
			yDesc = string.Empty;
			//yDesc = "dead ahead";
		} else if (relPos.z > 0)
		{
			yDesc = " high";
		}
		else {
			yDesc = " low";
		}
		print (zDesc + yDesc + xDesc);
	}
}
