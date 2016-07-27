#include "Agent.h"
#include "EntityManager.h"

#define AT_DESTINATION      88888
#define JOB_FINISHED        99999

// Init
CAgent::CAgent() {
  xPosition = 0;
  yPosition = 0;

  image = NULL;
  entityManager = NULL;

  housePosition.x = -1;
  housePosition.y = -1;
  jobPosition.x = -1;
  jobPosition.y = -1;
  bankPosition.x = -1;
  bankPosition.y = -1;
  saloonPosition.x = -1;
  saloonPosition.y = -1;

  inside_building = false;

  internal_timer = 0;
  internal_count = 0;

  srand(time_t(NULL));

  hunger = rand()%10;
  thirst = rand()%10;
  bag = 0;
  bank_acount = 0;
  tired = rand()%10;
  
  _state = STATE_WORKING;
  /*
  // Next action
  if (thirst >= 6) {
	  _state = STATE_DRINKING;
	  printf(" - Agent %d: New state: STATE_DRINKING\n", ID + 1);
  }
  else if (hunger >= 8) {
	  _state = STATE_EATING;
	  printf(" - Agent %d: New state: STATE_EATING\n", ID + 1);
  }
  else if (tired >= 8) {
	  _state = STATE_RESTING;
	  printf(" - Agent %d: New state: STATE_RESTING\n", ID + 1);
  }
  */
  eating_time_cost = rand() % 50 + 20;
  drinking_time_cost = rand() % 50 + 20;
  resting_time_cost = rand() % 100 + 30;
  working_time_cost = rand() % 50 + 20;

  walking_speed = rand() % 5 + 10;

  printf("Eating time: %d\n", eating_time_cost/100);
  printf("Drinking time: %d\n", drinking_time_cost/100);
  printf("Resting time: %d\n", resting_time_cost/100);
  printf("Working time: %d\n", working_time_cost/100);

  printf("Walking time: %d\n", walking_speed/100);

  isFirstTime = true;

  asigned_worker = NULL;

}

CAgent::~CAgent() {

}


// Functions
void CAgent::Update() {

  switch (_state) {
  case STATE_WORKING:
    FSM_Working();
    break;

  case STATE_DRINKING:
	  FSM_Drinking();
	  break;

  case STATE_RESTING:
	  FSM_Resting();
	  break;

  case STATE_DEPOSITING_MONEY:
	  FSM_DepositingMoney();
	  break;

  case STATE_EATING:
	  FSM_Eating();
	  break;

  }

}

void CAgent::Move(int x, int y) {

  xPosition += x;
  yPosition += y;
}

SDL_Texture* CAgent::LoadTexture( int filename_position ) {

  SDL_Texture* new_texture = NULL;

  SDL_Surface* loaded_surface = IMG_Load(IMAGE_FILENAME[filename_position].c_str());

  if( loaded_surface == NULL ) {
    printf("Unable to load the image: %s\n", SDL_GetError());
  }
  else {
    new_texture = SDL_CreateTextureFromSurface( renderer, loaded_surface);

    SDL_FreeSurface( loaded_surface );
  }

  return new_texture;

}

bool CAgent::SetJob() {
  
  return entityManager->SetJob( this );

}

bool CAgent::SetSaloon() {
  
  return entityManager->SetSaloon( this );

}

bool CAgent::SetBank() {

	return entityManager->SetBank(this);

}


void CAgent::Draw(int width, int height) {

  if(!inside_building) {

    SDL_Rect rect;

    rect.w = width;
    rect.h = height;
    rect.x = xPosition * width;
    rect.y = yPosition * height;

    background = image;
      
    SDL_RenderCopy( renderer, background, NULL, &rect );
  
  }

}

void CAgent::Erase(int width, int height) {

  SDL_Rect rect;

  rect.w = width;
  rect.h = height;
  rect.x = xPosition * width;
  rect.y = yPosition * height;

  background = LoadTexture( map[yPosition][xPosition] );
      
  SDL_RenderCopy( renderer, background, NULL, &rect );
  
}

