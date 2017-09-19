using UnityEngine;
using System.Collections;

/// <summary>
/// Restricts movement of an object to a particular area
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Area Limiter")]
public class btAreaLimiter : MonoBehaviour
{
	// Editor variables
	public Transform limitTransform;
	public Vector3 scale;
	
	
	// Private variables
	
	private Vector3 lastPos;
	
	private void Start()
	{
		this.lastPos = new Vector3(this.scale.x	* this.transform.lossyScale.x,
			this.scale.y * this.transform.lossyScale.y, 0.0f );
	}
	
	private void Update ()
	{
		if ( lastPos != this.transform.position )
		{	
			this.lastPos = this.transform.position;
			
			Vector3 limitPos = limitTransform.position;
			Vector3 limitScale = limitTransform.lossyScale;
			
			Vector3 minimum = limitPos - limitScale / 2 + this.scale / 480;
			Vector3 maximum = limitPos + limitScale / 2 - this.scale / 480;
			
			Vector3 worldPos = this.transform.position;
			
			float x = Mathf.Clamp( worldPos.x, minimum.x, maximum.x );
			float y = Mathf.Clamp( worldPos.y, minimum.y, maximum.y );
			
			this.transform.position = new Vector3( x, y, worldPos.z );
		}
	}
}