using UnityEngine;
using System.Collections;

/// <summary>
/// The player tile
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Player Tile")]
public class btPlayerTile : btTile 
{
	public override bool IsPassable()
	{
		return false;	
	}
	
	public override int GetTilePriority()
	{
		return 1;
	}
}
