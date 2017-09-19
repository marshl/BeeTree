using UnityEngine;
using System.Collections;

/// <summary>
/// Tests to see if the player can turn to the left
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Test Nodes/Left Test")]
public class btFunctionLeftTestNode : btFunctionTestNode
{
	public override NODE_STATE TreeExecute()
	{
		btPlayerTile player = btTileManager.instance.playerTile;
		btTile.DIRECTION playerLeft = btTileManager.DirectionTurnedLeft( player.direction );
		btBoardSpace leftSpace = btTileManager.instance.GetSpaceInDirectionFromTile( player, playerLeft, 1 );
		
		if ( leftSpace == null || leftSpace.IsPassable() == false )
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;
		}
		
		this.currentState = btFunctionNode.NODE_STATE.SUCCEEDED;
		return this.currentState;
	}
}
