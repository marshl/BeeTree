using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/Interface/UI Tree Manager")]
public class btInterfaceTreeManager : MonoBehaviour
{
	public static btInterfaceTreeManager instance;
	
	public Transform rootNodePlaceholder;
	
	public GameObject nodeTemplatePrefab;
	public GameObject receiverPrefab;
	public GameObject connectorPrefab;
	public GameObject deleteButtonPrefab;
	public GameObject indexDisplayPrefab;
	public GameObject infoButtonPrefab;
	public GameObject dotPrefab;

	public Transform edittingArea;
	
	public Material actionNodeMaterial;
	public Material innerNodeMaterial;
	public Material testNodeMaterial;
	
	private List<btInterfaceNode> nodeList;
	private bool editLock = false;
	public btInterfaceRootNode rootNode;
	
	public enum INODE_TYPE
	{
		// INNER_NODES
		SEQUENCE,
		PLAY_ONE,
		PLAY_ALL,
		LOOP,
		LOOPX,
		ROOT,
		
		// ACTION NODES
		MOVE_FORWARD,
		TURN_LEFT,
		TURN_RIGHT,
		PUSH,
		JUMP,
		
		// TEST NODES
		SIGHT_TEST,
		LEFT_TEST,
		RIGHT_TEST,
		BOULDER_TEST,
	};
	
	void Awake()
	{
		if(instance != null)
		{
			Debug.LogError("Multiply defined UITreeManager", this.gameObject);
			Debug.LogError("Multiply defined UITreeManager", instance.gameObject);
		}
		instance = this;	
		this.nodeList = new List<btInterfaceNode>();
		CreateRootNode();
	}
	
	private void Start()
	{
		this.edittingArea.GetComponent<Collider>().enabled = false;
	}
	
	private void Update ()
	{
		if(Input.GetKey(KeyCode.Escape))
		{
			Debug.Break();	
		}
	}
	
	public void SetEditLock( bool _locked)
	{
		if ( this.editLock == _locked )
			return;	
		
		this.editLock = _locked;
		
		foreach ( btInterfaceNode node in this.nodeList )
		{	
			node.SetLock( _locked );
		}
		this.edittingArea.GetComponent<Collider>().enabled = _locked;
	}
	
