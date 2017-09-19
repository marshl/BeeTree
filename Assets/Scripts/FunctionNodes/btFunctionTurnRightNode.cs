using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Turns the player right and moves them forward one space if possible
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Actions/Turn Right")]
public class btFunctionTurnRightNode : btFunctionActionNode
{
	public override NODE_STATE TreeExecute()
	{
		btPlayerTile player = btTileManager.instance.playerTile;
		btTile.DIRECTION playerRight = btTileManager.DirectionTurnedRight( player.direction );
		btBoardSpace space = btTileManager.instance.GetSpaceInDirectionFromTile( player, playerRight );
		
		if ( space == null || space.IsPassable() == false )
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;
		}
		
		player.isTurning = true;
		player.tileState = btTile.TILESTATE.TURNING_RIGHT;
		player.startingRotation = player.GetRotation();
		player.startSpace = player.currentSpace;
		player.endSpace = space;
		player.currentMovement = 0.0f;
		
		this.currentState = btFunctionNode.NODE_STATE.RUNNING;
		return this.currentState;
	}
}
