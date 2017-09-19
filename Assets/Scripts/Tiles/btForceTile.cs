using UnityEngine;
using System.Collections;

/// <summary>
/// A tile which moves any tile that is paced upon it in the direction that this tile is facing
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Force")]
public class btForceTile : btTile
{
	public override void OnMoveableTileEnter( btTile _tile, DIRECTION _enterDirection )
	{
		btBoardSpace moveToSpace = btTileManager.instance.GetSpaceInDirectionFromTile( this, this.direction, 1 );
		if ( moveToSpace == null || moveToSpace.IsPassable() == false )
		{
			return;	
		}
		_tile.tileState = btTile.TILESTATE.FORCE_MOVE;
		_tile.forceDirection = this.direction;
		_tile.currentMovement = 0.0f;
		_tile.startSpace = this.currentSpace;
		_tile.endSpace = moveToSpace;
	}
}
