using UnityEngine;
using System.Collections;

/// <summary>
/// A connection between two interface nodes, attached to an object with a line renderer
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Interface/Node Connection")]
public class btInterfaceConnection : MonoBehaviour
{
	public btInterfaceInnerNode parent;
	public btInterfaceNode child;
	public LineRenderer connectionLine;
	
	public static float heightAboveNodes = 0.02f;
	
	void Start()
	{
#if UNITY_EDITOR
		if ( this.parent == null ) Debug.LogError( "Parent node is null" );
		if ( this.child == null ) Debug.LogError( "Child node is null" );
#endif
	}
	
	private void Update ()
	{
        for ( int i = 0; i < 5; ++i )
        {
            this.connectionLine.SetPosition( i, (i % 2) == 0 ? this.GetOrigin() : this.GetDestination() );
        }
	}
	
	public Vector3 GetOrigin()
	{
		return Vector3.zero;//new Vector3(0.0f, 0.0f, btInterfaceConnection.heightAboveNodes );
	}
	
	/// <summary>
	/// Returns the destination position of the connection line
	/// </summary>
	/// <returns>
	/// The destination.
	/// </returns>
	public Vector3 GetDestination()
	{
		return (this.child.transform.localPosition + this.child.receiver.transform.localPosition)
			- ( this.parent.transform.localPosition + this.parent.Connector.transform.localPosition )
			+ new Vector3(0.0f, 0.0f, btInterfaceConnection.heightAboveNodes );
	}
	
	/// <summary>
	/// Gets the angle from parent to child.
	/// </summary>
	/// <returns>
	/// The angle from parent to child.
	/// </returns>
	public float GetAngleFromParentToChild()
	{
		return btCommon.AngleAroundAxis( Vector3.down, -this.parent.Connector.transform.position +
			this.child.receiver.transform.position, Vector3.forward );
	}
}
