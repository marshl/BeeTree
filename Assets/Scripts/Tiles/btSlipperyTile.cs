using UnityEngine;
using System.Collections;

/// <summary>
/// A tile upon which the player tile has no control (only useful when combined with force tiles)
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Slippery")]
public class btSlipperyTile : btTile
{
	public override void OnMoveableTileEnter( btTile _tile, DIRECTION _enterDirection )
	{
		DIRECTION slideDirection = _enterDirection;
		btBoardSpace adjacentSpace = btTileManager.instance.GetSpaceInDirectionFromTile( this, slideDirection, 1 );
		if ( adjacentSpace == null || adjacentSpace.IsPassable() == false )
		{
			return;	
		}
		_tile.tileState = btTile.TILESTATE.SLIDING;
		_tile.slideDirection = slideDirection;
		_tile.startSpace = this.currentSpace;
		_tile.endSpace = adjacentSpace;
		_tile.currentMovement = 0.0f;
	}
}
