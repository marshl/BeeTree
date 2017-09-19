using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Succeeds if there are no objects in front of the player "variable" number of times
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Tests/Sight Test")]
public class btFunctionBoulderTestNode : btFunctionTestNode
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
		
		this.currentState = btFunctionNode.NODE_STATE.SUCCEEDED;
		return this.currentState;
	}
}
