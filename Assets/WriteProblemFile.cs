using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WriteProblemFile : MonoBehaviour {


	string goal;
	// Use this for initialization
	void Start () {



		//WriteFile ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonPressed() {
		string command = GameObject.FindGameObjectWithTag ("commandtext").GetComponent<Text>().text;
		ParseCommands (command);
	}

	public void ParseCommands(string command) {

		IDictionary<string, int> resources = new Dictionary<string, int>();
		IDictionary<string, int> buildings = new Dictionary<string, int>();
		Labourer[] agents = new Labourer[8];
		agents = Object.FindObjectsOfType<Labourer> ();
		
	//	string goal = "(and (> (stone) 0) (> (iron) 0))";
		
		resources = FillResourceValuesForTesting (resources);
		buildings = FillBuildingValuesForTesting (buildings);

		string parsedCommand = "";

		bool suitableCommand = true;

		bool isGet = command.StartsWith ("get ", System.StringComparison.CurrentCultureIgnoreCase);
		bool isBuild = command.StartsWith ("build ", System.StringComparison.CurrentCultureIgnoreCase);
		bool isAttack = command.StartsWith ("attack ", System.StringComparison.CurrentCultureIgnoreCase);
		bool isMove = command.StartsWith ("move ", System.StringComparison.CurrentCultureIgnoreCase);
		bool isCraft = command.StartsWith ("craft ", System.StringComparison.CurrentCultureIgnoreCase);

		if (isGet == false && isBuild == false && isAttack == false && isMove == false) {
			suitableCommand = false;
			UnityEngine.Debug.Log("Command Failed.");
		}

		if (isGet == true) {
			int whichResource = 0;
			bool isStone = command.EndsWith (" stone", System.StringComparison.CurrentCultureIgnoreCase);
			bool isCoal = command.EndsWith (" coal", System.StringComparison.CurrentCultureIgnoreCase);
			if (isCoal == true)
				whichResource = 1;
			bool isOre = command.EndsWith (" ore", System.StringComparison.CurrentCultureIgnoreCase);
			if (isOre == true)
				whichResource = 2;
			bool isIron = command.EndsWith (" iron", System.StringComparison.CurrentCultureIgnoreCase);
			if (isIron == true)
				whichResource = 3;
			bool isTimber = command.EndsWith (" timber", System.StringComparison.CurrentCultureIgnoreCase);
			if (isTimber == true)
				whichResource = 4;
			bool isWood = command.EndsWith (" wood", System.StringComparison.CurrentCultureIgnoreCase);
			if (isWood == true)
				whichResource = 5;
			bool isMoney = command.EndsWith (" money", System.StringComparison.CurrentCultureIgnoreCase);
			if (isMoney == true)
				whichResource = 6;
			bool isAxe = command.EndsWith (" axe", System.StringComparison.CurrentCultureIgnoreCase);
			if (isAxe == true)
				whichResource = 7;
			bool isCart = command.EndsWith (" cart", System.StringComparison.CurrentCultureIgnoreCase);
			if (isCart == true)
				whichResource = 8;
			bool isRifle = command.EndsWith (" rifle", System.StringComparison.CurrentCultureIgnoreCase);
			if (isRifle == true)
				whichResource = 9;

			int howMany = 0;
			int first = command.IndexOf ("get ") + "get ".Length;
			int last = command.LastIndexOf(" ");
			int.TryParse(command.Substring(first, last - first), out howMany);

			switch(whichResource)
			{
			case 0:
				parsedCommand = "(> (stone) " + (howMany - 1) + ")";
				break;
			case 1:
				parsedCommand = "(> (coal) " + (howMany - 1) + ")";
				break;
			case 2:
				parsedCommand = "(> (ore) " + (howMany - 1) + ")";
				break;
			case 3:
				parsedCommand = "(> (iron) " + (howMany - 1) + ")";
				break;
			case 4:
				parsedCommand = "(> (timber) " + (howMany - 1) + ")";
				break;
			case 5:
				parsedCommand = "(> (wood) " + (howMany - 1) + ")";
				break;
			case 6:
				parsedCommand = "(> (money) " + (howMany - 1) + ")";
				break;
			case 7:
				parsedCommand = "(> (axe) " + (howMany - 1) + ")";
				break;
			case 8:
				parsedCommand = "(> (cart) " + (howMany - 1) + ")";
				break;
			case 9:
				parsedCommand = "(> (rifle) " + (howMany - 1) + ")";
				break;
			default:
				break;
			}
		
		}

		if (isBuild == true) {
			int whichBuilding = 0;
			bool isHut = command.EndsWith ("hut", System.StringComparison.CurrentCultureIgnoreCase);
			bool isHouse = command.EndsWith ("house", System.StringComparison.CurrentCultureIgnoreCase);
			if (isHouse == true)
				whichBuilding = 1;
			bool isSchool = command.EndsWith ("school", System.StringComparison.CurrentCultureIgnoreCase);
			if (isSchool == true)
				whichBuilding = 2;
			bool isBarracks = command.EndsWith ("barracks", System.StringComparison.CurrentCultureIgnoreCase);
			if (isBarracks == true)
				whichBuilding = 3;
			bool isStorage = command.EndsWith ("storage", System.StringComparison.CurrentCultureIgnoreCase);
			if (isStorage == true)
				whichBuilding = 4;
			bool isMine = command.EndsWith ("mine", System.StringComparison.CurrentCultureIgnoreCase);
			if (isMine == true)
				whichBuilding = 5;
			bool isSmelter = command.EndsWith ("smelter", System.StringComparison.CurrentCultureIgnoreCase);
			if (isSmelter == true)
				whichBuilding = 6;
			bool isQuarry = command.EndsWith ("quarry", System.StringComparison.CurrentCultureIgnoreCase);
			if (isQuarry == true)
				whichBuilding = 7;
			bool isSawmill = command.EndsWith ("sawmill", System.StringComparison.CurrentCultureIgnoreCase);
			if (isSawmill == true)
				whichBuilding = 8;
			bool isBlacksmith = command.EndsWith ("blacksmith", System.StringComparison.CurrentCultureIgnoreCase);
			if (isBlacksmith == true)
				whichBuilding = 9;
			bool isMarket = command.EndsWith ("market", System.StringComparison.CurrentCultureIgnoreCase);
			if (isMarket == true)
				whichBuilding = 10;

			switch(whichBuilding)
			{
			case 0:
				parsedCommand = "(> (huts) " + buildings["huts"] + ")";
				break;
			case 1:
				parsedCommand = "(> (houses) " + buildings["houses"] + ")";
				break;
			case 2:
				parsedCommand = "(> (schools) " + buildings["schools"] + ")";
				break;
			case 3:
				parsedCommand = "(> (barracks) " + buildings["barracks"] + ")";
				break;
			case 4:
				parsedCommand = "(> (storages) " + buildings["storages"] + ")";
				break;
			case 5:
				parsedCommand = "(> (mines) " + buildings["mines"] + ")";
				break;
			case 6:
				parsedCommand = "(> (smelters) " + buildings["smelters"] + ")";
				break;
			case 7:
				parsedCommand = "(> (quarries) " + buildings["quarries"] + ")";
				break;
			case 8:
				parsedCommand = "(> (sawmills) " + buildings["sawmills"] + ")";
				break;
			case 9:
				parsedCommand = "(> (blacksmiths) " + buildings["blacksmiths"] + ")";
				break;
			case 10:
				parsedCommand = "(> (markets) " + buildings["markets"] + ")";
				break;
			default:
				break;
			}



		}


		if (suitableCommand == true) {

			goal = parsedCommand;
			string[] lines = CreateGameState (agents, resources, buildings, goal);
			WriteFile (lines);
		}

	}

	public void WriteFile(string[] lines) {
	
		
		System.IO.File.WriteAllLines (Application.dataPath + @"/Planner/task-problem.pddl", lines);

		GetComponent<TaskPlannerProcess> ().ReadFile ();

	}

	string[] CreateGameState(Labourer[] agents, IDictionary<string, int> resources, IDictionary<string, int> buildings, string goal)
	{
		string[] resourcesStrings = { "stone", "coal", "ore", "iron", "timber", "wood", "money", "axes", "carts", "rifles" };
		for (int i = 0; i < resources.Count; i++) {
			resourcesStrings[i] = "(= (" + resourcesStrings[i]  + ") " + resources[resourcesStrings[i]] + ") ";
		}

		string[] buildingsStrings = { "huts", "houses", "schools", "barracks", "storages", "mines", "smelters", "quarries",
			"sawmills", "blacksmiths", "markets" };
		for (int i = 0; i < buildings.Count; i++) {
			buildingsStrings[i] = "(= (" + buildingsStrings[i] + ") " + buildings[buildingsStrings[i]] + ") ";
		}



		string[] lines = new string[5];
		lines [0] = "(define (problem task-problem)";
		lines [1] = "(:domain task)";

		string objectsLine = "(:objects ";

		for (int i = 0; i < agents.Length; i++) {
			objectsLine += agents[i].name + " ";
		}

		objectsLine += ")";

		string initLine = "(:init ";

		for (int i = 0; i < agents.Length; i++) {
			if (agents[i].isPerson == true)
				initLine += "(isperson " + agents[i].name + ") ";
			if (agents[i].isLabourer == true)
				initLine += "(islabourer " + agents[i].name + ") ";
			if (agents[i].isCarpenter == true)
				initLine += "(iscarpenter " + agents[i].name + ") ";
			if (agents[i].isBlacksmith == true)
				initLine += "(isblacksmith " + agents[i].name + ") ";
			if (agents[i].isTeacher == true)
				initLine += "(isteacher " + agents[i].name + ") ";
			if (agents[i].isMiner == true)
				initLine += "(isminer " + agents[i].name + ") ";
			if (agents[i].isTrader == true)
				initLine += "(istrader " + agents[i].name + ") ";
			if (agents[i].isLumberjack == true)
				initLine += "(islumberjack " + agents[i].name + ") ";
			if (agents[i].isRifleman == true)
				initLine += "(isrifleman " + agents[i].name + ") ";
			if (agents[i].hasSkill == true)
				initLine += "(hasskill " + agents[i].name + ") ";
			if (agents[i].nearbyForest == true)
				initLine += "(nearbyforest " + agents[i].name + ") ";
			if (agents[i].nearbyStone == true)
				initLine += "(nearbystone " + agents[i].name + ") ";
			if (agents[i].nearbyOre == true)
				initLine += "(nearbyore " + agents[i].name + ") ";
			if (agents[i].nearbyCoal == true)
				initLine += "(nearbycoal " + agents[i].name + ") ";

		}

		for (int i = 0; i < resourcesStrings.Length; i++) {
			initLine += resourcesStrings[i];
		}
		for (int i = 0; i < buildingsStrings.Length; i++) {
			initLine += buildingsStrings[i];
		}

		initLine += "(= (people) " + agents.Length + ")";
		initLine += ")";
			
		string goalLine = "(:goal " + goal + ")" + ")";

		lines [2] = objectsLine;
		lines [3] = initLine;
		lines [4] = goalLine;

		return lines;


	}

	IDictionary<string, int> FillResourceValuesForTesting(IDictionary<string, int> resources)
	{
		resources.Add ("stone", 0);
		resources.Add ("coal", 0);
		resources.Add ("ore", 0);
		resources.Add ("iron", 0);
		resources.Add ("timber", 0);
		resources.Add ("wood", 0);
		resources.Add ("money", 0);
		resources.Add ("axes", 0);
		resources.Add ("carts", 0);
		resources.Add ("rifles", 0);
		
		return resources;
	}

	IDictionary<string, int> FillBuildingValuesForTesting(IDictionary<string, int> buildings)
	{
		buildings.Add ("huts", 0);
		buildings.Add ("houses", 0);
		buildings.Add ("schools", 0);
		buildings.Add ("barracks", 0);
		buildings.Add ("storages", 1);
		buildings.Add ("mines", 0);
		buildings.Add ("smelters", 0);
		buildings.Add ("quarries", 0);
		buildings.Add ("sawmills", 0);
		buildings.Add ("blacksmiths", 0);
		buildings.Add ("markets", 0);

		
		return buildings;
	}
}
