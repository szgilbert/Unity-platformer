using UnityEngine;
using System.Collections;

//created by Sam Gilbert
//last modified 12/19/2013
//script used to display player health

public class GUIInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//gui will always be at correct position even at different resolutions
		guiText.transform.position = new Vector3(100f / (float)Screen.width, 30f / (float)Screen.height, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		//shows player health on screen
	 guiText.text = "Health :" + Player.health;
	}
}
