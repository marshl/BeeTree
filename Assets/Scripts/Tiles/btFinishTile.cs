using UnityEngine;
using System.Collections;

/// <summary>
/// The tile upon which the player ust reach to finish a level
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Finish")]
public class btFinishTile : btTile {

	public override void OnMoveableTileEnter( btTile _tile, DIRECTION _enterDirection )
	{
		btGameManager.instance.CompleteLevel();
	}
}
