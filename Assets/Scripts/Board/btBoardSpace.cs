using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class represents one tile on the board
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Space")]
public class btBoardSpace
{
    /// <summary>
    /// All of the tiles on this space
    /// </summary>
	public List<btTile> tiles;
	
    /// <summary>
    /// The X index of this space on the board
    /// </summary>
	public int x;

    /// <summary>
    /// THe Y index of this space on the board
    /// </summary>
	public int y;
	
    /// <summary>
    /// Deafult constructor
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
	public btBoardSpace( int _x, int _y )
	{
		this.x = _x;
		this.y = _y;
		
		this.tiles = new List<btTile>();	
	}
	
    /// <summary>
    /// Returns whether there are any tiles on this space
    /// </summary>
    /// <returns>Are there no tiles on this space</returns>
	public bool IsEmpty()
	{
		return this.tiles.Count == 0;	
	}
	
    /// <summary>
    /// Returns the number of tiles on this space
    /// </summary>
    /// <returns>The number of tiles on this space</returns>
	public int GetTileCount()
	{
		return this.tiles.Count;	
	}
	
    /// <summary>
    /// Returns whether there are any impassable tiles on this space
    /// </summary>
    /// <returns>If there are any impassable tiles on this space</returns>
	public bool IsPassable()
	{
		foreach (btTile tile in this.tiles)
		{
			if ( tile.IsPassable() == false)
			{
				return false;	
			}
		}
		return true;
	}
	
    /// <summary>
    /// Returns any tiles of the specified type that are on this space
    /// </summary>
    /// <typeparam name="T">The tile type</typeparam>
    /// <returns>All tiles of the specified type on this space</returns>
	public List<T> GetTilesOfType<T>() where T : btTile
	{
		List<T> list = new List<T>();
		foreach ( btTile tile in this.tiles )
		{
			T t = tile as T;
			if ( t != null )
			{
				list.Add( t );	
			}
		}	
		return list;
	}
}

