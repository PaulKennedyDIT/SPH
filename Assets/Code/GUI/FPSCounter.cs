using UnityEngine;
using System.Collections;
/*** Externally Sourced Frames Per Second Display.
 * 	Source material for implementation located at: http://wiki.unity3d.com/index.php?title=FramesPerSecond
 *  Implementation is utilised to display the Frames Per Second of the application to the user upon run time.
 */
public class FPSCounter : MonoBehaviour 
{

	public  float		updateInterval;
	private float 		accum;   		// FPS accumulated over the interval
	private float 		timeleft; 		// Left time for current interval
	private int   		frames;			// Frames drawn over the interval
	public static bool  toggleFPS; 		// Toggle the drawing of the FPS label.
	private string 		label; 			// output string
	
	void Start () 
	{
		updateInterval 	= 0.5F;
		accum   		= 0.0f; 		
		timeleft 		= 0.0f;			
		frames 			= 0; 		
		toggleFPS 		= true;
	}

	void Update () 
	{
		FrameRatePerSecond ();
	}

	public void OnGUI() 
	{
		if(toggleFPS)
		{
			GUI.Label(new Rect(11,0,80,20), label);
		}
	}

	public void FrameRatePerSecond()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;

		if( timeleft <= 0.0 )
		{
			float fps = accum/frames;
			string format = System.String.Format("{0:F2} FPS",fps);
			label = format;
			
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}