	public GameObject CreateInterfaceNode( INODE_TYPE _nodeType )
	{
		GameObject nodeTemplate = GameObject.Instantiate( this.nodeTemplatePrefab ) as GameObject;
		nodeTemplate.transform.parent = this.gameObject.transform;
		
		nodeTemplate.transform.localScale = new Vector3(1,1,1);
		nodeTemplate.transform.localPosition = new Vector3(0,0,0);
		
		btInterfaceNodeTemplate templateScript = nodeTemplate.GetComponent<btInterfaceNodeTemplate>();
		
		btInterfaceNode nodeScript = nodeTemplate.AddComponent( btInterfaceTreeManager.GetInterfaceNodeType( _nodeType ) ) as btInterfaceNode;
		
		this.nodeList.Add( nodeScript );
		nodeScript.nodeType = _nodeType;
		nodeScript.dragScript = nodeScript.GetComponent<UIDragObject>();
		
		btAreaLimiter areaLimiter = nodeTemplate.GetComponent<btAreaLimiter>();
		areaLimiter.limitTransform = this.edittingArea;
		
		btInterfaceInnerNode innerNodeScript = nodeScript as btInterfaceInnerNode;
		btInterfaceRootNode rootNodeScript = nodeScript as btInterfaceRootNode;
		
		GameObject infoButton = this.AddComponentToNode( nodeTemplate, this.infoButtonPrefab, templateScript.infoButtonPlaceholder );
		btInterfaceNodeInfo infoScript = infoButton.GetComponent<btInterfaceNodeInfo>();
		infoScript.attachedNode = nodeScript;
		
		nodeScript.glowTexture = templateScript.glowTexture;
		nodeScript.dotTexture = templateScript.dotTexture;
		
		if ( innerNodeScript != null )
		{
			GameObject connectorObj = this.AddComponentToNode( nodeTemplate, this.connectorPrefab, templateScript.connectorPlaceholder );
			
			btInterfaceConnector connectorScript = connectorObj.GetComponent<btInterfaceConnector>();
			connectorScript.attachedNode = innerNodeScript;
			btAreaLimiter dragLimiter = connectorScript.dragWidget.GetComponent<btAreaLimiter>();
			dragLimiter.limitTransform = this.edittingArea;
			
			innerNodeScript.Connector = connectorScript;
		}
		
		if ( nodeScript.CanHaveParent() )
		{
			GameObject receiverObj = this.AddComponentToNode( nodeTemplate, this.receiverPrefab, templateScript.receiverPlaceholder );
			
			btInterfaceReceiver receiverScript = receiverObj.GetComponent<btInterfaceReceiver>();
			receiverScript.attachedNode = nodeScript;
			nodeScript.receiver = receiverScript;
		}
		
		if( rootNodeScript == null )
		{
			GameObject deleteButtonObj = this.AddComponentToNode( nodeTemplate, this.deleteButtonPrefab, templateScript.deleteButtonPlaceholder );
			
			btInterfaceDeleteNode deleteScript = deleteButtonObj.GetComponent<btInterfaceDeleteNode>();
			deleteScript.attachedNode = nodeScript;
			nodeScript.deleteButton = deleteScript;
			
			GameObject indexDisplayObj = this.AddComponentToNode( nodeTemplate, this.indexDisplayPrefab, templateScript.indexDisplayPlaceholder );
			nodeScript.indexDisplay = indexDisplayObj.GetComponent<btInterfaceIndexDisplay>();
		}
		else
		{
			areaLimiter.enabled = false;	
		}
	
		string materialPath = nodeScript.GetIconMaterialPath();
		Object iconResource = Resources.Load( materialPath );
		if ( iconResource == null )
		{
			Debug.LogError( "Cannot find material with path \"" + materialPath + "\"" );
			return nodeTemplate;
		}
		Material iconMaterial = iconResource as Material;
		if ( iconMaterial == null )
		{
			Debug.LogError( "Resource \"" + iconResource.name
				+ "\" at path \"" + materialPath
				+ "\" cannot be converted into a meterial." );
			return nodeTemplate;
		}	
		
		Material backgroundMaterial = null;
		if ( nodeScript as btInterfaceInnerNode != null )
		{
			backgroundMaterial = this.innerNodeMaterial;	
		}
		else if ( nodeScript as btInterfaceActionNode != null )
		{
			backgroundMaterial = this.actionNodeMaterial;	
		}
		else if ( nodeScript as btInterfaceTestNode != null )
		{
			backgroundMaterial = this.testNodeMaterial;	
		}
		
		templateScript.iconTexture.material = iconMaterial;
		string nodeLabel = nodeScript.GetDisplayLabel();
		nodeTemplate.name = nodeLabel + "InterfaceNode"+this.nodeList.Count;
		
		templateScript.backgroundTexture.material = backgroundMaterial;
		
		// Clean up template object placeholders
		GameObject.Destroy( templateScript.connectorPlaceholder.gameObject );
		GameObject.Destroy( templateScript.deleteButtonPlaceholder.gameObject );
		GameObject.Destroy( templateScript.indexDisplayPlaceholder.gameObject );
		GameObject.Destroy( templateScript.receiverPlaceholder.gameObject );
		
		return nodeTemplate;
	}
	
	private GameObject AddComponentToNode( GameObject _node, GameObject _component, Transform _position )
	{
		GameObject componentObj = GameObject.Instantiate( _component ) as GameObject;
		componentObj.transform.parent = _node.transform;
		componentObj.transform.position = _position.position;
		componentObj.transform.localScale = new Vector3( 1,1,1 );
		
		GameObject.Destroy( _position.gameObject );
		return componentObj;
	}
	
