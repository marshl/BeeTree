using UnityEngine;
using System.Collections;

/// <summary>
/// Teh wall tile script, which is impassable
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Wall")]
public class btWallTile : btTile
{	
	public override bool IsPassable()
	{
		return false;	
	}
}
