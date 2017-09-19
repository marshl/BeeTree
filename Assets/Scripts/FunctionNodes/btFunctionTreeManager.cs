using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the function node tree
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Managers/Tree Manager")]
public class btFunctionTreeManager : MonoBehaviour 
{
	public static btFunctionTreeManager instance;

    /// Private Variables
	private btFunctionRootNode rootFunctionNode;
	
	private void Awake()
	{
#if UNITY_EDITOR
		if( instance != null )
		{
			Debug.LogError("Multiple instances of btFunctionTreeManagerFound", this.gameObject);
            return;
		}
#endif
		instance = this;
	}
	
    /// <summary>
    /// Builds the function tree using the interface nodes. Fails if there are no children of the root node
    /// </summary>
    /// <returns>If the tree creation succeeded or not</returns>
	public bool BuildFunctionTree()
	{
		btInterfaceNode rootInterfaceNode = btInterfaceTreeManager.instance.rootNode;
		
		this.rootFunctionNode = new btFunctionRootNode();
		this.rootFunctionNode.interfaceNode = btInterfaceTreeManager.instance.rootNode;
		btInterfaceTreeManager.instance.rootNode.functionNode = this.rootFunctionNode;
		this.rootFunctionNode.AddChildNodes_r( rootInterfaceNode );
	
		return this.rootFunctionNode.children.Count > 0;
	}
	
    /// <summary>
    /// Destroys the functional tree
    /// </summary>
	public void DestroyTree()
	{
		btInterfaceTreeManager.instance.RemoveFunctionNodeConnections();
		this.rootFunctionNode = null;
	}
	
    /// <summary>
    /// Runs the functional tree and returns the execution state of the root node
    /// </summary>
    /// <returns>The state of the root node</returns>
	public btFunctionNode.NODE_STATE ExecuteTree()
	{
		this.rootFunctionNode.Reset();
		btFunctionNode.NODE_STATE executeState = this.rootFunctionNode.TreeExecute();
		return executeState;
	}
}
