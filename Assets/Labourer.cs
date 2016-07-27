using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Command
{
	public int command;
	public string commanda;
	public int teamnumber;
	public string unitname;
	public string target;
	
	public Command(int task, string Target, int team, string name)
	{
		command = task;
		team = teamnumber;
		unitname = name;
		target = Target;
	}

	public Command(string task, string Target, int team, string name)
	{
		commanda = task;
		team = teamnumber;
		unitname = name;
		target = Target;
	}
}

public class Labourer : Unit {

	public int team;
	public int skill = 0;
	public bool free = false;
	public string name;
	public bool isPerson = true;
	public bool isLabourer = false;
	public bool isCarpenter = false;
	public bool isBlacksmith = false;
	public bool isTeacher = false;
	public bool isMiner = false;
	public bool isTrader = false;
	public bool isLumberjack = false;
	public bool isRifleman = false;
	public bool hasSkill = false;
	public bool nearbyForest = false;
	public bool nearbyStone = false;
	public bool nearbyOre = false;
	public bool nearbyCoal = false;
	public bool isDead = false;
	public Vector2 position;
	float capturedtime;
	public Sprite lab0, lab1;

	AIController airesources;

	Queue<Command> Plan;
	public PathFinding pathing;

	public enum Tasks : int
	{
		CutTree,
		Smelt,
		Mine,
		Quarry,
		Buy,
		Sell,
		Breed,
		SawWood,
		Educate,
		Train,
		SuperEducate,
		Store
	}

	void Start () {

		Plan = new Queue<Command>();
		team = transform.parent.GetComponent<AIPlayer> ().playerID_;

		airesources = transform.parent.GetComponent<AIController> ();
		capturedtime = airesources.alltimer;
		pathing = GetComponent<PathFinding>();
		if (team == 0) {
			
			GetComponent<SpriteRenderer> ().sprite = lab0;
			position = new Vector2(9, 16);
		} else if (team == 1) {
			
			GetComponent<SpriteRenderer>().sprite = lab1;
			position = new Vector2(10, 4);
		}

		InvokeRepeating ("Execute", 2.0f, 2.0f);
	}

	void Update () {
	
		if (isDead == true)
			Destroy (this.gameObject);

	}

	public override void newTask(int task, string target)
	{
		Plan.Enqueue(new Command (task, target, team, name));
	}

	public override void newTask(string task, string target)
	{
		Plan.Enqueue(new Command (task, target, team, name));
	}

	public void Execute()
	{
		if (free == true && Plan.Count != null) 
		{
			Command current = Plan.Dequeue();
			if(current.command == 0)
			{
				MoveTo(current.target);
			}
			/*else if (current.command == "QUARRY")
			{
				//Mine();
			}
			else if (current.command == "MAKE")
			{
				
			}
			else if (current.command == "SMELT")
			{
				
			}
			else if (current.command == "FAMILY")
			{
				
			}
			else if (current.command == "STORE")
			{
				
			}
			else if (current.command == "BUILD")
			{
				
			}
			else if (current.command == "BUY")
			{
				
			}
			else if (current.command == "SELL")
			{
				
			}
			else if (current.command == "EDUCATE")
			{
				
			}
			else if (current.command == "CUTTREE")
			{
				
			}*/
		}
	}

