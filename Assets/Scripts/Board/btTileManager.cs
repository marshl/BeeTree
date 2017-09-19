using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The manager of the playing board and all of the spaces and tiles on it
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Managers/Tile Manager")]
public class btTileManager : MonoBehaviour 
{
	// Singleton Instance
	public static btTileManager instance;

    /// Editor Variables
    public Transform gridTransform;
    public Vector3 tileSize = new Vector3(64, 64, 0);

    /// Public Variables
    public List<List<btBoardSpace>> tileGrid;
    public btPlayerTile playerTile;
 
    /// Private Variables
    private int tilesVertical;
    private int tilesHorizontal;
    
    private Vector3 bottomLeft;
    private List<btTileMovement> tileMovements;

    /// <summary>
    /// A movement of a tile from one space to another (stored to prevent array access issues)
    /// </summary>
	private class btTileMovement
	{
		public btTile tile;	
		public int newX;
		public int newY;
		public btTile.DIRECTION direction;
		
		public btTileMovement( btTile _tile, int _newX, int _newY, btTile.DIRECTION _movementDirection )
		{
			this.tile = _tile;
			this.newX = _newX;
			this.newY = _newY;
			this.direction = _movementDirection;
		}
	}
	
	private void Awake()
	{
#if UNITY_EDITOR
        /// Check for other managers
		if ( btTileManager.instance != null )
		{
			Debug.LogError( "Multiply defined TileManager", this.gameObject );
            Debug.LogError("Multiply defined TileManager", btTileManager.instance.gameObject);
			return;
		}
#endif
        btTileManager.instance = this;	
		
        /// FInd the board dimensions using the options for the level
		btLevelOptions levelOptions = GameObject.FindObjectOfType( typeof(btLevelOptions) ) as btLevelOptions;
		if ( levelOptions == null )
		{
			Debug.LogError( "Could not find options for level" );	
		}
		else
		{
			this.tilesHorizontal = levelOptions.tilesHorizontal;
			this.tilesVertical   = levelOptions.tilesVertical;
		}
		
        /// Populate tile grid
		this.tileGrid = new List<List<btBoardSpace>>();
		for ( int y = 0; y < this.tilesVertical; ++y )
		{
			this.tileGrid.Add( new List<btBoardSpace>() );
			for ( int x = 0; x < this.tilesHorizontal; ++x )
			{
				this.tileGrid[y].Add( new btBoardSpace( x, y ) );
			}
		}
		
		this.tileMovements = new List<btTileMovement>();
	}
	

	private void Start()
	{
		FindAndAddTiles();
		FindPlayerTile();
	
#if UNITY_EDITOR
		Object finishTile = GameObject.FindObjectOfType( typeof( btFinishTile ) );
		if ( finishTile == null )
		{
			Debug.LogError( "Cannot find finish tile" );	
		}
#endif
		
		this.OrderTiles();
	}
	
    /// <summary>
    /// Finds all of the tiles in the scene and places them in the correct locations
    /// </summary>
	public int FindAndAddTiles()
	{
        this.bottomLeft = this.gridTransform.localPosition - this.gridTransform.localScale * 0.5f;

		btTile[] tiles = Object.FindObjectsOfType(typeof(btTile)) as btTile[];
		foreach ( btTile tile in tiles )
		{
            /// Convert the tile position into a usable index
			Vector3 tilePos = tile.transform.position;
			int xIndex = (int)( tilePos.x + (0.5f  * (float)this.tilesHorizontal ));
			int yIndex = (int)( tilePos.y + (0.5f  * (float)this.tilesVertical ));

			if ( this.IsValidTileIndex(xIndex, yIndex) == false )
			{
				Debug.LogError( "Tile position out of board range: xIndex="+xIndex+" yIndex="+yIndex+" position="+tilePos);
				GameObject.Destroy( tile.gameObject );
				continue;
			}

            /// Set up the tile information
			//tile.xIndex = tile.xInitial = xIndex;
			//tile.yIndex = tile.yInitial = yIndex;
			tile.initialDirection = tile.direction = TileRotationToDirection( tile );
			
			btBoardSpace space = this.tileGrid[yIndex][xIndex];
			space.tiles.Add( tile );
			tile.currentSpace = tile.initialSpace = space;
            /// Reset the tile position using its new indices
			tile.transform.parent = this.transform;
			tile.transform.localScale = this.tileSize;
			this.ResetTilePosition( tile );
			
			#if UNITY_EDITOR
			foreach ( btTile spaceTile in space.tiles )
			{
				if ( spaceTile != tile && spaceTile.GetType() == tile.GetType() )
				{
					Debug.LogError( "Duplicate tile \"" + spaceTile.GetType().ToString() + "\" at (" + xIndex + ", " + yIndex + ")", tile );	
				}
			}
			#endif
			
		}	
		return tiles.Length;
	}
	
