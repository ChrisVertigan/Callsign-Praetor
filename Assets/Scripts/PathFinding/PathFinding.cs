using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {


	Tile tilestart;
	Tile goalTile;
	Tile current_node;
	Tile lowest_F;
	Tile closest;
	Tile _node;
	List<Tile> open_list = new List<Tile>();
	List<Tile> closed_list = new List<Tile>();
	public List<Tile> final_path = new List<Tile>();
	List<Tile> succesor_list = new List<Tile>();
	GameController gc;

	public PathFinding()
	{

	}

	void Clear()
	{
		open_list.Clear ();
		final_path.Clear ();
		closed_list.Clear ();
		succesor_list.Clear ();
	}

	public Tile GetClosest(int defined_tile_type, Vector2 position, int teamNo) {

		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		List<Tile> tile_type = new List<Tile>();
		List<Tile> tempo = new List<Tile> ();
		 

		int temp = 0;
		int index = 0;
		
		for (int i = 0; i < gc.tiles_.GetLength(0); i++) {
			
			for (int j = 0; j < gc.tiles_.GetLength (1); j++) 
			{
				_node = gc.tiles_[0, 0];
				_node.position.x = i;
				_node.position.y = j;
				_node.state = gc.tiles_[i, j].state;
				if(_node.state == defined_tile_type)
				{
					tempo.Add(_node);
				}
			}
		}


		if (tempo == null) 
		{
			Debug.Log ("Error");
			return null;
		}

		float distance = 9999999;

		foreach (Tile d in tempo) 
		{
			float tempx = d.position.x -= position.x;
			float tempy = d.position.y -= position.y;

			float tempe = tempx + tempy;

			if (tempe < 0)
			{
				tempe *= -1;
			}

			if (tempe < distance)
			{
				distance = tempe;
				closest = d;
			}
		}

		/*for (int a = 0; a < distance.Count; a++) {
			
			if (distance[a] < index) {
				
				index = a;
			}
		}*/
		Debug.Log (closest.position);
		return closest;
	}

	public void Finding( Vector2 start_pos, Vector2 finish_pos ) {
		
		Clear ();

		int array_position = 0;
		
		// Initialize the start node

		lowest_F.G = 99999;
		tilestart.G = 0;
		tilestart.H = ((finish_pos.x - start_pos.x) + (finish_pos.y - start_pos.y)) * 10;
		tilestart.F = tilestart.G + tilestart.H;
		
		tilestart.position = start_pos;
		
		// Initialize the goal node
		goalTile.G = 0;
		goalTile.H = 0;
		goalTile.F = 0;
		
		goalTile.position = finish_pos;
		
		// Set the current node at the start
		current_node = tilestart;
		
		// Start of the A* algorithm
		open_list.Add (tilestart);
		
		
		while (open_list.Count > 0) {
			
			bool skip_node = false;
			
			lowest_F = open_list [0];
			array_position = 0;
			// Lower F and put it in "current_node"
			for (int i = 1; i < open_list.Count; ++i) {
				if (open_list [i].F < lowest_F.F) {
					lowest_F = open_list [i];
					array_position = i;
				}
			}
			
			current_node = lowest_F;
			
			// Withdraws the node from the array
			open_list.RemoveAt (array_position);
			
			// Check if it's the final node
			if (current_node.position.x == goalTile.position.x && 
				current_node.position.y == goalTile.position.y) {
				break;
			}
			
			// Clean the succesor array
			succesor_list.Clear ();
			
			// Generate the succesors
			for (int j = 0; j < 8; j++) {
				Tile _new_succesor = current_node;
				_new_succesor.state = j;
				
				switch (j) {
				case 0:
					_new_succesor.position.x++;
					break;
					
				case 1:
					_new_succesor.position.x++;
					_new_succesor.position.y++;
					break;
					
				case 2:
					_new_succesor.position.y++;
					break;
					
				case 3:
					_new_succesor.position.x--;
					_new_succesor.position.y++;
					break;
					
				case 4:
					_new_succesor.position.x--;
					break;
					
				case 5:
					_new_succesor.position.x--;
					_new_succesor.position.y--;
					break;
					
				case 6:
					_new_succesor.position.y--;
					break;
					
				case 7:
					_new_succesor.position.x++;
					_new_succesor.position.y--;
					break;
				}
				
				succesor_list.Add (_new_succesor);
			}
			
			
			// for each of the succesors
			for (int t = 0; t < 8; t++) {

					
				// Calculate G
				if (t == 0 || t == 2 || t == 4 || t == 6) {
					succesor_list [t].G = current_node.G + 10;
				} else if (t == 1 || t == 3 || t == 5 || t == 7) {
					succesor_list [t].G = current_node.G + 14;
				}
					
					
				// Search in open list
					
				for (int g = 0; g < open_list.Count; g++) {
						
					// if it's in the open list, compare them and it's equal or better than this, discart this
					if (open_list [g].position.x == succesor_list [t].position.x &&
						open_list [g].position.y == succesor_list [t].position.y) {
							
						if (open_list [g].G <= succesor_list [t].G) {
							skip_node = true;
						} else if (open_list [g].G > succesor_list [t].G) {
							open_list.RemoveAt (g);
						}
							
							
					}
				}
				// Search in the closed list
				for (int c = 0; c < closed_list.Count; c++) {
						
					// if it's in the closed list, compare them and it's equal or better than this, discart this
					if (closed_list [c].position.x == succesor_list [t].position.x &&
						closed_list [c].position.y == succesor_list [t].position.y) {
							
						// If it's as good or equal, discard this succesor and continue with the next
						if (closed_list [c].G <= succesor_list [t].G) {
							skip_node = true;
						} else if (closed_list [c].G > succesor_list [t].G) {
							closed_list.RemoveAt (c);
						}
							
					}
						
				}
					
				// eliminates the node succesor from the open and closed list
					
					
				// set the parent of the succesor to the current node
				if (skip_node == false) {
					succesor_list [t].parent_position = current_node.position;
					succesor_list [t].parent = current_node;
						
					// Calculate the H and update the F
					int _internal_x = (int)(finish_pos.x - succesor_list [t].position.x);
					int _internal_y = (int)(finish_pos.y - succesor_list [t].position.y);
					if (_internal_x < 0) {
						_internal_x = _internal_x * (-1);
					}
					if (_internal_y < 0) {
						_internal_y = _internal_y * (-1);
					}
						
					succesor_list [t].H = (_internal_x + _internal_y) * 10;
					succesor_list [t].F = succesor_list [t].G + succesor_list [t].H;
						
					// Insert the actual node in the open list
					Tile _node_2 = succesor_list [t];
					open_list.Add (_node_2);
				}
					
				skip_node = false;
			}
		}
			
		// Insert the actual node to the closed list
		closed_list.Add (current_node);
			
	
		
		// End of the A* algorithm
		GetPath();
		
	}

	void GetPath() {
		
		if(closed_list.Count > 0) {
			
			// The last node is NOT inserted in the closed list,
			//  so that is why it is the first node to get in.
			final_path.Add(current_node);
			
			Tile _node = closed_list[ closed_list.Count -1];
			
			final_path.Add(_node);
			
			while( ( (_node.parent_position.x != tilestart.position.x) ||
			        (_node.parent_position.y != tilestart.position.y) ) ) { 
				
				for(int i = 0; i < closed_list.Count -1; i++) {
					if(closed_list[i].position.x == _node.parent_position.x && closed_list[i].position.y == _node.parent_position.y) {
						
						_node = closed_list[i];
						
						final_path.Add(_node);
						
					}
					
				}
				
			}
			
		}
		
	}
}
