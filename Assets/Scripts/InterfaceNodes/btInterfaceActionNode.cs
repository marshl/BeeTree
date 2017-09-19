using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Action/Action Base")]
public abstract class btInterfaceActionNode : btInterfaceLeafNode
{
	public override Color GetTintColour()
	{
		return Color.red;
	}
}
