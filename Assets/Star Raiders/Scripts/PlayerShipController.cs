using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public delegate void FooEventHandler(GameObject e);
public delegate void ShieldsChangedEventHandler(bool online);

public class PlayerShipController : MonoBehaviour {

	//public event FooEventHandler Changed; // using this for now
	public event ShieldsChangedEventHandler ShieldsChanged;

	public PlayerShipEngine[] thrusters;
	public float RollRate = 100.0f;
	public float YawRate = 100.0f; //30.0f;
	public float PitchRate = 100.0f;

	private Rigidbody _rigidbody;
	private Transform _transform;
	private WeaponController _weapons;

	// Platform variables
	private int _engineSpeed;
	private double _energy;
	private bool _shieldsOn;
	private bool _shieldsActive; // for when shields are damaged; update by frame
	private bool _computerOn; 	// TODO: which computer
	private Transform _target;
	private Quaternion _newLook;
	private bool _isRotating;
	private Camera MainCamera;
	//private bool _leftShot;
	//private float _nextPhotonShotTime;


	// TODO: clean up unused variables, nomenclature

	void Awake() 
	{
		_transform = transform;
		_rigidbody = rigidbody;
		if (_rigidbody == null) {
			Debug.LogError("Spaceship has no rigidbody - the thruster scripts will fail. Add rigidbody component to the spaceship.");
		}

		var obj = this.GetComponent<WeaponController>();
		if (obj != null)
		{
			_weapons = this.GetComponent<WeaponController>();
			_weapons.SetCollider (this.collider);
			_weapons.SetRigidbody (_rigidbody);
			_weapons.SetAudioSource(this.audio);
		}
	}

	void Start () {		
		// Ensure that the thrusters in the array have been linked properly
		foreach (PlayerShipEngine _thruster in thrusters) {
			if (_thruster == null) {
				Debug.LogError("Thruster array not properly configured. Attach thrusters to the game object and link them to the Thrusters array.");
			}			
		}

		_energy = 9999;
		_engineSpeed = 0;
		_shieldsOn = false;
		_shieldsActive = false;
		_computerOn = false;
		InvokeRepeating("ExpendEnergy", 0f, 1f);
		//InvokeRepeating("PrintDirection", 0f, 4f);

		var obj = GameObject.FindGameObjectWithTag("Enemy");
		if (obj) {
			_target = obj.transform;
		}
		_isRotating = false;

		//_testShot = GameObject.FindGameObjectWithTag("TestShot").transform;
		//_testShot = Instantiate (new PhotonBlast());

		MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

	}

	void GetPlayerCommands()
	{
		if (Input.GetKeyDown (KeyCode.Alpha0)) 
		{
			SetEngineSpeed (0);
		} 
		else if (Input.GetKeyDown (KeyCode.Alpha1)) {
			SetEngineSpeed (1);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SetEngineSpeed (2);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			SetEngineSpeed (3);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			SetEngineSpeed (4);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha5)) {
			SetEngineSpeed (5);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha6)) {
			SetEngineSpeed (6);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha7)) {
			SetEngineSpeed (7);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha8)) {
			SetEngineSpeed (8);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha9)) {
			SetEngineSpeed (9);
		}
			
		if (Input.GetKeyDown (KeyCode.Z))
		{
			_shieldsOn = !_shieldsOn;
			if (ShieldsChanged != null)
				ShieldsChanged(_shieldsOn);
		}

