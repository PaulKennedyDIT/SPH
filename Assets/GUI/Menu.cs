using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public static float Viscosity;
	public static float Size;
	public static int ParticleResolution;
	
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
			DisplayMainMenu (new Rect (Screen.width / 1.4f * 0.4f, Screen.height / 2.0f, Screen.width / 1.1f * 0.4f, Screen.height));
		}

		if(Application.loadedLevel == 1 || Application.loadedLevel == 2 || Application.loadedLevel == 3 || Application.loadedLevel == 4 || Application.loadedLevel == 5 || Application.loadedLevel == 6)
		{
			DisplaySimulationMenu(new Rect(10,20,Screen.width/1.1f * 0.4f,Screen.height/2f));
		}
	}

	void DisplayMainMenu(Rect ScreenSize)
	{
		GUILayout.BeginArea(ScreenSize);
		{
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Begin Test 1")) 
				{
					Application.LoadLevel(1);
				}

				if (GUILayout.Button("Begin Test 2")) 
				{
					Application.LoadLevel(2);
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Begin Test 3"))
				{
					Application.LoadLevel(3);
				}

				if (GUILayout.Button("Begin Test 4"))
				{
					Application.LoadLevel(4);
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Begin Test 5"))
				{
					Application.LoadLevel(5);
				}

				if (GUILayout.Button("Begin Test 6"))
				{
					Application.LoadLevel(6);
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginVertical("box"); 
			{	
				GUILayout.Label("Number of Particles:"+ ParticleResolution * ParticleResolution);
				ParticleResolution = (int)GUILayout.HorizontalSlider(ParticleResolution, 10, 22);
			}
			GUILayout.EndVertical();

			GUILayout.BeginHorizontal();
			{
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
				if(GUILayout.Button ("Toggle Show FPS"))
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
				GUILayout.Label("Number of Particles:"+ ParticleResolution * ParticleResolution);
				ParticleResolution = (int)GUILayout.HorizontalSlider(ParticleResolution, 10, 22);
			}
			GUILayout.EndVertical();
			
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Reload Particles")) 
				{
					Application.LoadLevel(Application.loadedLevel);
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();

	}
}
