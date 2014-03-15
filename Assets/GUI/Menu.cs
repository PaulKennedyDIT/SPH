using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public static float Viscosity;
	public static float Size;
	public static int ParticleResolution;

	public float NativeWidth = 1920;
	public float NativeHeight = 1080;

	// Use this for initialization
	void Start () 
	{
		ParticleResolution = 19;
		Viscosity = 1.002f;
		Size = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI () 
	{
		GUILayout.BeginArea(new Rect(Screen.width * 0.4f,Screen.height/2,Screen.width/2 * 0.4f,Screen.height));
		{
			GUILayout.MinHeight(10.0f);
			GUILayout.MinWidth(10.0f);
			GUILayout.BeginVertical("box"); // also can put width in here
			{
				GUILayout.Label("Viscosity:"+ Viscosity);
				Viscosity = GUILayout.HorizontalSlider(Viscosity, 0.0f, 10000.0f);

				GUILayout.Label("Particle size:"+ Size);
				Size = GUILayout.HorizontalSlider(Size, 0.2f, 1.0f);

				GUILayout.Label("Number of Particles:"+ ParticleResolution * ParticleResolution);
				ParticleResolution = (int)GUILayout.HorizontalSlider(ParticleResolution, 10, 22);

				if (GUILayout.Button ("Default Values")) 
				{
					ParticleResolution = 19;
					Viscosity = 1.002f;
					Size = 1.0f;
				}

				if (GUILayout.Button("Start Simulation")) // also can put width here
				{
					Application.LoadLevel(1);
				}

				if (GUILayout.Button ("Exit")) 
				{
					Application.Quit();
				}

			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
}
