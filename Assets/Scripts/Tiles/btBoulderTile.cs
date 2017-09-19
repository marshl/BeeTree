using UnityEngine;
using System.Collections;

/// <summary>
/// A tile which can be moved with the Push node, and is used in conjunction with switch tiles to change toggle tiles
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Boulder")]
public class btBoulderTile : btTile
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
