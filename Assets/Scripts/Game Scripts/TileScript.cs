//Christopher Vertigan - N3078812
//17/02/2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TileScript
{

    //Tile Struct
    public struct Tile
    {
        public int type_, resource_;

        public Tile(int type1, int resource1)
        {
            type_ = type1;
            resource_ = resource1;
        }
    }

    //Member Variables.
	public Sprite[] sprites;
    private int mapSizeX_, mapSizeY_;
    private char[] mapChars_;
    public Tile[,] tiles_;
    public int[] tileNumbers_;

    private enum TileTypes: int
    {
        Passable,
        Obstructed,
        School,
        Storage,
        Smelter,
        Barracks,
        Hut,
        House,
        Blacksmiths,
        Bridge
    }

    /*private enum Resources : int
    {
        Stone,
        Timber,
        Ore,
        Coal,
        Tool,
        Weapon,
        Cart,
        Empty
    }*/

    public TileScript (int x, int y)
    {
        mapSizeX_ = x;
        mapSizeY_ = y;

        tiles_ = new Tile[mapSizeX_, mapSizeY_];


    }

    ~TileScript()
    {
        
    }

    Tile createTile(int type, int resource)
    {
        Tile tile = new Tile(type, resource);

        return tile;
    }

    public bool mapReader(string filename) 
    {
        //Aurora Map is 768x, 1024y
		StreamReader streamReader = new StreamReader (Application.dataPath + "/Resources/Maps/" + filename);
		string mapString_ = streamReader.ReadToEnd ();
		mapChars_ = mapString_.ToCharArray();
        tileNumbers_ = new int[mapChars_.Length];

        for (int i = 0; i < mapChars_.Length; i++)
        {
            if (mapChars_[i] == '@')
            {
                tileNumbers_[i] = 0;
            }
            else if (mapChars_[i] == '.')
            {
                tileNumbers_[i] = 1;
            }
        }

        return false;
    }

    public int createMap()
    {
        int limiter_ = 0;


        for (int i = 0; i < mapSizeX_; i++)
        {
            for (int k = 0; k < mapSizeY_; k++)
            {
				if (limiter_ == 81)
					Debug.Log(tileNumbers_[limiter_]);
                tiles_[i, k] = createTile(tileNumbers_[limiter_], 0);
                limiter_++;
            }
        }
        return 0;
    }

}
