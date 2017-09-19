using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Pushes any boulders in front of the player forward by one space
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Actions/Push")]
public class btFunctionPushNode : btFunctionActionNode
{
	
	public override NODE_STATE TreeExecute()
	{
		btPlayerTile player = btTileManager.instance.playerTile;
		btTile.DIRECTION playerForward = player.direction;
		btBoardSpace space1 = btTileManager.instance.GetSpaceInDirectionFromTile( player, playerForward, 1 );
		if ( space1 == null ) // Fail if there is no tile in front of the player
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;	
		}

        List<btBoulderTile> boulders = space1.GetTilesOfType<btBoulderTile>();
        if (boulders.Count == 0) // Fail if there is no boulder in front of the player
        {
            this.currentState = btFunctionNode.NODE_STATE.FAILED;
            return this.currentState;
        }
		
		btBoardSpace space2 = btTileManager.instance.GetSpaceInDirectionFromTile( player, playerForward, 2 );
		if ( space2 == null || space2.IsPassable() == false ) // Fail if there is no tile in front of the boulder or it is impassable
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;	
		}
		
		btBoulderTile boulder = boulders[0];
		boulder.tileState = btTile.TILESTATE.FORCE_MOVE;
		boulder.forceDirection = player.direction;
		boulder.currentMovement = 0.0f;
		boulder.startSpace = boulder.currentSpace;
		boulder.endSpace = space2;
		
		this.currentState = btFunctionNode.NODE_STATE.RUNNING;
		return this.currentState;
	}
}
