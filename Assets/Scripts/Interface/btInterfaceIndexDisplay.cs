using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/Interface/Node Index Display")]
public class btInterfaceIndexDisplay : MonoBehaviour {

	public UILabel label;
	
	public void SetIndex( int _index )
	{
		if ( _index < 0 )
		{
			this.label.text = "-";
		}
		else
		{
			this.label.text = (_index + 1).ToString();
		}
	}
}
