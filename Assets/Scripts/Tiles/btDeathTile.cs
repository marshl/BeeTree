using UnityEngine;
using System.Collections;

/// <summary>
/// A tile upon which the player is killed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Death")]
public class btDeathTile : btTile {

	public override void OnMoveableTileEnter( btTile _tile, DIRECTION _enterDirection )
	{
		btGameManager.instance.KillPlayer();
		btUIManager.instance.DisplayMessageDialog( "You Died", "Don't fly into venus fly traps!" );
	}
}
