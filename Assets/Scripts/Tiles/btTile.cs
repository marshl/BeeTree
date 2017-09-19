using UnityEngine;
using System.Collections;

/// <summary>
/// The base tile class, from which all other tiles inherit
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Tiles/Base Tile")]
public abstract class btTile : MonoBehaviour
{
	public enum DIRECTION
	{
		NORTH,
		EAST,
		SOUTH,
		WEST,
	};	
	
	public enum TILESTATE
	{
		NONE,	
		MOVING_FORWARD,
		TURNING_LEFT,
		TURNING_RIGHT,
		FORCE_MOVE,
		SLIDING,
		JUMPING,
	};
	
	public DIRECTION direction;
	
	public btBoardSpace currentSpace;
	public DIRECTION initialDirection;
	public btBoardSpace initialSpace;
	
	public TILESTATE tileState;
	public float currentTileDisplacement;
	
	public bool isTurning = false;
	
	public static float WALK_MOVE_SPEED = 1.5f; // Tiles per second
	public static float SLIDE_MOVE_SPEED = 2.0f;
	public static float FORCE_MOVE_SPEED = 1.0f;
	public float currentMovement;
	
	public float currentRotation = 0.0f;
	public float startingRotation;
	public bool turnComplete = false;
	public static float TURN_SPEED = 180.0f; // Degrees per second
	
	public float jumpTimer;
	public static float JUMP_DURATION = 0.5f;
	
	public btBoardSpace startSpace;
	public btBoardSpace endSpace;
	
	public DIRECTION forceDirection;
	public DIRECTION slideDirection;
	////////////////////////////////
	// METHODS
	
	/// <summary>
	/// Resets the update values.
	/// </summary>
	public void ResetUpdateValues()
	{
		this.currentTileDisplacement = 0.0f;
		
		this.isTurning = false;
		this.currentMovement = 0.0f;
		
		this.currentRotation = 0.0f;
		this.turnComplete = false;
	}
	
	public bool TileUpdate()
	{
		switch ( this.tileState )
		{
			case TILESTATE.NONE:
			{
				return true;	
			}
			case TILESTATE.MOVING_FORWARD:
			{
				bool finishedMoving = this.MoveForward( btTile.WALK_MOVE_SPEED );
				if ( finishedMoving == true )
				{
					this.tileState = TILESTATE.NONE;
				}
				return finishedMoving;
			}	
			case TILESTATE.JUMPING:
			{
				if ( btTile.JUMP_DURATION == 0.0f )
					return true;
			
				this.jumpTimer += Time.deltaTime;
				float jumpDelta = jumpTimer / btTile.JUMP_DURATION;
				this.SetPosition( btTileManager.instance.GetPositionBetweenSpaces( this.startSpace, this.endSpace, jumpDelta ) );
				if ( jumpTimer >= btTile.JUMP_DURATION )
				{
					btTileManager.instance.AddTileMovementInDirection( this, this.direction, 2 );
					this.jumpTimer = 0.0f;
					this.tileState = TILESTATE.NONE;
					this.startSpace = this.endSpace = null;
					return true;
				}
				return false;
			}
			case TILESTATE.TURNING_LEFT:
			{
				if ( this.isTurning == true )
				{
					if ( this.SpinLeft() == true )
					{
						this.isTurning = false;	
					}
				}
				else
				{
					bool doneMoving = this.MoveForward( btTile.WALK_MOVE_SPEED );
					if ( doneMoving == true )
					{
						this.tileState = TILESTATE.NONE;
						return true;	
					}
				}
				return false;
			}
			case TILESTATE.TURNING_RIGHT:
			{
				if ( this.isTurning == true )
				{
					if ( this.SpinRight() == true )
					{
						this.isTurning = false;	
					}
				}
				else
				{
					bool doneMoving = this.MoveForward( btTile.WALK_MOVE_SPEED );
					if ( doneMoving == true )
					{
						this.tileState = TILESTATE.NONE;
						return true;	
					}
				}
				return false;
			}
			case TILESTATE.FORCE_MOVE:
			{
				bool doneMoving = this.MoveInDirection( btTile.FORCE_MOVE_SPEED, this.forceDirection );
				if ( doneMoving == true )
				{
					this.tileState = TILESTATE.NONE;	
				}
				return doneMoving;
			}
			case TILESTATE.SLIDING:
			{
				bool doneMoving = this.MoveInDirection( btTile.SLIDE_MOVE_SPEED, this.slideDirection );
				if ( doneMoving == true )
				{
					this.tileState = TILESTATE.NONE;	
				}
				return doneMoving;	
			}
			default:
			{
				Debug.LogError("Uncaught tile state \"" + this.tileState + "\"");
				return true;
			}
		}
	}
	