	private void CreateRootNode()
	{
		GameObject rootNodeObj = CreateInterfaceNode( INODE_TYPE.ROOT );	
		rootNodeObj.transform.position = this.rootNodePlaceholder.position;
		UIDragObject dragScript = rootNodeObj.GetComponent<UIDragObject>();
		dragScript.enabled = false;
		rootNodeObj.GetComponent<Collider>().enabled = false;
		
		this.rootNode = rootNodeObj.GetComponent<btInterfaceRootNode>();
	}
	
	public void DeleteInterfaceNode( btInterfaceNode _node )
	{
		bool removed = this.nodeList.Remove( _node );	
		if ( removed == false )
		{
			Debug.LogError( "Interface node " + _node + " not found in node list." );
			return;
		}
		
		btInterfaceInnerNode innerNode = _node as btInterfaceInnerNode;
		if ( innerNode != null )
		{
			innerNode.RemoveChildrenConnections();
		}
		
		_node.RemoveParent();
		
		if ( btUIManager.instance.currentNode == _node )
		{
			btUIManager.instance.HideNodeDetailPanel();	
		}
		
		GameObject.Destroy( _node.gameObject );
		
	}
	
	public void DestroyAllNodesExceptRoot()
	{
		for ( int i = 0; i < this.nodeList.Count; ++i )
		{
			btInterfaceNode node = this.nodeList[i];
			if ( node == this.rootNode )
				continue;
			
			this.DeleteInterfaceNode( node );
			--i;
		}
	}
	
	public void RemoveFunctionNodeConnections()
	{
		foreach ( btInterfaceNode node in this.nodeList )
		{
			node.functionNode = null;	
		}
	}
	
	public void UpdateNodeGlowColour()
	{
		foreach ( btInterfaceNode node in this.nodeList )
		{
			node.UpdateGlowColour();	
		}
	}
	
	public void ResetNodeGlowColour()
	{
		foreach ( btInterfaceNode node in this.nodeList )
		{
			node.ResetGlow();	
		}
	}
	
	public static System.Type GetInterfaceNodeType( INODE_TYPE _nodeType )
	{
		switch( _nodeType )
		{
			case btInterfaceTreeManager.INODE_TYPE.ROOT:
			{
				return typeof( btInterfaceRootNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.SEQUENCE:
			{
				return typeof( btInterfaceSequenceNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.PLAY_ALL:
			{
				return typeof( btInterfacePlayAllNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.PLAY_ONE:
			{
				return typeof( btInterfacePlayAllNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.LOOP:
			{
				return typeof( btInterfaceLoopNode );	
			}
			case btInterfaceTreeManager.INODE_TYPE.LOOPX:
			{
				return typeof( btInterfaceLoopXNode );	
			}
			
			
			case btInterfaceTreeManager.INODE_TYPE.MOVE_FORWARD:
			{
				return typeof( btInterfaceMoveForwardNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.TURN_LEFT:
			{
				return typeof( btInterfaceTurnLeftNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.TURN_RIGHT:
			{
				return typeof( btInterfaceTurnRightNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.PUSH:
			{
				return typeof( btInterfacePushNode );	
			}
			case btInterfaceTreeManager.INODE_TYPE.JUMP:
			{
				return typeof( btInterfaceJumpNode );	
			}
			
			
			case btInterfaceTreeManager.INODE_TYPE.SIGHT_TEST:
			{
				return typeof( btInterfaceSightTestNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.RIGHT_TEST:
			{
				return typeof( btInterfaceRightTestNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.LEFT_TEST:
			{
				return typeof( btInterfaceLeftTestNode );
			}
			case btInterfaceTreeManager.INODE_TYPE.BOULDER_TEST:
			{
				return typeof( btInterfaceBoulderTestNode );
			}
			
			default:
			{
				Debug.LogError("Uncaught node type: "+_nodeType);
				return null;
			}
		}
	}
}