void CAgent::Init(int _x, int _y, int _image, SDL_Renderer *_renderer, SDL_Texture *_background, CEntityManager* entity_manager) {

  renderer = _renderer;
  background = _background;
  xPosition = _x;
  yPosition = _y;

  housePosition.x = _x;
  housePosition.y = _y-1;

  //jobPosition.x = _x+5;
  //jobPosition.y = _y-1;
  
  //jobPosition.x = _x;
  //jobPosition.y = _y-6;

  //bankPosition.x = _x+5;
  //bankPosition.y = _y-6;

  //saloonPosition.x = _x+5;
  //saloonPosition.y = _y-1;

  entityManager = entity_manager;
  
  /////////////////////////////////////////////////////////
  // Load Texture
  SDL_Texture* new_texture = NULL;

  SDL_Surface* loaded_surface = IMG_Load(IMAGE_FILENAME[ _image ].c_str());

  if( loaded_surface == NULL ) {
    printf("Unable to load the image: %s\n", SDL_GetError());
  }
  else {
    new_texture = SDL_CreateTextureFromSurface( renderer, loaded_surface);

    SDL_FreeSurface( loaded_surface );
  }

  image = new_texture;
  /////////////////////////////////////////////////////////
  
  SetJob();

}


// Getters
void CAgent::Position(int &_x, int &_y) {
  _x = xPosition;
  _y = yPosition;
}

int CAgent::X_Position() {
  return xPosition;
}

int CAgent::Y_Position() {
  return yPosition;
}

int CAgent::GetID() {
  return ID;
}

CAgent::AGENT_TYPE CAgent::GetType() {
  return type;
}

char* CAgent::GetTypeName() {

  switch(type) {
    
    case UNEMPLOYED_TYPE:
      return "Unemployed";
      break;

    case MINER_TYPE:
      return "Miner";
      break;

    case BANKER_TYPE:
      return "Banker";
      break;

    case LUMBERJACK_TYPE:
      return "Lumberjack";
      break;

    case HOUSE_TYPE:
      return "House";
      break;

    case MINE_TYPE:
      return "Mine";
      break;

    case BANK_TYPE:
      return "Bank";
      break;

    case SALOON_TYPE:
      return "Saloon";
      break;

    default:
      return "No type found.";
      break;
  }


}

// Setters
void CAgent::SetPosition(int _x, int _y) {
  xPosition = _x;
  yPosition = _y;
}

void CAgent::SetID( int _id ) {
  ID = _id;
}

void CAgent::SetType( AGENT_TYPE _type ) {
  type = _type;
}


// Pathfinding
void CAgent::Clear() {

  open_list.clear();
  final_path.clear();
  closed_list.clear();
  succesor_vector.clear();

}

CNode CAgent::GetClosest(int defined_tile_type, Vector2D position) {

	std::vector<CNode> tile_type;
	std::vector<int> distance;

	CNode _node;
	int temp = 0;
	int index = 0;

	for (int i = 0; i < MAX_Y; i++) {
	
		for (int j = 0; j < MAX_X; j++) {
		
			if (map[i][j] == defined_tile_type) {
			
				_node.position.x = j;
				_node.position.y = i;
				tile_type.push_back(_node);

				temp = sqrt( ((_node.position.x - position.x)^2) + ((_node.position.x - position.y)^2) );
				distance.push_back(temp);

			}

		}
	
	}

	for (int a = 0; a < distance.size(); a++) {
	
		if (distance[a] < index) {
		
			index = a;
		
		}

	}

	return tile_type[index];

}

