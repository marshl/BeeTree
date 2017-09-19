using UnityEngine;
using System.Collections;

/// <summary>
/// Used to make the camera fade in and out during scene transitions
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Interface/Camera Fade")]
public class btCameraFade : MonoBehaviour 
{
	public Texture texture;
	public enum STATE
	{
		IDLE,
		FADE_IN,
		FADE_OUT,
		DONE,
	};
	
	public STATE state;
	private float alpha = 0.0f;
	private float time = 0.0f;
	
    /// <summary>
    /// Loads up a black square to be used as the texture backing
    /// </summary>
	private void Awake()
	{
		this.texture = Resources.Load("black_square") as Texture;
		Assert.IsNotNull( this.texture, "Did not load image" );
	}
	
    /// <summary>
    /// Fades in over the given number of seconds
    /// </summary>
    /// <param name="_seconds">The time it will take to fade in</param>
	public void FadeIn( float _seconds )
	{
		this.alpha = 1.0f;
		this.state = STATE.FADE_IN;
		this.time = _seconds;
	}

    /// summary>
    /// Fades out over the given number of seconds
    /// </summary>
    /// <param name="_seconds">The time it will take to fade out</param>
	public void FadeOut( float _seconds )
	{
		this.alpha = 0.0f;
		this.state = STATE.FADE_OUT;
		this.time = _seconds;
	}
	
	private void Update()
	{
		if ( this.state == STATE.FADE_IN )
		{
			this.alpha -= Time.deltaTime / this.time;
			this.state = this.alpha <= 0.0f ? STATE.DONE : STATE.FADE_IN;
		}
		else if ( this.state == STATE.FADE_OUT )
		{
			this.alpha += Time.deltaTime / this.time;
			this.state = this.alpha >= 1.0f ? STATE.DONE : STATE.FADE_OUT;
		}
	}
	
	void OnGUI()
	{
		if ( this.state != STATE.IDLE )
		{
			this.alpha = Mathf.Clamp( this.alpha, 0.0f, 1.0f );
			GUI.color = new Color( 0.0f, 0.0f, 0.0f, this.alpha );
			GUI.DrawTexture( new Rect(0.0f, 0.0f, Screen.width, Screen.height), texture );
		}
	}
}
