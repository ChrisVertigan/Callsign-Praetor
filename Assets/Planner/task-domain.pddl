;; domain file: task-domain.pddl

(define (domain task)
	(:requirements :strips :adl :fluents)
	
	(:predicates 
				 (isperson ?npc)
				 (islabourer ?npc)
				 (iscarpenter ?npc)
				 (isblacksmith ?npc)
				 (isteacher ?npc)
				 (isminer ?npc)
				 (istrader ?npc)
				 (islumberjack ?npc)
				 (isrifleman ?npc)
				 (hasskill ?npc)
				 (nearbyforest ?npc)
				 (nearbystone ?npc)
				 (nearbyore ?npc)
				 (nearbycoal ?npc)
				 (isdead ?npc))

	(:functions
 		(stone)
		(coal)
		(ore)
		(iron)
		(timber)
		(wood)
		(money)
		(axes)
		(carts)
		(rifles)
		(huts)
		(houses)
		(schools)
		(barracks)
		(storages)
		(mines)
		(smelters)
		(quarries)
		(sawmills)
		(blacksmiths)
		(markets)
		(people)
 	)

			
	;BUILDINGS

	 
	(:action buildhut
			 :parameters (?npc)
			 :precondition (and  (islabourer ?npc) (not (isdead ?npc)))
			 :effect (and (increase (huts) 1) ))
			 
	(:action buildhouse
			 :parameters (?npca ?npcb)
			 :precondition (and  (islabourer ?npca) 
									(iscarpenter ?npcb) (not (isdead ?npca)) (not (isdead ?npcb)) 
						   (> (stone) 0) (> (wood) 0))
			 :effect (and (increase (houses) 1)  ))
			 
	(:action buildschool
			 :parameters (?npca ?npcb)
			 :precondition (and  (islabourer ?npca) 
									(iscarpenter ?npcb) (not (isdead ?npca)) (not (isdead ?npcb))
						   (> (stone) 0) (> (wood) 0) (> (iron) 0))
			 :effect (and (increase (schools) 1)  ))
			 
	(:action buildbarracks
			 :parameters (?npca ?npcb)
			 :precondition (and  (islabourer ?npca) 
									(iscarpenter ?npcb) (not (isdead ?npca)) (not (isdead ?npcb))
						   (> (stone) 0) (> (wood) 0))
			 :effect (and (increase (barracks) 1)  ))
			 
	(:action buildstorage
			 :parameters (?npca ?npcb)
			 :precondition (and  (islabourer ?npca) 
									(iscarpenter ?npcb) (not (isdead ?npca)) (not (isdead ?npcb))
						   (> (stone) 0) (> (wood) 0))
			 :effect (and (increase (storages) 1)  ))

	(:action buildmine
			 :parameters (?npca ?npcb ?npcc)
			 :precondition (and  (islabourer ?npca) 
									(iscarpenter ?npcb) (isblacksmith ?npcc) (not (isdead ?npca)) (not (isdead ?npcb)) (not (isdead ?npcc))
							   (> (iron) 0) (> (wood) 0))
			 :effect (and (increase (mines) 1)   ))

	(:action buildsmelter
			 :parameters (?npc)
			 :precondition (and  (islabourer ?npc) (> (stone) 0) (not (isdead ?npc)))
			 :effect (and (increase (smelters) 1)))

	(:action buildquarry
			 :parameters (?npc)
			 :precondition (and  (islabourer ?npc) (not (isdead ?npc)))
			 :effect (and (increase (quarries) 1) ))
	
	(:action buildsawmill
			 :parameters (?npc)
			 :precondition (and  (islabourer ?npc) (not (isdead ?npc))
						(> (iron) 0) (> (stone) 0) (> (timber) 0))
			 :effect (and (increase (sawmills) 1) ))
			 
	(:action buildblacksmith
			 :parameters (?npc)
			 :precondition (and (islabourer ?npc) (not (isdead ?npc))
						(> (iron) 0) (> (stone) 0) (> (timber) 0))
			 :effect (and (increase (blacksmiths) 1) ))

	(:action buildmarket
			 :parameters (?npc)
			 :precondition (and  (iscarpenter ?npc) (not (isdead ?npc))
						(> (wood) 0))
			 :effect (and (increase (markets) 1) ))

	;ACTIONS	


	(:action movetoforest
			 :parameters (?npca)
			 :precondition (and  (not (nearbyforest ?npca)) (not (isdead ?npca)))
			 :effect (and (nearbyforest ?npca)))
	
	(:action movetostone
			 :parameters (?npca)
			 :precondition (and  (not (nearbystone ?npca)) (not (isdead ?npca)))
			 :effect (and (nearbystone ?npca)))
			 
	(:action movetocoal
			 :parameters (?npca)
			 :precondition (and  (not (nearbycoal ?npca)) (not (isdead ?npca)))
			 :effect (and (nearbycoal ?npca)))
			 
	(:action movetoore
			 :parameters (?npca)
			 :precondition (and  (not (nearbyore ?npca)) (not (isdead ?npca)))
			 :effect (and (nearbyore ?npca)))
	
	(:action familyhut
			 :parameters (?npca ?npcb)
			 :precondition (and   (> (huts) 0) (not (isdead ?npca)) (not (isdead ?npcb)))
			 :effect (and (increase (people) 1)  ))

	(:action familyhouse
			 :parameters (?npca ?npcb)
			 :precondition (and   (> (houses) 0) (not (isdead ?npca)) (not (isdead ?npcb)))
			 :effect (and (increase (people) 2)  ))
			 
	(:action educate
			 :parameters (?npca ?npcb)
			 :precondition (and (not (isdead ?npca)) (not (isdead ?npcb)))
			 :effect (and (hasskill ?npcb)  ))
			 
			 
	(:action educatebarracks
			 :parameters (?npca ?npcb)
			 :precondition (and (isteacher ?npca) (> (barracks) 0) (> (rifles) 0) (not (isdead ?npca)) (not (isdead ?npcb)))
			 :effect (and (isrifleman ?npcb)  ))

	(:action train
			 :parameters (?npca ?npcb)
			 :precondition (and   (isteacher ?npca) (> (schools) 0) (not (isdead ?npca)) (not (isdead ?npcb)))
			 :effect (and (hasskill ?npcb)  ))
			 
	(:action cuttree
			 :parameters (?npca)
			 :precondition (and  (nearbyforest ?npca) (not (isdead ?npca)))
			 :effect (and (increase (timber) 1) ))
			 
	(:action minecoal
			 :parameters (?npca)
			 :precondition (and  (nearbycoal ?npca) (not (isdead ?npca)))
			 :effect (and (increase (coal) 1) ))
			 
	(:action mineore
			 :parameters (?npca)
			 :precondition (and  (nearbyore ?npca) (not (isdead ?npca)))
			 :effect (and (increase (ore) 1) ))
			 
	(:action minestone
			 :parameters (?npca)
			 :precondition (and  (nearbystone ?npca) (not (isdead ?npca)))
			 :effect (and (increase (stone) 1) ))
			 
	(:action storecoal
			 :precondition (and (> (coal) 0) (> (storages) 0))
			 :effect (and (decrease (coal) 1) (increase (money) 1)))
			 
	(:action storeore
			 :precondition (and (> (ore) 0) (> (storages) 0))
			 :effect (and (decrease (ore) 1) (increase (money) 1)))
			 
	(:action storeiron
			 :precondition (and (> (iron) 0) (> (storages) 0))
			 :effect (and (decrease (iron) 1) (increase (money) 1)))
			 
	(:action storetimber
			 :precondition (and (> (timber) 0) (> (storages) 0))
			 :effect (and (decrease (timber) 1) (increase (money) 1)))
			 
	(:action storewood
			 :precondition (and (> (wood) 0) (> (storages) 0))
			 :effect (and (decrease (wood) 1) (increase (money) 1)))
			 
	(:action storestone
			 :precondition (and (> (stone) 0) (> (storages) 0))
			 :effect (and (decrease (stone) 1) (increase (money) 1)))
			 
	(:action smelt
			 :parameters (?npca)
			 :precondition (and (islabourer ?npca) (> (ore) 0) (> (coal) 0) (> (smelters) 0) (not (isdead ?npca)))
			 :effect (and (decrease (ore) 1) (decrease (coal) 1) (increase (iron) 1) ))
			 
	(:action quarry
			 :parameters (?npca)
			 :precondition (and (islabourer ?npca) (> (quarries) 0) (not (isdead ?npca)))
			 :effect (and (increase (stone) 1) ))
			 
	(:action saw
			 :parameters (?npca)
			 :precondition (and (islabourer ?npca) (> (timber) 0) (> (sawmills) 0) (not (isdead ?npca)))
			 :effect (and (decrease (timber) 1) (increase (wood) 1) ))
			 
	(:action makeaxe
			 :parameters (?npca)
			 :precondition (and (isblacksmith ?npca) (> (blacksmiths) 0) (not (isdead ?npca)))
			 :effect (and (increase (axes) 1) ))
			 
	(:action makecart
			 :parameters (?npca)
			 :precondition (and (isblacksmith ?npca) (> (blacksmiths) 0) (not (isdead ?npca)))
			 :effect (and (increase (carts) 1) ))
			 
	(:action makerifle
			 :parameters (?npca)
			 :precondition (and (isblacksmith ?npca) (> (blacksmiths) 0) (not (isdead ?npca)))
			 :effect (and (increase (rifles) 1) ))
			 
	;(:action buystone
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (money) 0 ) (not (isdead ?npca)))
	;		 :effect (and (decrease (money) 1) (increase (stone) 1)))
			 
	;(:action buycoal
		;	 :parameters (?npca)
		;	 :precondition (and (istrader ?npca) (> (money) 0) (not (isdead ?npca)))
		;	 :effect (and (decrease (money) 1) (increase (coal) 1)))
			 
	;(:action buyore
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (money) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (money) 1) (increase (ore) 1)))
			 
	;(:action buyiron
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (money) 2) (not (isdead ?npca)))
	;		 :effect (and (decrease (money) 3) (increase (iron) 1)))
			 
	;(:action buywood
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (money) 2) (not (isdead ?npca)))
	;		 :effect (and (decrease (money) 3) (increase (wood) 1)))
			 
	;(:action buytimber
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (money) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (money) 1) (increase (timber) 1)))
			 
	;(:action buyaxe
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (money) 2) (not (isdead ?npca)))
	;		 :effect (and (decrease (money) 3) (increase (axes) 1)))
			 
	;(:action buyrifle
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (money) 4) (not (isdead ?npca)))
	;		 :effect (and (decrease (money) 5) (increase (rifles) 1)))
			 
	;(:action buycart
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (money) 4) (not (isdead ?npca)))
	;		 :effect (and (decrease (money) 5) (increase (carts) 1)))
			 
	;(:action sellstone
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (stone) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (stone) 1) (increase (money) 1)))
			 
	;(:action sellcoal
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (coal) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (coal) 1) (increase (money) 1)))
			 
	;(:action sellore
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (ore) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (ore) 1) (increase (money) 1)))
			 
	;(:action selliron
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (iron) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (iron) 1) (increase (money) 3)))
			 
	;(:action sellwood
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (wood) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (wood) 1) (increase (money) 3)))
			 
	;(:action selltimber
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (timber) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (timber) 1) (increase (money) 1)))
			 
	;(:action sellaxe
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (axes) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (axes) 1) (increase (money) 3)))
			 
	;(:action sellrifle
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (rifles) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (rifles) 1) (increase (money) 5)))
			 
	;(:action sellcart
	;		 :parameters (?npca)
	;		 :precondition (and (istrader ?npca) (> (carts) 0) (not (isdead ?npca)))
	;		 :effect (and (decrease (carts) 1) (increase (money) 5)))
			 
	(:action combat
			 :parameters (?npca ?npcb)
			 :precondition (and (isrifleman ?npca) (> (rifles) 0) (not (isdead ?npca)) (not (isdead ?npcb)))
			 :effect (and (decrease (people) 1) (isdead ?npcb)))
			 
	)

			 
			 
				 