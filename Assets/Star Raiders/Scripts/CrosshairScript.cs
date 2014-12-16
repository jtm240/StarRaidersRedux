using UnityEngine;
using System.Collections;

public class CrosshairScript : MonoBehaviour {

	public Texture2D CrosshairTexture;
	public Texture2D ComputerTexture;	
	private Rect _crosshairPosition;
	private Rect _computerPosition;

	// Use this for initialization
	void Update () {
		_crosshairPosition = new Rect((Screen.width - CrosshairTexture.width) / 2, (Screen.height - CrosshairTexture.height) / 2, CrosshairTexture.width, CrosshairTexture.height);
		_computerPosition = new Rect(
			(Screen.width - ComputerTexture.width) / 1.2f
			,(Screen.height - ComputerTexture.height) / 1.2f
			,ComputerTexture.width
			,ComputerTexture.height);
	}

	void OnGUI()
	{
		GUI.DrawTexture (_crosshairPosition, CrosshairTexture);
		GUI.DrawTexture (_computerPosition, ComputerTexture);
	}
}

