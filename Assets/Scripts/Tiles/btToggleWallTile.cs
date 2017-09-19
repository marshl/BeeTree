using UnityEngine;
using System.Collections;

/// <summary>
/// A tile which can be toggled up and down by moving a boulder on top of a switch tile
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Toggle Wall")]
public class btToggleWallTile : btTile
{
	public bool isActive = true;
	
	public UITexture texture;
	public Material upMaterial;
	public Material downMaterial;
	
	public override bool IsPassable()
	{
		return this.isActive == false;
	}
	
	private void Awake()
	{
		this.SetToggle( this.isActive );	
	}
	
	public void SetToggle( bool _toggle )
	{
		this.isActive = _toggle;
		
		if ( this.isActive == true )
		{
			this.texture.material = upMaterial;
		}
		else
		{
			this.texture.material = downMaterial;	
		}
	}
}
