using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public static float Viscosity;
	public static float Size;
	public static int ParticleResolution;

	// Use this for initialization
	void Start () 
	{
		ParticleResolution = 19;
		Viscosity = 1.002f;
		Size = 1.0f;
	}
	void Update()
	{
	}

	void OnGUI () 
	{
		if(Application.loadedLevel == 0)
		{
			DisplayMainMenu (new Rect (Screen.width / 1.4f * 0.4f, Screen.height / 4.0f, Screen.width / 1.1f * 0.4f, Screen.height));
		}

		if(Application.loadedLevel == 1 || Application.loadedLevel == 2)
		{
			DisplaySimulationMenu(new Rect(10,20,Screen.width/1.1f * 0.4f,Screen.height/2f));
		}
	}

	void DisplayMainMenu(Rect ScreenSize)
	{
		GUILayout.BeginArea(ScreenSize);
		{
			GUILayout.BeginVertical("box"); // also can put width in here
			{	
				GUILayout.Label("Particle size:"+ Size);
				Size = GUILayout.HorizontalSlider(Size, 0.2f, 1.0f);
				
				GUILayout.Label("Number of Particles:"+ ParticleResolution * ParticleResolution);
				ParticleResolution = (int)GUILayout.HorizontalSlider(ParticleResolution, 10, 22);
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Simulation 1")) // also can put width here
				{
					Application.LoadLevel(1);
				}

				if (GUILayout.Button("Simulation 2")) // also can put width here
				{
					Application.LoadLevel(2);
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button ("Default Values")) 
				{
					ParticleResolution = 19;
					Viscosity = 1.002f;
					Size = 1.0f;
				}
				
				if (GUILayout.Button ("Exit")) 
				{
					Application.Quit();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}

	void DisplaySimulationMenu(Rect ScreenSize)
	{
		GUILayout.BeginArea(ScreenSize);
		{
			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button ("Show FPS"))
				{
					FPSCounter.toggleFPS = !FPSCounter.toggleFPS;
				}
				
				if (GUILayout.Button ("Main Menu")) 
				{
					Application.LoadLevel(0);
				}
			}
			GUILayout.EndHorizontal();
			
			GUILayout.BeginVertical("box");
			{
				GUILayout.Label("Particle size:"+ Menu.Size);
				Menu.Size = GUILayout.HorizontalSlider(Menu.Size, 0.2f, 1.0f);
				
				GUILayout.Label("Number of Particles:"+ Menu.ParticleResolution * Menu.ParticleResolution);
				Menu.ParticleResolution = (int)GUILayout.HorizontalSlider(Menu.ParticleResolution, 10, 22);
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Apply Changes")) 
				{
					Application.LoadLevel(Application.loadedLevel);
				}
				
				if (GUILayout.Button ("Default Values")) 
				{
					Menu.ParticleResolution = 19;
					Menu.Viscosity = 1.002f;
					Menu.Size = 1.0f;
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();

	}
}
