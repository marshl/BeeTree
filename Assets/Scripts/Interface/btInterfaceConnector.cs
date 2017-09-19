using UnityEngine;
using System.Collections;

/// <summary>
/// The connector object that is attached to inner nodes and has a draggable widget to connect to other nodes
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Interface/Node Connector")]
public class btInterfaceConnector : MonoBehaviour
{
	public GameObject connectionPrefab;
	public btInterfaceInnerNode attachedNode;
	public Transform dragWidgetHome;
	public btInterfaceDragWidget dragWidget;
	public LineRenderer connectionLine;
	
	private void Start()
	{
		this.connectionLine = this.GetComponent<LineRenderer> ();
		if ( this.connectionLine == null ) 
		{
			Debug.LogError ( "UIConnector is missing LineRenderer", this.gameObject );	
		}
		this.connectionLine.SetVertexCount( 5 );
	}

	private void Update()
	{
		if ( this.dragWidget.isBeingDragged == false ) 
		{
			btInterfaceNode child = this.dragWidget.currentChild;
			
			if ( child != null // If it is attached to a node
			  && child.parentConnector == null // That is not already attached to a node
			  && this.attachedNode.HasNodeInChildTree( child ) == false //And does not already have this node in its children tree
			  && ( child as btInterfaceInnerNode == null // And the node is not an inner node
			    || (child as btInterfaceInnerNode ).HasNodeInChildTree( this.attachedNode ) == false ) ) // Or if it is, it does not have this node as a child
			{
                this.AddChildNode( child );
			}

			this.dragWidget.transform.position = this.dragWidgetHome.position;	
		}
		
		for ( int i = 0; i < 5; ++i )
		{
			this.connectionLine.SetPosition( i, ( i % 2 ) == 0 ? this.GetOrigin() : this.GetDestination() );	
		}
	}

    public void AddChildNode( btInterfaceNode _node)
    {
        GameObject connectionObj = GameObject.Instantiate( connectionPrefab ) as GameObject;

        btInterfaceConnection connectionScript = connectionObj.GetComponent<btInterfaceConnection>();
        connectionScript.parent = this.attachedNode;
        connectionScript.child = _node;
        this.attachedNode.childConnectors.Add( connectionScript );
        _node.parentConnector = connectionScript;
		
		connectionObj.transform.parent = this.transform;
       // connectionObj.transform.parent = btInterfaceTreeManager.instance.transform;
        connectionObj.transform.localScale = Vector3.one;
		connectionObj.transform.localPosition = Vector3.zero;

        this.dragWidget.currentChild = null;

        this.attachedNode.invalidChildIndices = true;
        _node.indexDisplay.SetIndex(this.attachedNode.childConnectors.Count);
    }

	public Vector3 GetOrigin()
	{
		return Vector3.zero;
	}
	
	public Vector3 GetDestination()
	{
		return this.dragWidget.transform.localPosition + new Vector3 (0, 0, 0.02f);	
	}
}
