using UnityEngine;
using System.Collections;

public class IdleBehavior : ShipBehavior {

	public IdleBehavior(MonoBehaviour ship) : base(ship)
	{
		Debug.Log("idle behavior started");
	}

	public override void Update () {
		//base.Update ();
	}

	public override void FixedUpdate()
	{
		// if player comes by, switch to attack?
	}

	public override void BehaviorExpired ()
	{
		// not sure about this
		// call a delegate on the parent to set the next behavior
	}
}