void CAgent::Pathfinding( Vector2D start_pos, Vector2D finish_pos ) {
	
  Clear();

	CNode _internal_lowest_F;
	_internal_lowest_F.F = 99999999;
	unsigned int _internal_array_position = 0;

	// Initialize the start node
	node_start.G = 0;
	node_start.H = ((finish_pos.x - start_pos.x) + (finish_pos.y - start_pos.y))*10;
	node_start.F = node_start.G + node_start.H;

	node_start.position = start_pos;

	// Initialize the goal node
	node_goal.G = 0;
	node_goal.H = 0;
	node_goal.F = 0;

	node_goal.position = finish_pos;

	// Set the current node at the start
	current_node = node_start;

	// Start of the A* algorithm
	open_list.push_back(node_start);
	

	while(open_list.size() > 0) {

		bool skip_node = false;

		_internal_lowest_F=open_list[0];
		_internal_array_position=0;
		// Lower F and put it in "current_node"
		for(int i = 1; i < open_list.size(); ++i) {
			if(open_list[i].F < _internal_lowest_F.F ) {
				_internal_lowest_F = open_list[i];
				_internal_array_position = i;
			}
		}

		current_node = _internal_lowest_F;
		
    // Withdraws the node from the array
		open_list.erase(open_list.begin() + _internal_array_position );

		// Check if it's the final node
		if(current_node.position.x == node_goal.position.x && 
			current_node.position.y == node_goal.position.y) {
				break;
		}
		
		// Clean the succesor array
		succesor_vector.erase( succesor_vector.begin(), succesor_vector.end() );
		
		// Generate the succesors
		for(int j = 0; j < 8; j++){
			CNode _new_succesor = current_node;
			_new_succesor.id = j;
			
			switch(j){
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

			succesor_vector.push_back(_new_succesor);
		}


		// for each of the succesors
		for(int t = 0; t < 8; t++) {

      if(map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == COLOR_LITE_BLUE ||
         map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == COLOR_SAND ||
         map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == COLOR_GRASS ||
         map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == COLOR_DIRT ||
         map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == BANK_BOT ||
         map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == HOUSE ||
         map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == MINE_BOT_LEFT ||
         map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == MINE_BOT_RIGHT ||
         map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == SALOON_BOT
		) {
			
      // Succesor's positioning
			// 3 2 1
			// 4 # 0
			// 5 6 7

			// Calculate G
			if( t == 0 || t == 2 || t == 4 || t == 6 ) {
				succesor_vector[t].G = current_node.G + 10;
			}
			else if( t == 1 || t == 3 || t == 5 || t == 7 ) {
				succesor_vector[t].G = current_node.G + 14;
			}


			// Search in open list
			
			for( int g = 0; g < open_list.size(); g++ ) {

				// if it's in the open list, compare them and it's equal or better than this, discart this
				if(open_list[g].position.x == succesor_vector[t].position.x &&
					open_list[g].position.y == succesor_vector[t].position.y) {

					if(open_list[g].G <= succesor_vector[t].G) {
						skip_node = true;
					}
          
          else if(open_list[g].G > succesor_vector[t].G){
            open_list.erase( open_list.begin() + g );
          }
          

				}
			}
			// Search in the closed list
			for(int c = 0; c < closed_list.size(); c++) {

				// if it's in the closed list, compare them and it's equal or better than this, discart this
				if(closed_list[c].position.x == succesor_vector[t].position.x &&
					closed_list[c].position.y == succesor_vector[t].position.y) {

						// If it's as good or equal, discard this succesor and continue with the next
						if(closed_list[c].G <= succesor_vector[t].G) {
							skip_node = true;
						}
          
						else if(closed_list[c].G > succesor_vector[t].G) {
							closed_list.erase( closed_list.begin() + c );
						}

					}

			}
			
			// eliminates the node succesor from the open and closed list
			

			// set the parent of the succesor to the current node
			if(skip_node == false) {
				succesor_vector[t].parent_position = current_node.position;
				succesor_vector[t].parent = &current_node;
				
				// Calculate the H and update the F
				int _internal_x = (finish_pos.x - succesor_vector[t].position.x);
				int _internal_y= (finish_pos.y - succesor_vector[t].position.y);
				if(_internal_x < 0) { _internal_x = _internal_x * (-1); }
				if(_internal_y < 0) { _internal_y = _internal_y * (-1); }

				succesor_vector[t].H = ( _internal_x + _internal_y )*10;
				succesor_vector[t].F = succesor_vector[t].G + succesor_vector[t].H;
				
				// Insert the actual node in the open list
				CNode _node_2 = succesor_vector[t];
				open_list.push_back(_node_2);
			}

			skip_node = false;
			}
		}

		// Insert the actual node to the closed list
		closed_list.push_back(current_node);

	}

	// End of the A* algorithm
  GetPath();

}

/*
void CAgent::GetMap() {

  for(int y = 0; y < MAX_Y; y++) {
  
    for(int x = 0; x < MAX_X; x++) {
    
      temp_map[y][x] = map[y][x];

    }

  }

}*/
/*
void CAgent::Pathfinding( Vector2D start_pos, Vector2D finish_pos ) {

  //In the checking of the map it will be a +1, that's because we have
  //  sorrunded the map with a wall, and we don't want to take that in 
  //  the visualizing of the agents at the real map.
  
  //GetMap();
  
  int temp_map[1026][1026];

  for(int i = 0; i < 1026; i++) {
  
    for(int j = 0; j < 1026; j++) {
    
      temp_map[i][j] = map[i][j];

    }

  }

  Clear();

	CNode _internal_lowest_F;
	_internal_lowest_F.F = 99999999;
	unsigned int _internal_array_position = 0;

	// Initialize the start node
	node_start.G = 0;
	node_start.H = ((finish_pos.x - start_pos.x) + (finish_pos.y - start_pos.y))*10;
	node_start.F = node_start.G + node_start.H;

	node_start.position = start_pos;
	temp_map[(int)node_start.position.x+1][(int)node_start.position.y+1] = 'S';

	// Initialize the goal node
	node_goal.G = 0;
	node_goal.H = 0;
	node_goal.F = 0;

	node_goal.position = finish_pos;
	temp_map[(int)node_goal.position.x+1][(int)node_goal.position.y+1] = 'F';

	// Draw
	//Erase_Map();
	//Draw_Map();

	// Set the current node at the start
	current_node = node_start;


	// Start of the A* algorithm
	open_list.push_back(node_start);
	

	while(open_list.size() > 0) {

		bool skip_node = false;

		_internal_lowest_F=open_list[0];
		_internal_array_position=0;
		// Lower F and put it in "current_node"
		for(int i = 1; i < open_list.size(); ++i) {
			if(open_list[i].F < _internal_lowest_F.F ) {
				_internal_lowest_F = open_list[i];
				_internal_array_position = i;
			}
		}

		current_node = _internal_lowest_F;
		
    // Withdraws the node from the array
		open_list.erase(open_list.begin() + _internal_array_position );

		// Check if it's the final node
		if(current_node.position.x == node_goal.position.x && 
			current_node.position.y == node_goal.position.y) {
				break;
		}
		
		// Clean the succesor array
		succesor_vector.erase( succesor_vector.begin(), succesor_vector.end() );
		
		// Generate the succesors
		for(int j = 0; j < 8; j++){
			CNode _new_succesor = current_node;
			_new_succesor.id = j;
			
			switch(j){
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

			succesor_vector.push_back(_new_succesor);
		}


		// for each of the succesors
		for(int t = 0; t < 8; t++) {
      
			// See if you can pass
			if(temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == COLOR_LITE_BLUE ||
         temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == COLOR_SAND ||
         temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == COLOR_GRASS ||
         temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == COLOR_DIRT ||
         temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == BANK_BOT ||
         temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == HOUSE ||
         temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == MINE_BOT_LEFT ||
         temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == MINE_BOT_RIGHT ||
         temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == SALOON_BOT ||
			   temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == 'F' ||
			   temp_map[(int)succesor_vector[t].position.x][(int)succesor_vector[t].position.y] == 'S'
				) {
			
      /*
      if(temp_map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == 'F' ||
			   temp_map[(int)succesor_vector[t].position.x+1][(int)succesor_vector[t].position.y+1] == 'S') {
           */
      // Succesor's positioning
			// 3 2 1
			// 4 # 0
			// 5 6 7

/*
			// Calculate G
			if( t == 0 || t == 2 || t == 4 || t == 6 ) {
				succesor_vector[t].G = current_node.G + 10;
			}
			else if( t == 1 || t == 3 || t == 5 || t == 7 ) {
				succesor_vector[t].G = current_node.G + 14;
			}


			// Search in open list
			
			for( int g = 0; g < open_list.size(); g++ ) {

				// if it's in the open list, compare them and it's equal or better than this, discart this
				if(open_list[g].position.x == succesor_vector[t].position.x &&
					open_list[g].position.y == succesor_vector[t].position.y) {

					if(open_list[g].G <= succesor_vector[t].G) {
						skip_node = true;
					}
          //////////////////////////////////////
          else if(open_list[g].G > succesor_vector[t].G){
            open_list.erase( open_list.begin() + g );
          }
          //////////////////////////////////////

				}
			}
			// Search in the closed list
			for(int c = 0; c < closed_list.size(); c++) {

				// if it's in the closed list, compare them and it's equal or better than this, discart this
				if(closed_list[c].position.x == succesor_vector[t].position.x &&
					closed_list[c].position.y == succesor_vector[t].position.y) {

					// If it's as good or equal, discard this succesor and continue with the next
					if(closed_list[c].G <= succesor_vector[t].G) {
						skip_node = true;
					}
          //////////////////////////////////////
          else if(closed_list[c].G > succesor_vector[t].G){
            closed_list.erase( closed_list.begin() + c );
          }
          //////////////////////////////////////

				}
			}
			
			// eliminates the node succesor from the open and closed list
			

			// set the parent of the succesor to the current node
			if(skip_node == false){
				succesor_vector[t].parent_position = current_node.position;
				
				// Calculate the H and update the F
				int _internal_x = (finish_pos.x - succesor_vector[t].position.x);
				int _internal_y= (finish_pos.y - succesor_vector[t].position.y);
				if(_internal_x < 0) { _internal_x = _internal_x * (-1); }
				if(_internal_y < 0) { _internal_y = _internal_y * (-1); }

				succesor_vector[t].H = ( _internal_x + _internal_y )*10;
				succesor_vector[t].F = succesor_vector[t].G + succesor_vector[t].H;
				
				// Insert the actual node in the open list
				CNode _node_2 = succesor_vector[t];
				open_list.push_back(_node_2);
			}

			skip_node = false;
			}
		}

		// Insert the actual node to the closed list
		closed_list.push_back(current_node);

		//Show's the nodes of the closed list
		temp_map[(int)current_node.position.x+1][(int)current_node.position.y+1] = 176; // '+';
	}

	// End of the A* algorithm


////////////////////////////////////////
	// Show the path in a diferent color at the log's map
	GetPath();

	// Show the Start in the log's map
	temp_map[(int)node_start.position.x+1][(int)node_start.position.y+1] = 'S';
  
////////////////////////////////////////	

}
*/

// AI Functions


void CAgent::GetPath() {

  if(closed_list.size() > 0) {
    
    // The last node is NOT inserted in the closed list,
    //  so that is why it is the first node to get in.
    final_path.push_back(current_node);

    CNode _node = closed_list[ closed_list.size()-1 ];
    
    final_path.push_back(_node);

    while( ( (_node.parent_position.x != node_start.position.x) ||
             (_node.parent_position.y != node_start.position.y) ) ) { 

      for(int i = 0; i < closed_list.size()-1; i++) {
        if(closed_list[i].position.x == _node.parent_position.x && closed_list[i].position.y == _node.parent_position.y) {
        
		      _node = closed_list[i];
        
		      final_path.push_back(_node);

	      }

      }

    }

  }

}

void CAgent::FSM_Working() {

  if( jobPosition.x >= 0 ) {

    if(isFirstTime) {
  
      inside_building = false;
      isFirstTime = false;

      internal_timer = 0;
      internal_count = 0;

      Vector2D actual_position;
      actual_position.x = xPosition;
      actual_position.y = yPosition;
	  
	  CNode temp = GetClosest(MINE_BOT_LEFT, housePosition);

	  Vector2D _temp;

	  _temp.x = temp.position.x;
	  _temp.y = temp.position.y;
	  
	  Pathfinding(actual_position, _temp);

      if(closed_list.size() > 0) {
        printf(" - Agent %d: Going to work.\n", ID + 1);
        internal_count = closed_list.size();
      }
      else if(closed_list.size() <= 0) {
        printf(" - Agent %d: Already at work.\n", ID + 1);
        internal_count = AT_DESTINATION;
        inside_building = true;
      }


    }
    else if(internal_count > 0 && internal_count < AT_DESTINATION) {
  
      internal_timer++;

      if(internal_timer >= walking_speed) {

        inside_building = false;
        internal_timer -= walking_speed;

        xPosition = final_path[internal_count - 1].position.x;
        yPosition = final_path[internal_count - 1].position.y;
      
        internal_count--;

        if(internal_count <= 0) {
          internal_count = AT_DESTINATION;
          internal_timer = 0;
          inside_building = true;
          printf(" - Agent %d: Arrived to work.\n", ID + 1);
        }

      }

    }
    else if(internal_count == AT_DESTINATION) {
    
      internal_timer++;

      if(internal_timer >= working_time_cost) {
    
        internal_count = 0;
        internal_timer = 0;
        inside_building = true;

        int gold_gained = rand()%2 + 1;
        bag += gold_gained;

        printf(" - Agent %d: Gained %d gold!!, carrying: %d gold.\n", ID + 1, gold_gained, bag);
            
        internal_count = JOB_FINISHED;
      }

    }
    else if(internal_count == JOB_FINISHED) {
  
		  thirst++;
		  hunger++;
		  tired++;

		  // Next action
		  if (thirst >= 6) {
			  _state = STATE_DRINKING;
        printf(" - Agent %d: New state: STATE_DRINKING\n", ID + 1);
		  }
		  else if (hunger >= 8) {
			  _state = STATE_EATING;
        printf(" - Agent %d: New state: STATE_EATING\n", ID + 1);
		  }
		  else if (tired >= 8) {
			  _state = STATE_RESTING;
        printf(" - Agent %d: New state: STATE_RESTING\n", ID + 1);
		  }
		  else if (bag >= 5) {
			  _state = STATE_DEPOSITING_MONEY;
        printf(" - Agent %d: New state: STATE_DEPOSITING_MONEY\n", ID + 1);
		  }

		  // If no new action is selected, it will mantain it's current one.
		  isFirstTime = true;
      inside_building = false;

    }

  }
  else if(jobPosition.x < 0) {

    if( SetJob() == true) {
      printf(" - Agent %d: Job found!\n", ID + 1);
      
    }

  }

}

void CAgent::FSM_Eating() {
  
    if(isFirstTime) {
  
      isFirstTime = false;
	  inside_building = false;

      internal_timer = 0;
      internal_count = 0;

      Vector2D actual_position;
      actual_position.x = xPosition;
      actual_position.y = yPosition;
      
      Pathfinding(actual_position, housePosition);

      if(closed_list.size() > 0) {
        printf(" - Agent %d: Going home to eat.\n", ID + 1);
        internal_count = closed_list.size();
      }
      else if(closed_list.size() <= 0) {
        printf(" - Agent %d: Already at home.\n", ID + 1);
        internal_count = AT_DESTINATION;
		inside_building = true;
      }


    }
    else if(internal_count > 0 && internal_count < AT_DESTINATION) {
  
      internal_timer++;

      if(internal_timer >= walking_speed) {
		
		inside_building = false;
		internal_timer -= walking_speed;

        xPosition = final_path[internal_count - 1].position.x;
        yPosition = final_path[internal_count - 1].position.y;
      
        internal_count--;

        if(internal_count <= 0) {
          internal_count = AT_DESTINATION;
          internal_timer = 0;
          printf(" - Agent %d: Time to eat!.\n", ID + 1);
		  inside_building = true;
        }

      }

    }
    else if(internal_count == AT_DESTINATION) {
    
      internal_timer++;
	  

      if(internal_timer >= eating_time_cost) {
    
        internal_count = 0;
        internal_timer = 0;
		inside_building = true;
        
        printf(" - Agent %d: I'm full.\n", ID + 1);
            
        internal_count = JOB_FINISHED;
      }

    }
    else if(internal_count == JOB_FINISHED) {
  
		  thirst++;
		  hunger = 0;
		  tired++;

		  // Next action
      _state = STATE_WORKING;

		  if (thirst >= 6) {
			  _state = STATE_DRINKING;
        printf(" - Agent %d: New state: STATE_DRINKING\n", ID + 1);
		  }
		  else if (tired >= 8) {
			  _state = STATE_RESTING;
        printf(" - Agent %d: New state: STATE_RESTING\n", ID + 1);
		  }
		  else if (bag >= 5) {
			  _state = STATE_DEPOSITING_MONEY;
        printf(" - Agent %d: New state: STATE_DEPOSITING_MONEY\n", ID + 1);
		  }

		  // If no new action is selected, it will mantain it's current one.
		  isFirstTime = true;
		  inside_building = false;

    }

}

void CAgent::FSM_Drinking() {

  if(saloonPosition.x >= 0) {
	
	  if(isFirstTime) {
  
        isFirstTime = false;
        inside_building = false;

        internal_timer = 0;
        internal_count = 0;

        Vector2D actual_position;
        actual_position.x = xPosition;
        actual_position.y = yPosition;

        Pathfinding(actual_position, saloonPosition);

        if(closed_list.size() > 0) {
          printf(" - Agent %d: Going to the saloon.\n", ID + 1);
          internal_count = closed_list.size();
        }
        else if(closed_list.size() <= 0) {
          printf(" - Agent %d: Already at the saloon.\n", ID + 1);
		  internal_count = AT_DESTINATION;
		  inside_building = true;
        }


      }
      else if(internal_count > 0 && internal_count < AT_DESTINATION) {
  
        internal_timer++;

        if(internal_timer >= walking_speed) {

          inside_building = false;
          internal_timer -= walking_speed;

          xPosition = final_path[internal_count - 1].position.x;
          yPosition = final_path[internal_count - 1].position.y;
      
          internal_count--;

          if(internal_count <= 0) {
            internal_count = AT_DESTINATION;
            internal_timer = 0;
            inside_building = true;
            printf(" - Agent %d: Time to drink!.\n", ID + 1);
          }

        }

      }
      else if(internal_count == AT_DESTINATION) {
    
        internal_timer++;

        if(internal_timer >= drinking_time_cost) {
    
          internal_count = 0;
          internal_timer = 0;
          inside_building = true;
        
          printf(" - Agent %d: I'm feeling a little dizzy... HIP!.\n", ID + 1);
            
          internal_count = JOB_FINISHED;
        }

      }
      else if(internal_count == JOB_FINISHED) {
  
		    thirst = 0;
		    hunger++;
		    tired++;

		    // Next action
        _state = STATE_WORKING;

		    if (thirst >= 6) {
			    _state = STATE_DRINKING;
          printf(" - Agent %d: New state: STATE_DRINKING\n", ID + 1);
		    }
		    else if (tired >= 8) {
			    _state = STATE_RESTING;
          printf(" - Agent %d: New state: STATE_RESTING\n", ID + 1);
		    }
		    else if (bag >= 5) {
			    _state = STATE_DEPOSITING_MONEY;
          printf(" - Agent %d: New state: STATE_DEPOSITING_MONEY\n", ID + 1);
		    }

		    // If no new action is selected, it will mantain it's current one.
		    isFirstTime = true;
        inside_building = false;

      }
  }
  else if(saloonPosition.x < 0) {

    if( SetSaloon() == true) {
      printf(" - Agent %d: Saloon found!\n", ID + 1);
      
    }

  }

}

void CAgent::FSM_Resting() {
	
	if(isFirstTime) {
  
      isFirstTime = false;
	  inside_building = false;

      internal_timer = 0;
      internal_count = 0;

      Vector2D actual_position;
      actual_position.x = xPosition;
      actual_position.y = yPosition;

      Pathfinding(actual_position, housePosition);

      if(closed_list.size() > 0) {
        printf(" - Agent %d: Going home.\n", ID + 1);
        internal_count = closed_list.size();
      }
      else if(closed_list.size() <= 0) {
        printf(" - Agent %d: Already at home.\n", ID + 1);
        internal_count = AT_DESTINATION;
		inside_building = true;
      }


    }
    else if(internal_count > 0 && internal_count < AT_DESTINATION) {
  
      internal_timer++;

      if(internal_timer >= walking_speed) {

        internal_timer -= walking_speed;
		inside_building = false;

        xPosition = final_path[internal_count - 1].position.x;
        yPosition = final_path[internal_count - 1].position.y;
      
        internal_count--;

        if(internal_count <= 0) {
          internal_count = AT_DESTINATION;
          internal_timer = 0;
          printf(" - Agent %d: ZzZzZzZz...\n", ID + 1);
		  inside_building = true;
        }

      }

    }
    else if(internal_count == AT_DESTINATION) {
    
      internal_timer++;

      if(internal_timer >= resting_time_cost) {
    
        internal_count = 0;
        internal_timer = 0;
		inside_building = true;
        
        printf(" - Agent %d: Time to wake up...\n", ID + 1);
            
        internal_count = JOB_FINISHED;
      }

    }
    else if(internal_count == JOB_FINISHED) {
  
		  thirst++;
		  hunger++;
		  tired = 0;

		  // Next action
      _state = STATE_WORKING;

		  if (thirst >= 6) {
			  _state = STATE_DRINKING;
        printf(" - Agent %d: New state: STATE_DRINKING\n", ID + 1);
		  }
		  else if (hunger >= 8) {
			  _state = STATE_EATING;
        printf(" - Agent %d: New state: STATE_EATING\n", ID + 1);
		  }
		  else if (tired >= 8) {
			  _state = STATE_RESTING;
        printf(" - Agent %d: New state: STATE_RESTING\n", ID + 1);
		  }
		  else if (bag >= 5) {
			  _state = STATE_DEPOSITING_MONEY;
        printf(" - Agent %d: New state: STATE_DEPOSITING_MONEY\n", ID + 1);
		  }

		  // If no new action is selected, it will mantain it's current one.
		  isFirstTime = true;
		  inside_building = false;

    }

}

void CAgent::FSM_DepositingMoney() {


	if (bankPosition.x > 0) {

		if (isFirstTime) {

			isFirstTime = false;
			inside_building = false;

			internal_timer = 0;
			internal_count = 0;

			Vector2D actual_position;
			actual_position.x = xPosition;
			actual_position.y = yPosition;

			Pathfinding(actual_position, bankPosition);

			if (closed_list.size() > 0) {
				printf(" - Agent %d: Going to the bank.\n", ID + 1);
				internal_count = closed_list.size();
			}
			else if (closed_list.size() <= 0) {
				printf(" - Agent %d: Already at the bank.\n", ID + 1);
				internal_count = AT_DESTINATION;
				inside_building = true;
			}


		}
		else if (internal_count > 0 && internal_count < AT_DESTINATION) {

			internal_timer++;

			if (internal_timer >= walking_speed) {

				internal_timer -= walking_speed;
				inside_building = false;

				xPosition = final_path[internal_count - 1].position.x;
				yPosition = final_path[internal_count - 1].position.y;

				internal_count--;

				if (internal_count <= 0) {
					internal_count = AT_DESTINATION;
					internal_timer = 0;
					printf(" - Agent %d: Time deposit the gold.\n", ID + 1);
					inside_building = true;
				}

			}

		}
		else if (internal_count == AT_DESTINATION) {

			internal_timer++;

			if (internal_timer >= resting_time_cost) {

				internal_count = 0;
				internal_timer = 0;
				inside_building = true;

				bank_acount += bag;
				bag = 0;

				printf(" - Agent %d: Total amount: %d\n", ID + 1, bank_acount);

				internal_count = JOB_FINISHED;
			}

		}
		else if (internal_count == JOB_FINISHED) {

			thirst++;
			hunger++;
			tired = 0;

			// Next action
			_state = STATE_WORKING;

			if (thirst >= 6) {
				_state = STATE_DRINKING;
				printf(" - Agent %d: New state: STATE_DRINKING\n", ID + 1);
			}
			else if (hunger >= 8) {
				_state = STATE_EATING;
				printf(" - Agent %d: New state: STATE_EATING\n", ID + 1);
			}
			else if (tired >= 8) {
				_state = STATE_RESTING;
				printf(" - Agent %d: New state: STATE_RESTING\n", ID + 1);
			}

			// If no new action is selected, it will mantain it's current one.
			isFirstTime = true;
			inside_building = false;

		}

	}
	else if (bankPosition.x < 0) {

		if (SetBank() == true) {
			printf(" - Agent %d: Bank found!\n", ID + 1);

		}

	}

}

void CAgent::ChangeJob( AGENT_TYPE new_job ) {

  switch( new_job ) {

    case UNEMPLOYED_TYPE:
      type = UNEMPLOYED_TYPE;
      image = unemployed_texture;

      break;

    case MINER_TYPE:
      type = MINER_TYPE;
      image = miner_texture;
      break;

    case BANKER_TYPE:
      type = BANKER_TYPE;
      image = banker_texture;
      break;

    case LUMBERJACK_TYPE:
      type = LUMBERJACK_TYPE;
      image = lumberjack_texture;
      break;
  
  }

}