	public void BuildBuilding(string buildingName)
	{

		//Different timer for every building.


		switch (buildingName) 
		{
		case "Blacksmith":
			if (airesources.resourceLevels["Iron"] > 0 && airesources.resourceLevels["Stone"] > 0 && airesources.resourceLevels["Timber"] > 0)
			{
				free = false;
				//Find Closest Empty Tile to Labourer
				//Swap tile to correct state and team number.
				
				float capturedtimer = airesources.alltimer;

				airesources.resourceLevels["Iron"] -= 1;
				airesources.resourceLevels["Stone"] -= 1;
				airesources.resourceLevels["Timber"] -= 1;
				free = true;
			}
			break;
		case "Sawmill":
			if (airesources.resourceLevels["Iron"] > 0 && airesources.resourceLevels["Stone"] > 0 && airesources.resourceLevels["Timber"] > 0)
			{
				free = false;
				
				float capturedtimer = airesources.alltimer;

				airesources.resourceLevels["Iron"] -= 1;
				airesources.resourceLevels["Stone"] -= 1;
				airesources.resourceLevels["Timber"] -= 1;
				free = true;
			}
			break;
		case "School":
			if (airesources.resourceLevels["Iron"] > 0 && airesources.resourceLevels["Stone"] > 0 && airesources.resourceLevels["Wood"] > 0)
			{
				free = false;
				
				float capturedtimer = airesources.alltimer;

				airesources.resourceLevels["Iron"] -= 1;
				airesources.resourceLevels["Stone"] -= 1;
				airesources.resourceLevels["Wood"] -= 1;
				free = true;
			}
			break;
		case "Storage":
			if (airesources.resourceLevels["Stone"] > 0 && airesources.resourceLevels["Wood"] > 0)
			{
				if (hasSkill == false)
				{
					return;
				}
				free = false;

				
				float capturedtimer = airesources.alltimer;

				airesources.resourceLevels["Stone"] -= 1;
				airesources.resourceLevels["Wood"] -= 1;
				free = true;
			}
			break;
		case "Hut":
			//Find Closest, no resources needed.
			free = false;

			float capturedtimer = airesources.alltimer;

			free = true;
			break;
		case "House":
			if (airesources.resourceLevels["Stone"] > 0 && airesources.resourceLevels["Wood"] > 0)
			{
				if(hasSkill == false)
				{
					return;
				}
				free = false;

				capturedtimer = airesources.alltimer;

				airesources.resourceLevels["Stone"] -= 1;
				airesources.resourceLevels["Wood"] -= 1;
				free = true;
			}
			break;
		case "Mine":
			if (airesources.resourceLevels["Iron"] > 0 && airesources.resourceLevels["Wood"] > 0)
			{
				if(hasSkill == false)
				{
					return;
				}
				free = false;

				capturedtimer = airesources.alltimer;

				airesources.resourceLevels["Iron"] -= 1;
				airesources.resourceLevels["Wood"] -= 1;
				free = true;
			}
			break;
		case "Quarry":
			free = false;

			capturedtimer = airesources.alltimer;
			//Find closest, no resources.
			free = true;
			break;
		case "Smelter":
			if (airesources.resourceLevels["Stone"] > 0)
			{
				free = false;
				
				capturedtimer = airesources.alltimer;

				airesources.resourceLevels["Stone"] -= 1;
				free = true;
			}
			break;
		case "Barracks":
			if (airesources.resourceLevels["Stone"] > 0 && airesources.resourceLevels["Wood"] > 0)
			{
				if(hasSkill == false)
				{
					return;
				}
				free = false;
				
				capturedtimer = airesources.alltimer;

				airesources.resourceLevels["Stone"] -= 1;
				airesources.resourceLevels["Wood"] -= 1;
				free = true;
			}
			break;
		case "Market":
			if (airesources.resourceLevels["Wood"] > 0)
			{
				if (hasSkill == false)
				{
					return;
				}
				free = false;
				
				capturedtimer = airesources.alltimer;

				airesources.resourceLevels["Wood"] -= 1;
				free = true;
			}
			break;
		}
	}

	IEnumerator FinalMove()
	{
		foreach (Tile e in pathing.final_path) 
		{
			Debug.Log ("Hello");
			yield return new WaitForSeconds(1);
			position.x = e.position.x;
			position.y = e.position.y;
		}
		yield return null;
	}

