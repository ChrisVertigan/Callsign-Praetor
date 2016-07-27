#include "Node.h"

CNode::CNode() {
	F = 0;
	G = 0;
	H = 0;

	position.x = 0;
	position.y = 0;

	parent_position.x = 0;
	parent_position.y = 0;

    parent = nullptr;

	id = 0;
}

CNode::~CNode() {

}