using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Inner/Sequence")]
public class btInterfaceSequenceNode : btInterfaceInnerNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionSequenceNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Sequence";
	}
	
	public override string GetDescription()
	{
		return "A sequence node runs all of its child nodes in order from left to right." +
			 	"But if any of those children fail, then the sequence node also fails." +
				"When a sequence node finishes executing all of its children, it returns success.";
	}
	
	public override string GetIconMaterialName()
	{
		return "Sequence";	
	}
}