	public void OrderTiles()
	{
		List<btTile> tiles = new List<btTile>();
		tiles.AddRange( Object.FindObjectsOfType( typeof(btTile) ) as btTile[] );
		
		tiles.Sort( delegate(btTile x, btTile y) {
			return x.GetTilePriority().CompareTo( y.GetTilePriority() );
		} );
		
		for ( int i = 0; i < tiles.Count; ++i )
		{
			Vector3 pos = tiles[i].transform.localPosition;
			tiles[i].transform.localPosition = new Vector3( pos.x, pos.y, -1.0f - (float)i / (float)tiles.Count );
		}
	}
	
    /// <summary>
    /// Tile update loop, managed by the game manager
    /// Returns true once all tiles have finished movements and such
    /// </summary>
    /// <returns>When all tiles have finished their update</returns>
	public bool UpdateTiles()
	{
		bool allComplete = true;
		for ( int y = 0; y < this.tilesVertical; ++y )
		{
			for ( int x = 0; x < this.tilesHorizontal; ++x )
			{
				btBoardSpace space = this.GetSpace( x, y );
                if ( space == null )
                    continue;

				foreach ( btTile tile in space.tiles )
				{
					bool tileUpdate = tile.TileUpdate();
					if ( tileUpdate == false )
					{
						allComplete = false;	
					}
				}
			}
		}
		return allComplete;
	}
	
    /// <summary>
    /// Moves all tiles that have changed their position since the last update
    /// </summary>
    /// <returns>True if no movements have occurred, otherwise false</returns>
	public bool ExecuteTileMovements()
	{
		if ( this.tileMovements.Count == 0 )
		{
			return true;	
		}
		
		foreach ( btTileMovement movement in this.tileMovements )
		{
			this.MoveTileToSpaceIndex( movement.tile, movement.newX, movement.newY, true, movement.direction );
		}
		this.tileMovements.Clear();
		return false;
	}
	
    /// <summary>
    /// Converts a board index into the coresponding position wher that tile should be placed
    /// Does not perform error checking on the extents of the indices
    /// </summary>
    /// <param name="_x">The x index</param>
    /// <param name="_y">The Y index</param>
    /// <returns>The position those indices</returns>
	public Vector2 BoardIndexToPosition( int _x, int _y )
	{
		return new Vector2( 
			( (float)_x + 0.5f ) * this.tileSize.x + this.bottomLeft.x,
			( (float)_y + 0.5f ) * this.tileSize.y + this.bottomLeft.y );
	}
	
	public Vector2 GetSpacePosition( btBoardSpace _space )
	{
		return this.BoardIndexToPosition( _space.x, _space.y );
	}	
	
	public Vector2 GetPositionBetweenSpaces( btBoardSpace _space1, btBoardSpace _space2, float _d )
	{
		return this.GetPositionBetweenSpaces( _space1.x, _space1.y, _space2.x, _space2.y, _d );
	}
	
    /// <summary>
    /// Returns the position between two tiles
    /// No error checks are performed on the indices or the delta
    /// </summary>
    /// <param name="_x1">The x index of the start tile</param>
    /// <param name="_y1">The y index of the start tile</param>
    /// <param name="_x2">The x index of the end tile</param>
    /// <param name="_y2">The y index of the end tile</param>
    /// <param name="_d">The delta between the two tiles</param>
    /// <returns></returns>
	public Vector2 GetPositionBetweenSpaces( int _x1, int _y1, int _x2, int _y2, float _d )
	{
		Vector2 start = this.BoardIndexToPosition( _x1, _y1 );
		Vector2 end = this.BoardIndexToPosition( _x2, _y2 );
		return start + ( ( end - start ) * _d );
	}
	
