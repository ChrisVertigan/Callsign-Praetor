using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#region AI
public class AIController : MonoBehaviour {


	public enum States : int
	{
		WAR,
		Peace,
		Build,
		Trading,
		Training,
		Education,
		Population,
		Deforestation,
		Mining
	}

	public float alltimer = 0;
	float timer = 0;
	List<Labourer> labourers = new List<Labourer>();
	Tile[,] board;
	Tile[] buildings;
	int i, j, TeamNo;


	public Dictionary<string, int> resourceLevels = new Dictionary<string, int>();

	// Use this for initialization
	void Start () 
	{
		board = new Tile[GameObject.Find ("GameController").GetComponent<GameController> ().tiles_.GetLength (0), GameObject.Find ("GameController").GetComponent<GameController> ().tiles_.GetLength (1)];
		TeamNo = GetComponent<AIPlayer> ().playerID_;
		labourers.AddRange(gameObject.GetComponentsInChildren<Labourer>());
		//AI Starts with 3 wood and 3 stone with which to build initial buildings.
		resourceLevels.Add ("Stone", 3);
		resourceLevels.Add ("Timber", 3);
		resourceLevels.Add ("Ore", 0);
		resourceLevels.Add ("Wood", 0);
		resourceLevels.Add ("Iron", 0);
		resourceLevels.Add ("Coal", 0);
		resourceLevels.Add ("Rifle", 0);
		resourceLevels.Add ("Axe", 0);
		resourceLevels.Add ("Cart", 0);
		resourceLevels.Add ("Money", 0);

	}
	
	// Update is called once per frame
	void Update () {
	
		alltimer += Time.deltaTime;
		timer += Time.deltaTime;

		if (timer >= 1) 
		{
			//HostileSweep();
			timer = 0;
			i++;
			j++;
		}

		if (i == 2) 
		{
			LabourerCheck();
		}

		if (j == 5) 
		{
			AssessBoard();
		}
	}

	/* Waiting on pathfinding   public void HostileSweep()
	{

	}*/

	public void LabourerCheck()
	{
		for (int a = 0; a < labourers.Count; a++) 
		{
			if(labourers[a].free == true)
			{
				AssessBoard();
			}
		}
		
		i = 0;
	}

	public void Build(string building)
	{
		GameController gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		WriteProblemFile wpf = gc.transform.GetComponent<WriteProblemFile> ();

		for (int i = 0; i < labourers.Count; i++)
		{
			if (labourers[i].free == true)
			{
				switch (building)
				{
				case "Sawmill":
					wpf.ParseCommands("build 1 sawmill");
					return;
				case "Hut":
					wpf.ParseCommands("build 1 hut");
					return;
				case "House":
					wpf.ParseCommands("build 1 house");
					return;
				case "Barracks":
					wpf.ParseCommands("build 1 barracks");
					return;
				case "Storage":
					wpf.ParseCommands("build 1 storage");
					return;
				case "Smelter":
					wpf.ParseCommands("build 1 smelter");
					return;
				case "Mine":
					wpf.ParseCommands("build 1 mine");
					return;
				case "School":
					wpf.ParseCommands("build 1 school");
					return;
				case "Quarry":
					wpf.ParseCommands("build 1 quarry");
					return;
				}
			}
		}
	}




	public void AssessBoard()
	{

		int a = 0;
		int b = 0;

		GameController gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		WriteProblemFile wpf = gc.transform.GetComponent<WriteProblemFile> ();

		foreach (Tile e in gc.tiles_) 
		{
			board[a, b] = e;
			a++;
			if(a >= gc.mapX_)
			{
				a = 0;
				b++;
			}
		}


	
		foreach (KeyValuePair<string, int> entry in resourceLevels) 
		{
			if (entry.Key == "Stone" && entry.Value < 2)
			{
				foreach (Labourer e in labourers)
				{
					if (e.free == true)
					{
						wpf.ParseCommands("get 1 stone");
						j = 0;
						return;
					}
				}
			}

			if (entry.Key == "Timber" && entry.Value < 2)
			{
				foreach (Labourer e in labourers)
				{
					if (e.free == true)
					{
						wpf.ParseCommands("get 1 timber");
						j = 0;
						return;
					}
				}
			}

			if (entry.Key == "Coal" && entry.Value < 1)
			{
				foreach (Labourer e in labourers)
				{
					if (e.free == true)
					{
						wpf.ParseCommands("get 1 coal");
						j = 0;
						return;
					}
				}
			}

			if (entry.Key == "Ore" && entry.Value < 1)
			{
				foreach (Labourer e in labourers)
				{
					if (e.free == true)
					{
						wpf.ParseCommands("get 1 ore");
						j = 0;
						return;
					}
				}
			}

			if (entry.Key == "Rifle" && entry.Value < 1)
			{
				foreach (Labourer e in labourers)
				{
					if (e.free == true)
					{
						if (e.skill == 2)
						{
							foreach (Tile z in board)
							{
								if (z.state == 6 && z.team == TeamNo)
								{
									wpf.ParseCommands("get 1 rifle");
									j = 0;
									return;
								}
							}
						}
					}
				}
			}

			if (entry.Key == "Axe" && entry.Value < 1)
			{
				foreach (Labourer e in labourers)
				{
					if (e.free == true)
					{
						if (e.skill == 2)
						{
							foreach (Tile z in board)
							{
								if (z.state == 6 && z.team == TeamNo)
								{
									wpf.ParseCommands("get 1 axe");
									j = 0;
									return;
								}
							}
						}
					}
				}
			}

			if (entry.Key == "Iron" && entry.Value < 1)
			{
				foreach (Labourer e in labourers)
				{
					if (e.free == true)
					{
						if (e.skill == 2)
						{
							foreach (Tile z in board)
							{
								if (z.state == 14 && z.team == TeamNo)
								{
									wpf.ParseCommands("get 1 iron");
									j = 0;
									return;
								}
							}
						}
					}
				}
			}

			if (entry.Key == "Wood" && entry.Value < 1)
			{
				foreach (Labourer e in labourers)
				{
					if (e.free == true)
					{
						if (e.skill == 2)
						{
							foreach (Tile z in board)
							{
								if (z.state == 12 && z.team == TeamNo)
								{
									wpf.ParseCommands("get 1 wood");
									j = 0;
									return;
								}
							}
						}
					}
				}
			}

			if (entry.Key == "Money" && entry.Value < 30)
			{
				foreach (Labourer e in labourers)
				{
					if (e.free == true)
					{
						if (e.skill == 5)
						{
							foreach (Tile z in board)
							{
								if (z.state == 15 && z.team == TeamNo)
								{
									wpf.ParseCommands("get 30 money");
									j = 0;
									return;
								}
							}
						}
					}
				}
			}
		}

	}



}
#endregion