//		if (Input.GetButtonDown("Fire2")  && Time.time > _nextPhotonShotTime) {
//			FirePhoton();
//		}

		if (Input.GetButtonDown("Fire2")) {
			_weapons.FirePhoton ();
		}
	}

	void PrintDirection()
	{
		var foo = _target.InverseTransformPoint (_transform.eulerAngles);
		print (foo);
		if (foo.z > 0)
		{
			print ("Target is in front");
		}
		else
		{
			print ("Target is in back");
		}
	}

	void PrintDirectionasDDA()
	{
		//var foo = Vector3.Cross (_target.position, _transform.eulerAngles);
		//Debug.Log (foo);
		var AB = _target.position - _transform.eulerAngles; // hmm this might not work, hopefull r/u/f are relative
		var relPos = Vector3.zero;
		relPos.x = Vector3.Dot (AB, _transform.right.normalized);
		relPos.y = Vector3.Dot (AB, _transform.up.normalized);
		relPos.z = Vector3.Dot (AB, _transform.forward.normalized);

		Debug.Log (relPos);
	}

	void PrintDirectiofdsfdfdsn()
	{
		var heading = _target.position - _transform.eulerAngles;
		var direction = heading / heading.magnitude; // or call normalize
		
		Debug.Log (direction.y);
		var horizDesc = string.Empty;
		if (Mathf.Abs(direction.y) <= 0.05f)
		{
			horizDesc = "dead ahead";
		}
		else
		{
			horizDesc = (direction.y > 0) ? "port side" : "starboard side";
		}

		var yDesc = string.Empty;
		if (direction.y != 0)
		{
			yDesc = (direction.y > 0) ? "high" : "low";
		}

		Debug.Log(direction + " " + horizDesc + ", " + yDesc);
	}

	void Update () {
		GetPlayerCommands ();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_target)
			{
				// hmm this needs to rotate until it gets there
				// store the intended rotation and continue lerping until we arrive there. 
				// cancel if any inputs are made
				//Debug.Log ("rotating");
				Quaternion rotation = Quaternion.LookRotation(_target.position - _transform.position);
				_newLook = rotation;
				_isRotating = true;
			}
		}

		if (_isRotating)
		{
			_transform.rotation = Quaternion.Slerp(_transform.rotation, _newLook, Time.deltaTime * 6f);
		}

		if (_isRotating && _newLook == _transform.rotation)
		{
			//Debug.Log ("Done rotating!");
			_isRotating = false;
		}
	}
	
	void FixedUpdate () {
		var horiz = Input.GetAxis("Horizontal");
		var vert = Input.GetAxis("Vertical");
		if (_isRotating && (vert != 0f || horiz != 0f))
		{
			//Debug.Log ("show's over");
			_isRotating = false;
		}
			
		_rigidbody.AddRelativeTorque(new Vector3(0,horiz*YawRate*_rigidbody.mass,0));
		_rigidbody.AddRelativeTorque(new Vector3(vert*PitchRate*_rigidbody.mass,0,0));	
		if (Input.GetKey (KeyCode.Q)){
			_rigidbody.AddRelativeTorque(new Vector3(0,0,10f*RollRate*_rigidbody.mass));
		} else if (Input.GetKey (KeyCode.E))
		{
			_rigidbody.AddRelativeTorque(new Vector3(0,0,-10f*RollRate*_rigidbody.mass));
		}
	}

	void SetEngineSpeed(int speed)
	{
		foreach (PlayerShipEngine _thruster in thrusters) {
			_thruster.SetEngineSpeed (speed);
		}
	}

	void ExpendEnergy()
	{
		// calculate energy cost
		// target = 60fps, delta time .02?
		// so x amount of fixed updates per frame, increment and every 25% compute some cost...
		
		// hit by enemy = 100 units
		// fire photon = 10
		// shields 2/s
		// computer .5/s
		// life support (always on) .25/s
		
		// speed0 0
		// speed1 1
		// speed2 1.5
		// speed3 2
		// speed4 2.5
		// speed5 3
		// speed6 3.5
		// speed7 7
		// speed8 11.25
		// speed9 15
		//quarterSecond += 1;
		//if (quarterSecond == 4) quarterSecond = 0;
		
		// TODO: if alive...
		double energyCost = .25;
		
		if (_shieldsOn)
			energyCost += 2;
		
		if (_computerOn)
			energyCost += .5;
		
		switch (_engineSpeed)
		{
		case 1:
			energyCost += 1;
			break;
		case 2:
			energyCost += 1.5;
			break;
		case 3:
			energyCost += 2;
			break;
		case 4:
			energyCost += 2.5;
			break;
		case 5:
			energyCost += 3;
			break;
		case 6:
			energyCost += 3.5;
			break;
		case 7:
			energyCost += 7;
			break;
		case 8:
			energyCost += 11.25;
			break;
		case 9:
			energyCost += 15;
			break;
		}
		_energy -= energyCost;
		// TODO: hudController.UpdateEnergy (energy);
	}

	void OnCollisionEnter(Collision other)
	{
		//Debug.Log ("bang!");
		TakeDamage ();
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log ("ginned");
		TakeDamage ();
	}

	void TakeDamage()
	{
		// TODO: location specific damage?

	}
}