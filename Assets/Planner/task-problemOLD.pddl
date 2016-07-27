;; problem file: task-problem.pddl

(define (problem task-problem)
	(:domain task)
	(:objects npca npcb npcc npcd)
	(:init (islabourer npca) (iscarpenter npcb) (isblacksmith npcc) (isrifleman npcd)
										(= (stone) 0) (= (coal) 0) (= (ore) 0) (= (iron) 0) (= (timber) 0) (= (wood) 0) (= (money) 0) (= (axes) 0) (= (carts) 0)
										 (= (rifles) 0) (= (huts) 0) (= (houses) 0) (= (schools) 0) (= (barracks) 0) (= (storages) 0) (= (mines) 0) (= (smelters) 0)
										 (= (quarries) 0) (= (sawmills) 0) (= (blacksmiths) 0) (= (markets) 0) (= (people) 4))
	(:goal (> (money) 29)))