using UnityEngine;
using System.Collections;

public abstract class btInterfaceTestNode : btInterfaceLeafNode
{
	public override Color GetTintColour()
	{
		return Color.green;
	}
}
