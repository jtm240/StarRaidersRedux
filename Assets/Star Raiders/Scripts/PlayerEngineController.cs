using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEngineController : MonoBehaviour {

	public List<Transform> Engines;

	private bool _isActive;
	private Transform _transform;
	private Rigidbody _rigidbody;
	private Light _light;
	private ParticleSystem _particleSystem;
	private AudioSource _audio;
	private float _volume = 1.0f;
	private float _force;
	
	void Start () {
		_isActive = false;
		// Cache the transform and parent rigidbody to improve performance
		_transform = transform;
		
		// Ensure that parent object (e.g. spaceship) has a rigidbody component so it can apply force.
		if (_transform.parent.rigidbody != null) {			
			_rigidbody = _transform.parent.rigidbody;
		} else {
			Debug.LogError("Thruster has no parent with rigidbody that it can apply the force to.");
		}
		
		// Cache the light source to improve performance (also ensure that the light source in the prefab is intact.)
		_light = _transform.GetComponent<Light>().light;
		if (_light == null) {
			Debug.LogError("Thruster prefab has lost its child light. Recreate the thruster using the original prefab.");
		}
		// Cache the particle system to improve performance (also ensure that the particle system in the rpefab is intact.)
		_particleSystem = particleSystem;
		if (_particleSystem == null) {
			Debug.LogError("Thruster has no particle system. Recreate the thruster using the original prefab.");
		}
		
		// Start the audio loop playing but mute it. This is to avoid play/stop clicks and clitches that Unity may produce.
		audio.loop = true;
		audio.volume = _volume;
		audio.mute = true;
		audio.Play();		
	}

	public void SetEngineSpeed(int speed)
	{
		float forwardSpeed;
		switch (speed)
		{
		case 0:
			forwardSpeed = 0f;
			break;
		case 1:
			forwardSpeed = 2.5f;
			break;
		case 2:
			forwardSpeed = 5f;
			break;
		case 3:
			forwardSpeed = 10f;
			break;
		case 4:
			forwardSpeed = 30f;
			break;
		case 5:
			forwardSpeed = 60f;
			break;
		case 6:
			forwardSpeed = 120f;
			break;
		case 7:
			forwardSpeed = 250f;
			break;
		case 8:
			forwardSpeed = 370f;
			break;
		case 9:
			forwardSpeed = 430f;
			break;
		default: 
			forwardSpeed = 0;
			break;
		}
		
		_force = forwardSpeed * 1000;
		_volume = 0.11f * speed;
		_isActive = forwardSpeed > 0 ? true : false;
	}

	void Update () {
		// If the light source of the thruster is intact...
		if (_light != null) {
			// Set the intensity based on the number of particles
			_light.intensity = _particleSystem.particleCount / 20;
		}
		
		// If the thruster is active...
		if (_isActive) {
			// ...and if audio is muted...
			if (audio.mute) {
				// Unmute the audio
				audio.mute=false;
			}
			// If the audio volume is lower than the sound effect volume...
			// TODO: this is a crude hack of the original code, but it works for now
			if (audio.volume < _volume) {
				// ...fade in the sound (to avoid clicks if just played straight away)
				audio.volume += 5f * Time.deltaTime;
				//Debug.Log ("little louder");
			} else if (audio.volume > _volume)
			{
				//Debug.Log ("sssh! quiet please");
				//audio.volume -= 5f * Time.deltaTime;
				audio.volume = Mathf.Clamp (audio.volume - 5f * Time.deltaTime, _volume, audio.volume);
			}
			
			// If the particle system is intact...
			if (_particleSystem != null) {	
				// Enable emission of thruster particles
				_particleSystem.enableEmission = true;
			}		
		} else {
			// The thruster is not active
			if (audio.volume > 0.01f) {
				// ...fade out volume
				audio.volume -= 5f * Time.deltaTime;	
			} else {
				// ...and mute it when it has faded out
				audio.mute = true;
			}
			
			// If the particle system is intact...
			if (_particleSystem != null) {				
				// Stop emission of thruster particles
				_particleSystem.enableEmission = false;				
			}
			
		}
	}

	void FixedUpdate() {
		if (_isActive) {
			_rigidbody.AddRelativeForce (Vector3.forward * _force);
		}		
	}
}
