using UnityEngine;
using System.Collections;

public class MenuSim : MonoBehaviour 
{
	public float NativeWidth = 1920;
	public float NativeHeight = 1080;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI () 
	{

		GUILayout.BeginArea(new Rect(10,20,Screen.width/2 * 0.4f,Screen.height));
		{
			GUILayout.BeginVertical("box"); // also can put width in here
			{
				if (GUILayout.Button ("Main Menu")) 
				{
					Application.LoadLevel(0);
				}

				GUILayout.Label("Viscosity:"+ Menu.Viscosity);
				Menu.Viscosity = GUILayout.HorizontalSlider(Menu.Viscosity, 0.0f, 10000.0f);

				GUILayout.Label("Particle size:"+ Menu.Size);
				Menu.Size = GUILayout.HorizontalSlider(Menu.Size, 0.2f, 1.0f);

				GUILayout.Label("Number of Particles:"+ Menu.ParticleResolution * Menu.ParticleResolution);
				Menu.ParticleResolution = (int)GUILayout.HorizontalSlider(Menu.ParticleResolution, 10, 22);
				
				if (GUILayout.Button("Apply Changes")) // also can put width here
				{
					Application.LoadLevel(1);
				}

				if (GUILayout.Button ("Default Values")) 
				{
					Menu.ParticleResolution = 19;
					Menu.Viscosity = 1.002f;
					Menu.Size = 1.0f;
				}
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
}
