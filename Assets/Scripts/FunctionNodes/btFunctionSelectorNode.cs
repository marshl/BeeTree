using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
public class btFunctionSelectorNode : btFunctionInnerNode
{
	public override NODE_STATE TreeExecute()
	{
        Debug.LogException(new System.NotImplementedException());
		/* Will probably not be implemented */
		return NODE_STATE.FAILED;
	}
}
