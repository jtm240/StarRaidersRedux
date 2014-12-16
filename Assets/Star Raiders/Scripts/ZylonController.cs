using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZylonController : MonoBehaviour {

	public float damping = 6.0f;
	public List<ParticleSystem> Engines;
	public List<Transform> WeaponMountPoints;
	public Transform ExplosionPrefab;

	private ShipBehavior CurrentBehavior;
	private WeaponController _weaponController;

	void Awake()
	{
		//_myTransform = transform;
	}

	void Start () {
		var obj = GameObject.FindGameObjectWithTag("Player");
		if (obj != null)
		{
			//_target = obj.transform;
		}

		_weaponController = this.GetComponent<WeaponController>();
		_weaponController.SetCollider (this.collider);
		//CurrentBehavior = new PatrolBehavior(this, _weaponController);
		CurrentBehavior = new IdleBehavior(this);//, _weaponController);
	}
	
	void Update () {
		if (CurrentBehavior != null)
			CurrentBehavior.Update();
	}

	void FixedUpdate()
	{
		if (CurrentBehavior != null)
			CurrentBehavior.FixedUpdate ();
	}
	
	void OnMouseDown()
	{
		Debug.Log ("Hi!");
	}

	void OnTriggerEnter (Collider col)
	{
		//Debug.Log("ouch!");
		if (ExplosionPrefab != null)
		{
			Instantiate (ExplosionPrefab, transform.position, transform.rotation);
		}
		Destroy (this.gameObject);
	}

	void SetEngineEffects(bool startEffects)
	{
		Debug.Log ("setting engines: " + startEffects);
		foreach (var engine in Engines)
		{
			//engine.animation.enabled = startEffects;
			engine.enableEmission = startEffects;
		}
	}
}
