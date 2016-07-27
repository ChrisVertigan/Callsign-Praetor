using UnityEngine;
using System.Collections;

public class PlayGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Clicked(){

		//Potentially send through parameters to the GameController.

		Application.LoadLevel("Map1");
	}
}
