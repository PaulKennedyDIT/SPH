using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour 
{

	public  float updateInterval;
	private float accum;   			// FPS accumulated over the interval
	private float timeleft; 		// Left time for current interval
	private int   frames;			// Frames drawn over the interval
	private bool displayFPS; 		// Toggle the drawing of the FPS label.
	private string fpsstr; 			// output string


	// Use this for initialization
	void Start () 
	{
		updateInterval 	= 0.5F;
		accum   		= 0.0f; 		
		timeleft 		= 0.0f;			
		frames 			= 0; 		
		displayFPS 		= true;
	}
	
	// Update is called once per frame
	void Update () 
	{

		FrameRatePerSecond ();

		if(Input.GetKeyDown("f"))
		{
			displayFPS = !displayFPS;
		}
	}

	public void OnGUI() 
	{
		if(displayFPS)
		{
			GUI.Label(new Rect(11,0,80,20), fpsstr);
		}
	}

	public void FrameRatePerSecond()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{
			// display two fractional digits (f2 format)
			float fps = accum/frames;
			string format = System.String.Format("{0:F2} FPS",fps);
			fpsstr = format;
			
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}
