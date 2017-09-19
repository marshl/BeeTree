using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Succeeds if there are no objects in front of the player "variable" number of times
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Tests/Sight Test")]
public class btFunctionSightTestNode : btFunctionTestNode
{
	public override NODE_STATE TreeExecute()
	{
		btPlayerTile player = btTileManager.instance.playerTile;
		btTile.DIRECTION forward = player.direction;
		
		for ( int distance = 1; distance <= this.variable; ++distance )
		{
			btBoardSpace space = btTileManager.instance.GetSpaceInDirectionFromTile( player, forward, distance );
			if ( space == null
			  || space.IsPassable() == false )
			{
				this.currentState = btFunctionNode.NODE_STATE.FAILED;
				return this.currentState;
			}
		}
		
		this.currentState = btFunctionNode.NODE_STATE.SUCCEEDED;
		return this.currentState;
	}
}