	public void MoveTo(string resource)
	{
		//Use Distance to set time of move. Think about lerping between the move.
		free = false;
		float capturedtimer = airesources.alltimer;
		Tile found;

		switch (resource) 
		{
		case "FOREST":
			//Find Closest Forest.
			found = pathing.GetClosest(3, position, 0);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove());
			free = true;
			break;
		case "STONE":
			//Find Closest Stone.
			found = pathing.GetClosest(4, position, 0);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "ORE":
			//Find Closest Ore.
			found = pathing.GetClosest(2, position, 0);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "COAL":
			//Find Closest Coal.
			found  = pathing.GetClosest(1, position, 0);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "SMELTER":
			found = pathing.GetClosest(14, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "SAWMILL":
			found = pathing.GetClosest(12, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "BARRACKS":
			found = pathing.GetClosest(5, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "SCHOOL":
			found = pathing.GetClosest(13, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "STORAGE":
			found = pathing.GetClosest(15, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "HUT":
			found = pathing.GetClosest(8, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "HOUSE":
			found = pathing.GetClosest(7, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "QUARRY":
			found = pathing.GetClosest(11, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "MINE":
			found = pathing.GetClosest(10, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "MARKET":
			found = pathing.GetClosest(9, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		case "BLACKSMITH":
			found = pathing.GetClosest(6, position, team);

			pathing.Finding(position, found.position);
			StartCoroutine(FinalMove ());
			free = true;
			break;
		}
	}

	public void Craft(string resource)
	{

		if (hasSkill == false)
			return;

		free = false;

		float capturedtimer = airesources.alltimer;
		
		while (airesources.alltimer < (capturedtimer + 10))

		switch (resource) 
		{
		case "AXE":
			//Find Nearest Blacksmith

			
			free = true;
			break;
		case "RIFLE":
			//Find Nearest Blacksmith

			
			free = true;
			break;
		case "CART":
			//Find Nearest Blacksmith.

			
			free = true;
			break;
		}
	}

	public void Smelt(string resource)
	{
		//Find Closest Smelter.
		Tile found = pathing.GetClosest (14, position, team);
		free = false;

		pathing.Finding (position, found.position);
		StartCoroutine (FinalMove());
		float capturedtimer = airesources.alltimer;
		
		while (airesources.alltimer < (capturedtimer + 5))

		airesources.resourceLevels ["Ore"] -= 1;
		airesources.resourceLevels ["Coal"] -= 1;
		airesources.resourceLevels ["Iron"] += 1;
	}

	public void Store(string resource)
	{
		if (hasSkill == false)
			return;

		free = false;
		//Time at function start, use to complete time.

		float capturedtimer = airesources.alltimer;

		while (airesources.alltimer < (capturedtimer + 1))

		switch (resource) 
		{
		case "IRON":
			airesources.resourceLevels["Iron"] -= 1;
			airesources.resourceLevels["Money"] += 30;
			free = true;
			break;
		case "WOOD":
			airesources.resourceLevels["Wood"] -= 1;
			airesources.resourceLevels["Money"] += 30;
			free = true;
			break;
		case "TIMBER":
			airesources.resourceLevels["Timber"] -= 1;
			airesources.resourceLevels["Money"] += 10;
			free = true;
			break;
		case "ORE":
			airesources.resourceLevels["Ore"] -= 1;
			airesources.resourceLevels["Money"] += 10;
			free = true;
			break;
		case "COAL":
			airesources.resourceLevels["Coal"] -= 1;
			airesources.resourceLevels["Money"] += 10;
			free = true;
			break;
		case "RIFLE":
			airesources.resourceLevels["Rifle"] -= 1;
			airesources.resourceLevels["Money"] += 10;
			free = true;
			break;
		case "AXE":
			airesources.resourceLevels["Axe"] -= 1;
			airesources.resourceLevels["Money"] += 10;
			free = true;
			break;
		case "CART":
			airesources.resourceLevels["Cart"] -= 1;
			airesources.resourceLevels["Money"] += 10;
			free = true;
			break;
		case "STONE":
			airesources.resourceLevels["Stone"] -= 1;
			airesources.resourceLevels["Money"] += 10;
			free = true;
			break;
		}
		
	}

	public void Breed(GameObject other)
	{
		free = false;
		//Find Closest Hut/House.
		
		float capturedtimer = airesources.alltimer;

		GameObject a = Instantiate (Resources.Load ("Labourer"), transform.position, transform.rotation) as GameObject;
		a.transform.parent = gameObject.transform.parent;
		a.GetComponent<Labourer> ().team = team;
	}

	public void Mine(string resource)
	{
		free = false;
		
		float capturedtimer = airesources.alltimer;

		if (resource == "COAL") 
		{
			//Find closest resource.

			airesources.resourceLevels ["Coal"] += 1;
			free = true;
		}
		else if (resource == "ORE") 
		{

			airesources.resourceLevels["Ore"] += 1;
			free = true;
		}
	}

	public void Quarry()
	{
		free = false;

		//Find Closest Stone.
		
		float capturedtimer = airesources.alltimer;

		airesources.resourceLevels ["Stone"] += 1;
		free = true;
	}

	public void Educate(GameObject other, string skill)
	{
		free = false;
		Labourer o = other.GetComponent<Labourer> ();
		if (o.hasSkill)
		{
			free = true;
			return;
		}

		float capturedtimer = airesources.alltimer;
		//Potentially cancel all plans for a new recruit.
		o.Plan.Clear ();

		while (o.free == false) {

		}

		o.free = false;

		while (airesources.alltimer < (capturedtimer + 100))

		//Teach the other a skill.
		switch (skill) 
		{
		case "LUMBERJACK":
			o.isLumberjack = true;
			free = true;
			o.free = true;
			break;
		case "BLACKSMITH":
			o.isBlacksmith = true;
			free = true;
			o.free = true;
			break;
		case "MINER":
			o.isMiner = true;
			free = true;
			o.free = true;
			break;
		case "TRADER":
			o.isTrader = true;
			free = true;
			o.free = true;
			break;
		case "CARPENTER":
			o.isCarpenter = true;
			free = true;
			o.free = true;
			break;
		}
	}

	public void Train(GameObject other)
	{
		free = false;
		Labourer o = other.GetComponent<Labourer> ();
		if (o.free == false)
		{
			free = true;
			return;
		}

		GameObject a = Instantiate (Resources.Load ("Rifleman"), o.transform.position, o.transform.rotation) as GameObject;
		a.transform.parent = transform.parent;
		Destroy(o);
		free = true;
	}

	public void SuperEducate(GameObject other, string skill)
	{
		free = false;
		Labourer o = other.GetComponent<Labourer> ();
		if (o.hasSkill || o.free == false)
		{
			free = true;
			return;
		}
		
		//Potentially cancel all plans for a new recruit.

		//Find the nearest school. If no school is available return out.

		float capturedtimer = airesources.alltimer;

		while (airesources.alltimer < (capturedtimer + 50))

		//Teach the other a skill.
		switch (skill) 
		{
		case "LUMBERJACK":
			o.isLumberjack = true;
			free = true;
			o.free = true;
			break;
		case "BLACKSMITH":
			o.isBlacksmith = true;
			free = true;
			o.free = true;
			break;
		case "MINER":
			o.isMiner = true;
			free = true;
			o.free = true;
			break;
		case "TRADER":
			o.isTrader = true;
			free = true;
			o.free = true;
			break;
		case "CARPENTER":
			o.isCarpenter = true;
			free = true;
			o.free = true;
			break;
		}
	}

	public void BuySell(string resource)
	{
		//Not Implemented
	}

	public void CutTree()
	{
		//Find Closest Tree.
		Tile found = pathing.GetClosest (3, position, team);

		pathing.Finding (position, found.position);
		StartCoroutine ("FinalMove");
		free = false;
		float capturedtimer = airesources.alltimer;

		airesources.resourceLevels ["Timber"] += 1;
		free = true;

	}

	public override void Death()
	{
		Destroy (this.transform);
	}

}
