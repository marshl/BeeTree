using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Moves the player forward by one space if possible
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Action/Move Forward")]
public class btFunctionJumpNode : btFunctionActionNode
{
	public override NODE_STATE TreeExecute()
	{
		btPlayerTile player = btTileManager.instance.playerTile;
		btTile.DIRECTION playerForward = player.direction;
		btBoardSpace space1 = btTileManager.instance.GetSpaceInDirectionFromTile( player, playerForward );
		if ( space1 == null || space1.IsPassable() == true )
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;
		}
		btBoardSpace space2 = btTileManager.instance.GetSpaceInDirectionFromTile( player, playerForward, 2 );
		if ( space2 == null || space2.IsPassable() == false )
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;
		}
		
		player.startSpace = player.currentSpace;//btTileManager.instance.GetSpace( player.xIndex, player.yIndex );
		player.endSpace = space2;
		player.tileState = btTile.TILESTATE.JUMPING;
		
		this.currentState = btFunctionNode.NODE_STATE.RUNNING;
		return this.currentState;
	}
}