    /// <summary>
    /// Finds the player tile in the board and stores it
    /// </summary>
	public void FindPlayerTile()
	{
		List<btPlayerTile> playerTiles = GetTilesOfType<btPlayerTile>();
		if ( playerTiles.Count == 0 )
		{
			Debug.LogError( "Cannot find player tile" );
            return;
		}
		else if ( playerTiles.Count > 1 )
		{
			Debug.LogError( "More than one player tile found" );
		}
		this.playerTile = playerTiles[0];	
	}
	
    /// <summary>
    /// Finds all tiles of the given type on the board
    /// </summary>
    /// <typeparam name="T">The tile type to find</typeparam>
    /// <returns>ANy tiles found</returns>
	public List<T> GetTilesOfType<T>() where T : btTile
	{
		List<T> tiles = new List<T>();
		
		for ( int y = 0; y < instance.tilesVertical; ++y )
		{
			for ( int x = 0; x < instance.tilesHorizontal; ++x )
			{
				btBoardSpace space = instance.tileGrid[y][x];
				foreach ( btTile node in space.tiles )
				{
					if ( node as T != null )
					{
						tiles.Add( node as T );	
					}
				}
			}
		}
		return tiles;
	}
	
    /// <summary>
    /// Rotates an input direction 90 Degrees anti-clockwise
    /// </summary>
    /// <param name="_direction">The input direction</param>
    /// <returns>The rotated direction</returns>
	public static btTile.DIRECTION DirectionTurnedLeft( btTile.DIRECTION _direction )
	{
		switch ( _direction )	
		{
		case btTile.DIRECTION.NORTH:
			return btTile.DIRECTION.WEST;
		case btTile.DIRECTION.EAST:
			return btTile.DIRECTION.NORTH;
		case btTile.DIRECTION.SOUTH:
			return btTile.DIRECTION.EAST;
		case btTile.DIRECTION.WEST:
			return btTile.DIRECTION.SOUTH;
		default:
			Debug.LogError( "Uncaught direction \""+_direction+"\" in DirectionAsVector()" );
			return btTile.DIRECTION.NORTH;
		}
	}

    /// <summary>
    /// Rotates an input direction 90 Degrees clockwise
    /// </summary>
    /// <param name="_direction">The input direction</param>
    /// <returns>The rotated direction</returns>
	public static btTile.DIRECTION DirectionTurnedRight( btTile.DIRECTION _direction )
	{
		switch ( _direction )	
		{
		case btTile.DIRECTION.NORTH:
			return btTile.DIRECTION.EAST;
		case btTile.DIRECTION.EAST:
			return btTile.DIRECTION.SOUTH;
		case btTile.DIRECTION.SOUTH:
			return btTile.DIRECTION.WEST;
		case btTile.DIRECTION.WEST:
			return btTile.DIRECTION.NORTH;
		default:
			Debug.LogError("Uncaught direction \""+_direction+"\" in DirectionAsVector()");
			return btTile.DIRECTION.NORTH;
		}
	}

    /// <summary>
    /// Rotates an input direction 180 Degrees
    /// </summary>
    /// <param name="_direction">The input direction</param>
    /// <returns>The rotated direction</returns>
	public static btTile.DIRECTION DirectionReversed( btTile.DIRECTION _direction )
	{
		switch ( _direction )	
		{
		case btTile.DIRECTION.NORTH:
			return btTile.DIRECTION.SOUTH;
		case btTile.DIRECTION.EAST:
			return btTile.DIRECTION.WEST;
		case btTile.DIRECTION.SOUTH:
			return btTile.DIRECTION.NORTH;
		case btTile.DIRECTION.WEST:
			return btTile.DIRECTION.EAST;
		default:
			Debug.LogError("Uncaught direction \""+_direction+"\" in DirectionAsVector()");
			return btTile.DIRECTION.NORTH;
		}
	}
	
