using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Moves the player forward by one space if possible
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Action/Move Forward")]
public class btFunctionMoveForwardNode : btFunctionActionNode
{
	public override NODE_STATE TreeExecute()
	{
		btPlayerTile player = btTileManager.instance.playerTile;
		btTile.DIRECTION playerForward = player.direction;
		btBoardSpace space = btTileManager.instance.GetSpaceInDirectionFromTile( player, playerForward );
		if ( space == null || space.IsPassable() == false )
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;
		}
		player.tileState = btTile.TILESTATE.MOVING_FORWARD;
		player.startSpace = player.currentSpace;
		player.endSpace = space;
		player.currentMovement = 0.0f;
		
		this.currentState = btFunctionNode.NODE_STATE.RUNNING;
		return this.currentState;
	}
}
