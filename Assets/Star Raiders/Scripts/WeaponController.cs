using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour {
	// hmmm weapon classes might be able to be generic
	public List<Transform> PhotonMountPoints;
	public AudioClip PhotonShotSoundEffect;
	public PhotonBlast PhotonShotPrefab;
	public float PhotonFireRate;

	public float NextPhotonShotTime { get; private set; }
	private int _mountPointIndex;
	private Collider _parentCollider;
	private Rigidbody _parentRigidbody;
	private AudioSource _audio;

	// TODO: deprecated?
	public WeaponController() {}

	public void SetAudioSource(AudioSource source)
	{
		_audio = source;
	}

	public void SetCollider(Collider col)
	{
		_parentCollider = col;
		//Debug.Log ("got collider " + col.ToString ());
	}

	public void SetRigidbody(Rigidbody rb)
	{
		_parentRigidbody = rb;
	}
	
	public void FirePhoton()
	{
		//Debug.Log ("fire!");
		if (!CanFire ()) return;

		NextPhotonShotTime = Time.time + PhotonFireRate;
		var mount = GetNextPhoton ();
		var shot = (PhotonBlast)Instantiate (PhotonShotPrefab, mount.position, mount.rotation);
		if (_parentRigidbody != null) 
			shot.rigidbody.velocity = _parentRigidbody.velocity;

		if (_parentCollider != null) 
			Physics.IgnoreCollision (_parentCollider, shot.collider);

		_mountPointIndex++;

		if (_audio != null && PhotonShotSoundEffect != null) {
			_audio.PlayOneShot(PhotonShotSoundEffect);
		}
	}
	
	public bool CanFire()
	{
		return (Time.time > NextPhotonShotTime);
	}
	
	private Transform GetNextPhoton()
	{
		if (_mountPointIndex == PhotonMountPoints.Count)
			_mountPointIndex = 0;
		
		return PhotonMountPoints[_mountPointIndex];
	}
}