    /// <summary>
    /// COnverts the input direction into an equivalent vector
    /// </summary>
    /// <param name="_direction"></param>
    /// <returns></returns>
	public static Vector2 DirectionAsVector( btTile.DIRECTION _direction )
	{
		switch ( _direction )	
		{
		case btTile.DIRECTION.NORTH:
			return new Vector2(0,1);
		case btTile.DIRECTION.EAST:
			return new Vector2(1,0);
		case btTile.DIRECTION.SOUTH:
			return new Vector2(0, -1);
		case btTile.DIRECTION.WEST:
			return new Vector2(-1, 0);
		default:
			Debug.LogError("Uncaught direction \""+_direction+"\" in DirectionAsVector()");
			return new Vector2(0,0);
		}
	}
	
    /// <summary>
    /// Returns the corresponding direction of the rotation of a tile
    /// </summary>
    /// <param name="_tile">The input tile</param>
    /// <returns>The direction that tile is facing</returns>
	public static btTile.DIRECTION TileRotationToDirection( btTile _tile )
	{
		float zrot = _tile.transform.rotation.eulerAngles.z;
        /// Clamp the rotation
		while ( zrot >= 360.0f) zrot -= 360.0f;
		while ( zrot < 0.0f) zrot += 360.0f;
		
		if ( zrot >= 45.0f && zrot < 135.0f )
		{
			return btTile.DIRECTION.WEST;	
		}
		else if ( zrot >= 135.0f && zrot < 225.0f )
		{
			return btTile.DIRECTION.SOUTH;	
		}
		else if ( zrot >= 225.0f && zrot < 315.0f )
		{
			return btTile.DIRECTION.EAST;
		}
		else
		{
			return btTile.DIRECTION.NORTH;	
		}
	}
	
    /// <summary>
    /// Converts an input direction into the corresponding rotation
    /// </summary>
    /// <param name="_direction">The input direction</param>
    /// <returns>The z rotation value</returns>
	public static float DirectionAsRotation( btTile.DIRECTION _direction )
	{
		switch ( _direction )	
		{
		case btTile.DIRECTION.NORTH:
			return 0.0f;
		case btTile.DIRECTION.EAST:
			return 270.0f;
		case btTile.DIRECTION.SOUTH:
			return 180.0f;
		case btTile.DIRECTION.WEST:
			return 90.0f;
		default:
			Debug.LogError("Uncaught direction \""+_direction+"\" in DirectionAsVector()");
			return 0.0f;
		}
	}
	
    /// <summary>
    /// Finds the direction difference between two spaces
    /// </summary>
    /// <param name="_space1">The start space</param>
    /// <param name="_space2">The end space</param>
    /// <returns>The direction from the start space to the end space</returns>
	public btTile.DIRECTION GetDirectionBetweenSpaces( btBoardSpace _space1, btBoardSpace _space2 )
	{
		int x = _space1.x - _space2.x;
		int y = _space1.y - _space2.y;
		
		if ( x != 0 ) x /= x;
		if ( y != 0 ) y /= y;

		if ( x == 0 && y == 1 )
		{
			return btTile.DIRECTION.NORTH;
		}
		else if ( x == 1 && y == 0 )
		{
			return btTile.DIRECTION.EAST;
		}
		else if ( x == 0 && y == -1 )
		{
			return btTile.DIRECTION.SOUTH;
		}
		else if ( x == -1 && y == 0 )
		{
			return btTile.DIRECTION.WEST;
		}
		else
		{
			Debug.Log("Uncaught movement between tiles ("+_space1.x+","+_space1.y+") -> ("+_space2.x+","+_space2.y+"): ("+x+","+y+")");
			return btTile.DIRECTION.NORTH;
		}
	}
	
    /// <summary>
    /// Returns whether the input indices are valid
    /// </summary>
    /// <param name="_x">The x index</param>
    /// <param name="_y">The y index</param>
    /// <returns>True if the indices are valid, otherwise false</returns>
	public bool IsValidTileIndex( int _x, int _y )
	{
		return _y >= 0 && _y < this.tilesVertical
			&& _x >= 0 && _x < this.tilesHorizontal;
			
	}
	
    /// <summary>
    /// Returns the space at the given indices
    /// Returns null if the indices are invalid
    /// </summary>
    /// <param name="_x">The x index</param>
    /// <param name="_y">The y index</param>
    /// <returns>The space at those indices</returns>
	public btBoardSpace GetSpace( int _x, int _y )
	{
		if ( IsValidTileIndex( _x, _y ) == false )
			return null;
		
		return instance.tileGrid[_y][_x];
	}
	
