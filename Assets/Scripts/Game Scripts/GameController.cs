// Christopher Vertigan - N3078812
// 20/02/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	
	public int aiNumber_ = 2;
	public GameObject[] AIs_;
	public int mapX_;
	public int mapY_;
	public string filename_;
	public TileScript mapMaker_;
	public int ai;
	public Tile[,] tiles_;

	private float clock_ = 0; 
	private List<GameObject> tilesg_ = new List<GameObject> ();
	private int x = 0;
	private int y = 0;

	void Start () {

		//Increment tile positions by 0.5 position on the x
		//Increment tile positions by 0.5 position on the y 
		mapMaker_ = new TileScript (mapX_, mapY_);
		mapMaker_.mapReader (filename_);
		mapMaker_.createMap ();
		float xpos = 0;
		float ypos = 0;
		float xx = 0, yy = 0;
		tiles_ = new Tile[mapMaker_.tiles_.GetLength (1), mapMaker_.tiles_.GetLength (0)];

		for (int i = 0; i < mapMaker_.tiles_.Length; i++)
		{
			int random = Random.Range (0, 100);

			int random_ = 0;

			if(random < 60)
			{
				random_ = 0;
			}
			else if (random >= 60 && random < 70)
			{
				random_ = 1;
			}
			else if (random >= 70 && random < 80)
			{
				random_ = 2;
			}
			else if (random >= 80 && random < 90)
			{
				random_ = 3; 
			}
			else if (random >= 90)
			{
				random_ = 4;
			}


			Vector3 pos = new Vector3(xpos, ypos, 0);
			GameObject til = Instantiate(Resources.Load("tilePrefab"), pos, transform.rotation) as GameObject;
			til.transform.parent = gameObject.transform;
			tilesg_.Add(til);
			tiles_[x, y] = tilesg_[i].GetComponent<Tile>();
			tiles_[x, y].passable = mapMaker_.tileNumbers_[i];
			tiles_[x, y].ChangeState(random_, 0);
			x++;
			xx++;


			xpos += 0.5f;
			if (x >= mapX_)
			{
				x = 0;
				xx = 0;
				xpos = 0;
				y++;
				yy++;
				ypos -= 0.5f;
			}

		}


		AIs_ = new GameObject[aiNumber_];
		for (ai = 0; ai < AIs_.Length; ai++)
		{
			if (ai == 0)
			{
				GameObject a = Instantiate(Resources.Load ("AIPlayer"), GameObject.Find("Main Camera").transform.position - (new Vector3(0, 3, -9)), transform.rotation) as GameObject;
				a.GetComponent<AIPlayer>().playerID_ = 0;
			}
			else 
			{
				GameObject a = Instantiate(Resources.Load ("AIPlayer"), GameObject.Find("Main Camera").transform.position + (new Vector3(0, 3, 9)), transform.rotation) as GameObject;
				a.GetComponent<AIPlayer>().playerID_ = 1;
			}
		}


	}
	
	void Update () {
		
		clock_ += Time.deltaTime;
		
	}
}
