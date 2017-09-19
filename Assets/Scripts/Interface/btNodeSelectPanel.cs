using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/Interface/Node Select Panel")]
public class btNodeSelectPanel : MonoBehaviour 
{
	public static btNodeSelectPanel instance;
	
	public GameObject nodeTilePrefab;
	
	public UIPanel panel;
	public Transform gridTransform;
	public UIDraggableCamera draggableCamera;
	public Transform topLeft;
	public Transform bottomRight;
	public Transform blockerTransform;
	
	public Vector2 tileSize;
	private List<btNodeSelectTile> tileList;
	
	private void Awake()
	{
		Assert.Equals( instance, null );
		instance = this;
		this.tileList = new List<btNodeSelectTile>();
		this.CreateNodesFromLevelOptions();
		
		Vector3 scale = this.topLeft.localPosition - this.bottomRight.localPosition;
		this.blockerTransform.localScale = new Vector3( Mathf.Abs(scale.x), Mathf.Abs(scale.y), 1.0f );
		
		this.SetEditLock( true );
	}
		
	public void CreateNodesFromLevelOptions()
	{
		btLevelOptions levelOptions = Object.FindObjectOfType(typeof(btLevelOptions)) as btLevelOptions;
		if ( levelOptions == null )
		{
			Debug.LogError( "Could not find level options object" );
			return;
		}
		
		if ( levelOptions.allowedNodes == null 
		  || levelOptions.allowedNodes.Length == 0 )
		{
			Debug.LogError( "No allowed nodes defined in level options" );	
		}
		else
		{
			foreach ( btInterfaceTreeManager.INODE_TYPE nodeType in levelOptions.allowedNodes )
			{
				btNodeSelectTile tile = this.CreateNewTile( nodeType );
				tile.transform.localPosition = new Vector3( this.tileList.Count * tileSize.x, 0.0f, 0.0f );
				this.tileList.Add( tile );
				tile.SetLock( true );
			}
		}
		this.panel.clipRange = new Vector4( ((float)(this.tileList.Count - 1) / 2.0f) * this.tileSize.x, 0.0f,
			(float)(Mathf.Max(this.tileList.Count,4)) * this.tileSize.x, this.tileSize.y );
	}
	
	public btNodeSelectTile CreateNewTile( btInterfaceTreeManager.INODE_TYPE _nodeType )
	{
		if ( this.nodeTilePrefab == null )
		{
			Debug.LogError( "No node tile prefab defined", this );
			return null;
		}
		
		GameObject tileObj = GameObject.Instantiate( this.nodeTilePrefab ) as GameObject;
		tileObj.transform.parent = this.gridTransform;
		tileObj.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
		btNodeSelectTile tileScript = tileObj.GetComponent<btNodeSelectTile>();
		if ( tileScript == null )
		{
			Debug.LogError( "Could not find tile script on node tile prefab.", tileObj );
			return null;
		}
		tileScript.nodeType = _nodeType;
		
		UIDragCamera dragCameraScript = tileObj.GetComponent<UIDragCamera>();
		if ( dragCameraScript == null )
		{
			Debug.LogError( "No UIDragCamera script found on tile prefab.", tileObj );
			return tileScript;
		}
		dragCameraScript.draggableCamera = this.draggableCamera;
		
		tileScript.buttonScript.nodeType = _nodeType;
		GameObject tempObj = new GameObject();
		tempObj.SetActive( false );
		System.Type type = btInterfaceTreeManager.GetInterfaceNodeType( _nodeType );
		if ( type == null )
		{
			Debug.LogError( "Could not find matching type for " + _nodeType );
		}
		else
		{
			btInterfaceNode nodeScript = tempObj.AddComponent( type ) as btInterfaceNode;
			string materialPath = nodeScript.GetIconMaterialPath();
			Object iconResource = Resources.Load( materialPath );
			if ( iconResource == null )
			{
				Debug.LogError( "Cannot find material with path \"" + materialPath + "\"" );
			}
			else
			{
				Material iconMaterial = iconResource as Material;
				if ( iconMaterial == null )
				{
					Debug.LogError( "Resource \"" + iconResource.name
						+ "\" at path \"" + materialPath
						+ "\" cannot be converted into a meterial." );
				}	
				else
				{
					tileScript.texture.material = iconMaterial;
				}
			}
			
			tileScript.label.text = nodeScript.GetDisplayLabel();
		}
				
		Destroy( tempObj );
		return tileScript;
	}
	
	public void SetEditLock( bool _isLocked )
	{
		this.blockerTransform.gameObject.SetActive( _isLocked );
		foreach ( btNodeSelectTile tile in this.tileList )
		{
			tile.SetLock( _isLocked );	
		}
		
	}
}
