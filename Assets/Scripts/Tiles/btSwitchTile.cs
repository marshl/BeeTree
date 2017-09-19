using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/Tiles/Switch")]
public class btSwitchTile : btTile
{
	// Editor variables
	
	/// <summary>
	/// The toggle wall tiles that will be toggle whena  boulder is placed on this tile
	/// </summary>
	public List<btToggleWallTile> wallsToToggle;
	public UITexture texture;
	public Material upMaterial;
	public Material downMaterial;
	
	public override void OnMoveableTileEnter( btTile _tile, DIRECTION _enterDirection )
	{
		if ( _tile as btBoulderTile != null && this.wallsToToggle != null )
		{
			this.texture.material = this.downMaterial;
			
			foreach ( btToggleWallTile toggleWall in this.wallsToToggle )
			{
				toggleWall.SetToggle( !toggleWall.isActive );	
			}
		}
	}
	
	public override void OnMoveableTileExit( btTile _tile, DIRECTION _enterDirection )
	{
		if ( _tile as btBoulderTile != null  && this.wallsToToggle != null )
		{
			this.texture.material = this.upMaterial;
			
			foreach ( btToggleWallTile toggleWall in this.wallsToToggle )
			{
				toggleWall.SetToggle( !toggleWall.isActive );	
			}
		}
	}
}
