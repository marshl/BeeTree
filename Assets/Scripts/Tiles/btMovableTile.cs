using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/Tiles/Movable")]
public class btMovableTile : btTile
{
	public virtual bool GetCanPassTileType<T>() where T : btTile
	{
		return true;	
	}
}
