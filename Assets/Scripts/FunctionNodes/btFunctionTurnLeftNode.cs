using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Turns the player left and moves them forward one space if possible
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Actions/Turn Left")]
public class btFunctionTurnLeftNode : btFunctionActionNode
{
	public override NODE_STATE TreeExecute()
	{
		btPlayerTile player = btTileManager.instance.playerTile;
		btTile.DIRECTION playerLeft = btTileManager.DirectionTurnedLeft( player.direction );
		btBoardSpace space = btTileManager.instance.GetSpaceInDirectionFromTile( player, playerLeft, 1 );
		
		if ( space == null || space.IsPassable() == false )
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;
		}
		
		player.isTurning = true;
		player.tileState = btTile.TILESTATE.TURNING_LEFT;
		player.startingRotation = player.GetRotation();
		player.startSpace = player.currentSpace;
		player.endSpace = space;
		player.currentMovement = 0.0f;
	
		this.currentState = btFunctionNode.NODE_STATE.RUNNING;
		return this.currentState;
	}
}