    /// <summary>
    /// Returns the tile at the given space indices with the given tile index
    /// </summary>
    /// <param name="_x">The x index</param>
    /// <param name="_y">The y index</param>
    /// <param name="_index">The tile index</param>
    /// <returns></returns>
	public btTile GetTileAtSpaceWithIndex( int _x, int _y, int _index )
	{
        btBoardSpace space = this.GetSpace( _x, _y );
		if ( space == null )
			return null;
	
		if( _index < 0 || _index >= space.tiles.Count )
		{
			return null;
		}
		return space.tiles[_index];
	}
	
    /// <summary>
    /// Returns the tile that is "steps" spaces away from "tile" in "direction"
    /// </summary>
    /// <param name="_tile">The origin</param>
    /// <param name="_Direction">The direction</param>
    /// <param name="_Steps">The steps to take</param>
    /// <returns></returns>
	public btBoardSpace GetSpaceInDirectionFromTile( btTile _tile, btTile.DIRECTION _Direction, int _Steps = 1 )
	{
		Vector2 directionVector = DirectionAsVector( _Direction );
		directionVector *= _Steps;
		
		int newX = (int)directionVector.x + _tile.currentSpace.x;//_tile.xIndex;
		int newY = (int)directionVector.y + _tile.currentSpace.y;//_tile.yIndex;
		
		if( IsValidTileIndex( newX, newY ) == false)
		{
			return null;
		}
		return instance.tileGrid[newY][newX];
	}
	
    /// <summary>
    /// Returns whether the given space can be passed
    /// </summary>
    /// <param name="_xIndex">The x index of the space</param>
    /// <param name="_yIndex">The y index of the space</param>
    /// <returns></returns>
	public bool GetIsSpacePassable( int _xIndex, int _yIndex )
	{
		btBoardSpace space = this.GetSpace( _xIndex, _yIndex );
		if ( space == null )
			return false;
		
		return space.IsPassable();
	}
	
    /// <summary>
    /// Adds a movement for the input tile to the input indices, using the movement direction
    /// </summary>
    /// <param name="_tile">The tile to be moved</param>
    /// <param name="_xIndex">The new x index</param>
    /// <param name="_yIndex">The new y index</param>
    /// <param name="_movementDirection"> The direction that the tile is being moved</param>
	public void AddTileMovement( btTile _tile, int _xIndex, int _yIndex, btTile.DIRECTION _movementDirection )
	{
		this.tileMovements.Add( new btTileManager.btTileMovement( _tile, _xIndex, _yIndex, _movementDirection ) );	
	}
	
    /// <summary>
    /// Adds a movement for the input tile to the input space, using the movement direction
    /// </summary>
    /// <param name="_tile">The tile to be moved</param>
    /// <param name="_space">The space to move the tile to</param>
    /// <param name="_movementDirection">The direction in which the tile is being moved</param>
	public void AddTileMovementToSpace( btTile _tile, btBoardSpace _space, btTile.DIRECTION _movementDirection )
	{
		this.AddTileMovement( _tile, _space.x, _space.y, _movementDirection );	
	}
	
    /// <summary>
    /// Adds a movement for "tile" in "direction", "steps" number of spaces (default of one)
    /// </summary>
    /// <param name="_tile">The tile to be moved</param>
    /// <param name="_direction">The direction to move the tile</param>
    /// <param name="_steps">The number of steps that the tile is being moved (default: 1)</param>
	public void AddTileMovementInDirection( btTile _tile, btTile.DIRECTION _direction, int _steps = 1 )
	{
        Vector2 directionVector = DirectionAsVector(_direction) * _steps;
		int newX = _tile.currentSpace.x + (int)directionVector.x;
		int newY = _tile.currentSpace.y + (int)directionVector.y;
		this.AddTileMovement( _tile, newX, newY, _direction );
	}
	
