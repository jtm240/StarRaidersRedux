using UnityEngine;
using System.Collections;
using System;

public class GuiController : MonoBehaviour {

	//public GameObject player;
	private PlayerShipController player;

	public GUIText MessageText;
	public GUIText VelocityIndicator;
	public AudioClip AcknowledgeSound;

	private float MessageDisplayTime;

	// Use this for initialization
	void Start () {
		var obj = GameObject.FindGameObjectWithTag("Player");
		if (obj == null)
		{
			Debug.Log ("Could not find player!");
			return;
		}

		player = obj.GetComponent<PlayerShipController>();
		if (player == null)
		{
			Debug.Log ("Could not find player script!");
			return;
		}

		//player.Changed += new FooEventHandler(PlayerChanged);
		player.ShieldsChanged += new ShieldsChangedEventHandler(ShieldsChanged);
	}

	void ShieldsChanged(bool online)
	{
		var status = online ? "ON" : "OFF";
		MessageText.text = "SHIELDS " + status;
		MessageDisplayTime = 4f;
		// play sound
		if (AcknowledgeSound != null) {
			//audio.PlayOneShot(AcknowledgeSound);
			AudioSource.PlayClipAtPoint (AcknowledgeSound, player.transform.position);
		}
	}

	// Update is called once per frame
	void Update () {
		MessageDisplayTime -= Time.deltaTime;
		if (MessageDisplayTime <= 0)
			MessageText.text = string.Empty;

		var spd = player.rigidbody.velocity.magnitude;
		if (spd < 1f)
			spd = 0;
		VelocityIndicator.text = (spd * 3.6).ToString ("0.00") + "kph";
	}
}
