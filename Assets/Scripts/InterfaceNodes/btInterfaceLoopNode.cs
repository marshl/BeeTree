using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Inner/Loop")]
public class btInterfaceLoopNode : btInterfaceInnerNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionLoopNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Loop";
	}
	
	public override string GetDescription()
	{
		return "A loop node runs all of its children in order from left to right." +
				"But if any of those children fail, then the loop node also fails." +
				"When a sequence node finishes executing all of its children, it starts again at the first child.";
	}
	
	public override string GetIconMaterialName()
	{
		return "Loop";	
	}
}