    /// <summary>
    /// Moves "tile" to "space" in the input direction
    /// </summary>
    /// <param name="_tile">The tile to be moved</param>
    /// <param name="_space">The space to move the tile to</param>
    /// <param name="_runCallbacks">Should callbacks be run on the tiles (false when resetting tiles, true during normal gameplay)</param>
    /// <param name="_movementDirection">The direction that the tile is moving in (used for callbacks)</param>
	private void MoveTileToSpace( btTile _tile, btBoardSpace _space, bool _runCallbacks, btTile.DIRECTION _movementDirection )
	{
		btBoardSpace oldSpace = _tile.currentSpace;//this.GetSpace( _tile.xIndex, _tile.yIndex );
		_tile.currentSpace.tiles.Remove( _tile );
		//_tile.xIndex = _space.x;
		//_tile.yIndex = _space.y;
		_tile.currentSpace = _space;
		_space.tiles.Add( _tile );
		
		if ( _runCallbacks == true )
		{
			if ( oldSpace != null )
			{
				foreach ( btTile oldTile in oldSpace.tiles )
				{
					if ( oldTile != _tile )
					{
						oldTile.OnMoveableTileExit( _tile, _movementDirection );
					}
				}
			}
			
			foreach ( btTile newTile in _space.tiles )
			{
				if ( newTile != _tile )
				{
					newTile.OnMoveableTileEnter( _tile, _movementDirection );	
				}
			}
		}
		
		this.ResetTilePosition( _tile );
	}
	
    /// <summary>
    /// Moves "tile" to the space in the given 
    /// </summary>
    /// <param name="_tile">The tile to be moved</param>
    /// <param name="_x">The x index of the space to move to</param>
    /// <param name="_y">The y index of the space to move to</param>
    /// <param name="_runCallbacks">Should callbacks be run on the tiles (false when resetting tiles, true during normal gameplay)</param>
    /// <param name="_movementDirection">The direction that the tile is moving in (used for callbacks)</param>
	private void MoveTileToSpaceIndex( btTile _tile, int _x, int _y, bool _runCallbacks, btTile.DIRECTION _movementDirection )
	{
		btBoardSpace space = this.GetSpace( _x, _y );
        if ( space == null )
        {
            Debug.LogError( "Cannot move tile \"" + _tile.name + "\" to (" + _x + ", " + _y + "): Invalid space index", _tile );
            return;
        }
		this.MoveTileToSpace( _tile, space, _runCallbacks, _movementDirection );
	}	

    /// <summary>
    /// Resets the position of the input tile
    /// </summary>
    /// <param name="_tile">The tile to reset</param>
	private void ResetTilePosition( btTile _tile )
	{
		_tile.SetPosition( this.GetSpacePosition( _tile.currentSpace ) );
		//_tile.SetPosition( this.BoardIndexToPosition( _tile.xIndex, _tile.yIndex ) );	
		_tile.ResetRotationUsingDirection();
	}
	
    /// <summary>
    /// Resets all tiles back to their original positions and states
    /// </summary>
	public void ResetTiles()
	{
		for ( int y = 0; y < instance.tilesVertical; ++y )
		{
			for ( int x = 0; x < instance.tilesHorizontal; ++x )
			{
				btBoardSpace space = instance.tileGrid[y][x];

				for ( int i = 0; i < space.tiles.Count; ++i)
				{
					btTile tile = space.tiles[i];
		
					if ( tile.initialSpace != space )
					//if ( tile.xInitial != x || tile.yInitial != y )
					{
						this.MoveTileToSpace( tile, tile.initialSpace, false, btTile.DIRECTION.NORTH );
					}
					
					tile.direction = tile.initialDirection;
					tile.tileState = btTile.TILESTATE.NONE;
					tile.ResetUpdateValues();
					this.ResetTilePosition( tile );
				}
			}
		}
	}
	
    /// <summary>
    /// Prints out the current state of the board
    /// </summary>
	public void Print()
	{
		string str = "";
		for ( int y = 0; y < instance.tilesVertical; ++y )
		{
			for ( int x = 0; x < instance.tilesHorizontal; ++x )
			{
				btBoardSpace space = instance.tileGrid[y][x];
				foreach ( btTile tile in space.tiles )
				{
					str += "("+x+"/"+y+"): "+tile.ToString()+"\n";
				}
			}
		}
		Debug.Log( str );
	}
}
