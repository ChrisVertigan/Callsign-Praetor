using UnityEngine;
using System.Collections;



public class Tile : MonoBehaviour {

	GameController script;
	public int resource, passable, team, state;
	SpriteRenderer renderer;
	public float G, H, F;
	public Vector2 position;
	public Vector2 parent_position;
	public Tile parent;


	public int Resource
	{
		get { return resource; }
		set 
		{
			resource = value;
			switch (resource) 
			{
			case 0:
				//No resource
				renderer.sprite = script.mapMaker_.sprites[16];
				break;
			case 1:
				//Coal
				renderer.sprite = script.mapMaker_.sprites[4];
				break;
			case 2:
				//Ore
				renderer.sprite = script.mapMaker_.sprites[15];
				break;
			case 3:
				//Timber
				renderer.sprite = script.mapMaker_.sprites[34];
				break;
			case 4:
				//Stone
				renderer.sprite = script.mapMaker_.sprites[25];
				break;
			}
			
			if (passable == 0)
			{
				renderer.sprite = script.mapMaker_.sprites[10];
			}
		}
	}

	enum States : int
	{
		Passable = 16,
		Coal = 4,
		Ore = 15,
		Timber = 34,
		Stone = 25,
		BarracksBlue = 0,
		BarracksRed = 1,
		BlacksmithBlue = 2,
		BlacksmithRed = 3,
		HouseBlue = 6,
		HouseRed = 7,
		HutBlue = 8,
		HutRed = 9,
		MarketBlue = 11,
		MarketRed = 12,
		MineBlue = 13,
		MineRed = 14,
		QuarryBlue = 17,
		QuarryRed = 18,
		SawmillBlue = 19,
		SawmillRed = 20,
		SchoolBlue = 21,
		SchoolRed = 22,
		SmelterBlue = 23,
		SmelterRed = 24,
		Storage1Blue = 26,
		Storage1Red = 30,
		Storage2Blue = 27,
		Storage2Red = 31,
		Storage3Blue = 28,
		Storage3Red = 32,
		StorageFullBlue = 29,
		StorageFullRed = 33
	}

	// Use this for initialization
	void Start () {

		script = GameObject.Find ("GameController").GetComponent<GameController> ();
		renderer = this.GetComponent<SpriteRenderer> ();



	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeState(int stateNumber, int teamNo)
	{
		team = teamNo;
		state = stateNumber;
		script = GameObject.Find ("GameController").GetComponent<GameController> ();
		renderer = this.GetComponent<SpriteRenderer> ();

		script.mapMaker_.sprites = Resources.LoadAll<Sprite>("Sprites");

		if (script.mapMaker_.sprites [0].name != null) {

		}
		switch (stateNumber) 
		{
		case 0:
			renderer.sprite = script.mapMaker_.sprites[(int)States.Passable];
			break;
		case 1:
			renderer.sprite = script.mapMaker_.sprites[(int)States.Coal];
			break;
		case 2:
			renderer.sprite = script.mapMaker_.sprites[(int)States.Ore];
			break;
		case 3:
			renderer.sprite = script.mapMaker_.sprites[(int)States.Timber];
			break;
		case 4:
			renderer.sprite = script.mapMaker_.sprites[(int)States.Stone];
			break;
		case 5:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.BarracksBlue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.BarracksRed];
			break;
		case 6:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.BlacksmithBlue];
			else
				renderer.sprite = script.mapMaker_.sprites[(int)States.BlacksmithRed];
			break;
		case 7:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.HouseBlue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.HouseRed];
			break;
		case 8:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.HutBlue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.HutRed];
			break;
		case 9:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.MarketBlue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.MarketRed];
			break;
		case 10:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.MineBlue];
			else  if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.MineRed];
			break;
		case 11:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.QuarryBlue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.QuarryRed];
			break;
		case 12:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.SawmillBlue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.SawmillRed];
			break;
		case 13:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.SchoolBlue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.SchoolRed];
			break;
		case 14:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.SmelterBlue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.SmelterRed];
			break;
		case 15:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.Storage1Blue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.Storage1Red];
			break;
		case 16:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.Storage2Blue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.Storage2Red];
			break;
		case 17:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.Storage3Blue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.Storage3Red];
			break;
		case 18:
			if (team == 1)
				renderer.sprite = script.mapMaker_.sprites[(int)States.StorageFullBlue];
			else if (team == 2)
				renderer.sprite = script.mapMaker_.sprites[(int)States.StorageFullRed];
			break;
		}

		if (passable == 0)
		{
			renderer.sprite = script.mapMaker_.sprites[10];
		}
	}
}
