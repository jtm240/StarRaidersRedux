using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PatrolBehavior : ShipBehavior {

	private Transform _transform; // TODO: naming
	private Rigidbody _rigidbody;
	private GameObject _player;
	private List<Vector3> Waypoints;
	private int WaypointIndex;
	private Vector3 Waypoint;
	private WeaponController _weaponController;

	private string _lastMessage;

	private float damping = 6f; // TODO: get from parent ship
	
	// TODO: maybe revisit an IShip model with access to rigidbody, weaponcontroller, enginecontroller etc
	public PatrolBehavior(MonoBehaviour ship, WeaponController weaponController) : base(ship)
	{
		_transform = Ship.transform;
		_rigidbody = Ship.rigidbody;
		_player = GameObject.FindGameObjectWithTag("Player");
		_weaponController = weaponController;

		Waypoints = new List<Vector3>();
		var pos = _transform.position;
		for (int i=0; i < 4; i++)
		{
			var wp = new Vector3(pos.x + Random.Range (-500, 500), pos.y + Random.Range(-500, 500), pos.z + Random.Range (-500, 500));
			Waypoints.Add (wp);
		}
		Waypoint = Waypoints[0];
	}

	private Vector3 SetNextWaypoint()
	{
		WaypointIndex++;

		if (WaypointIndex == Waypoints.Count)
		{
			WaypointIndex = 0;
		}

		return Waypoints[WaypointIndex];
	}
	
	public override void Update () {
	
	}

	public override void FixedUpdate()
	{
		// TODO: variable ranges
		// is the player nearby?
		var playerSqrDist = (_transform.position - _player.transform.position).sqrMagnitude;
		if (_player != null && playerSqrDist <= 750000f)
		{
			// intercept the player
			var rotation = Quaternion.LookRotation (_player.transform.position - _transform.position);
			_transform.rotation = Quaternion.Slerp (_transform.rotation, rotation, Time.deltaTime * damping);
			if (_player != null && playerSqrDist > 300000f)
			{
				_rigidbody.AddRelativeForce (Vector3.forward * 100000f);
			}

			if (playerSqrDist <= 250000f)
			{
				// TODO: after firing, choose a new location
				PrintMsg("firing");
				_weaponController.FirePhoton ();
			}
		}
		else 
		{
			//PrintMsg("proceeding to waypoint");
			// check distance to waypoint
			if ((Waypoint - _transform.position).sqrMagnitude <= 750)
			{
				Waypoint = SetNextWaypoint ();
				//Debug.Log ("set new waypoint: " + Waypoint.ToString ());
			}

			var rotation = Quaternion.LookRotation (Waypoint - _transform.position);
			_transform.rotation = Quaternion.Slerp (_transform.rotation, rotation, Time.deltaTime * damping);
			_rigidbody.AddRelativeForce (Vector3.forward * 100000f);
		}
	}

	public void PrintMsg(string msg)
	{
		if (msg == _lastMessage) return;
		Debug.Log(msg);
		_lastMessage = msg;
	}




	public override void BehaviorExpired ()
	{
	}
}
