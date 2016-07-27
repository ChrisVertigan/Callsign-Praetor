// Christopher Vertigan - N3078812
// 20/02/2016

using UnityEngine;
using System.Collections;

public class AIPlayer : MonoBehaviour {
	
	public int playerID_;
	public int unitsStart_ = 4;
	public GameObject[] units_;
	
	private GameObject controller_;
	
	void Start () {
		
		controller_ = GameObject.Find("GameController");
		units_ = new GameObject[unitsStart_];
		
		//Create the starting labourers.
		for (int i = 0; i < unitsStart_; i++) 
		{
			units_[i] = Instantiate(Resources.Load ("Labourer"), transform.position, transform.rotation) as GameObject;
			units_[i].transform.parent = gameObject.transform;
			units_[i].transform.name = "NPC" + playerID_.ToString() + i.ToString();
			units_[i].GetComponent<Labourer>().name = units_[i].transform.name;
		}
	}
	
	void Update () {
		
	}
}
