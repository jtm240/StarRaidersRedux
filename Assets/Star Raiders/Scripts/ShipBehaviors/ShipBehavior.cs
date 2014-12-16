using UnityEngine;
using System.Collections;

//public abstract class ShipBehavior : ScriptableObject {
public abstract class ShipBehavior {

	// TODO: private set?
	public MonoBehaviour Ship { get; set; }
	public string AlliesTag { get; set; }
	public string EnemiesTag { get; set; }
	public float RunTime { get; set; }
	public ShipBehavior NextBehavior { get; set; }

	public ShipBehavior() { RunTime = 30f; }

	public ShipBehavior(MonoBehaviour ship)
	{
		//Debug.Log ("in superclass constructor");
		RunTime = 30f;
		Ship = ship;
	}

//	public ShipBehavior(MonoBehaviour ship, string alliesTag, string enemiesTag, float runTime, ShipBehavior nextBehavior)
//	{
//		Ship = ship;
//		AlliesTag = alliesTag;
//		EnemiesTag = enemiesTag;
//		RunTime = runTime;
//		NextBehavior = nextBehavior;
//	}

//	public void Update()
//	{
//		RunTime -= Time.deltaTime;
//	}

	public abstract void BehaviorExpired();

	public virtual void Update() {}
	public virtual void FixedUpdate() {}
	
}