	public bool MoveForward( float _moveSpeed )
	{
		return this.MoveInDirection( _moveSpeed, this.direction );
	}
	
	public bool MoveInDirection( float _moveSpeed, DIRECTION _direction )
	{	
		this.currentMovement += Time.deltaTime * _moveSpeed;
		Vector2 pos = btTileManager.instance.GetPositionBetweenSpaces( this.startSpace, this.endSpace, this.currentMovement );
		this.SetPosition( pos );
		
		if ( this.currentMovement >= 1.0f )
		{
			this.currentMovement = 0.0f;
			btTileManager.instance.AddTileMovementInDirection( this, _direction );
			return true;	
		}
		return false;
	}
	
	/// <summary>
	/// Spins the tile to the left until it has finished a turn
	/// </summary>
	/// <returns>
	/// Whether the turn is complete or not
	/// </returns>
	public bool SpinLeft()
	{
		currentRotation -= Time.deltaTime * btTile.TURN_SPEED;
		this.SetRotation( this.startingRotation - this.currentRotation );
		if ( this.currentRotation <= -90.0f )
		{
			this.SnapDirectionLeft();
			return true;
		}
		return false;
	}
	
	/// <summary>
	/// Spins the tile to the right until the turn is complete
	/// </summary>
	/// <returns>
	/// Whether the turn is complete or not
	/// </returns>
	public bool SpinRight()
	{
		currentRotation += Time.deltaTime * btTile.TURN_SPEED;
		this.SetRotation( this.startingRotation - this.currentRotation );
		if ( this.currentRotation >= 90.0f )
		{
			this.SnapDirectionRight();
			return true;
		}
		return false;
	}
	
	public void SnapDirectionLeft()
	{
		this.direction = btTileManager.DirectionTurnedLeft( this.direction );
		this.ResetRotationUsingDirection();
	}
	
	public void SnapDirectionRight()
	{
		this.direction = btTileManager.DirectionTurnedRight( this.direction );
		this.ResetRotationUsingDirection();
	}
	
	public void ResetRotationUsingDirection()
	{
		float zrot = btTileManager.DirectionAsRotation( this.direction );
		this.transform.rotation = Quaternion.Euler( 0.0f, 0.0f, zrot );
	}
	
	public void SetRotation( float _rotation )
	{
		this.transform.rotation = Quaternion.Euler( 0.0f, 0.0f, _rotation );
	}
	
	public float GetRotation()
	{
		return this.transform.rotation.eulerAngles.z;	
	}
	
	public void SetPosition( Vector2 _pos )
	{
		this.transform.localPosition = new Vector3( _pos.x, _pos.y, this.transform.localPosition.z );		
	}
	
	////////////////////////////////
	// VIRTUAL METHODS
	
	public virtual bool IsPassable()
	{
		return true;	
	}
	
	public virtual bool GetIsPassableByTileType<T>()
	{
		return true;	
	}
	
	public virtual bool IsDeadly()
	{
		return false;	
	}
	
	public virtual bool IsMovableBoulder()
	{
		return false;	
	}
	
	public virtual void OnMoveableTileEnter( btTile _Tile, DIRECTION _enterDirection )
	{
		
	}
	
	public virtual void OnMoveableTileExit( btTile _Tile, DIRECTION _exitDirection )
	{
		
	}
	
	public virtual int GetTilePriority()
	{
		return 0;	
	}
}
