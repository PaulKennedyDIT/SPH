using UnityEngine;
using System.Collections;

/* Menu Options
 * Usage: Attach to the Main Camera.
 * Setup involves the addition of Scenes 0 to 8 being added to the build path.
 */
public class Menu : MonoBehaviour 
{
	void OnGUI () 
	{
		// Displays the Main Menu
		if(Application.loadedLevel == 0)
		{
			DisplayMainMenu (new Rect (Screen.width / 1.4f * 0.4f, Screen.height / 2.0f, Screen.width / 1.1f * 0.4f, Screen.height));
		}

		// Display the Simulation Menu 
		if(Application.loadedLevel != 0)
		{
			DisplaySimulationMenu(new Rect(10,20,Screen.width/1.1f * 0.4f,Screen.height/2f));
		}
	}

	/** Main Menu Method
	 * Contains a Selection of Test Simulation Options to the user.
	 * Each test simulation demonstrates the visual cases in which the SPH implementation covers.
	 */
	void DisplayMainMenu(Rect ScreenSize)
	{
		GUILayout.BeginArea(ScreenSize);
		{
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Begin Test 1")) 
				{
					Application.LoadLevel(1);				// Load Scene 1 Test Case on mouse click.
				}

				if (GUILayout.Button("Begin Test 2")) 
				{
					Application.LoadLevel(2);				// Load Scene 2 Test Case on mouse click.
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Begin Test 3"))
				{
					Application.LoadLevel(3);				// Load Scene 3 Test Case on mouse click.
				}

				if (GUILayout.Button("Begin Test 4"))
				{
					Application.LoadLevel(4);				// Load Scene 4 Test Case on mouse click.
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Begin Test 5"))
				{
					Application.LoadLevel(5);				// Load Scene 5 Test Case on mouse click.
				}

				if (GUILayout.Button("Begin Test 6"))
				{
					Application.LoadLevel(6);				// Load Scene 6 Test Case on mouse click.
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Begin Test 7"))
				{
					GameManager.manager.ParticleSize = 0.4f;
					Application.LoadLevel(7);				// Load Scene 7 Test Case on mouse click.
				}
				
				if (GUILayout.Button("Begin Test 8"))
				{
					Application.LoadLevel(8);				// Load Scene 8 Test Case on mouse click.
				}
			}
			GUILayout.EndHorizontal();

			//	Horizontal Slider which allows for a user to select the number of Particles to be displayed in the scene
			GUILayout.BeginVertical("box"); 
			{	
				GUILayout.Label("Number of Particles:"+ GameManager.manager.ParticleResolution * GameManager.manager.ParticleResolution);
				GameManager.manager.ParticleResolution = (int)GUILayout.HorizontalSlider(GameManager.manager.ParticleResolution, 10, 22);
			}
			GUILayout.EndVertical();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button ("Exit")) 
				{
					Application.Quit();					// Closes the Application.
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
					FPSCounter.toggleFPS = !FPSCounter.toggleFPS;		// Toggles the boolean flag for displaying the label containing the Frames per second that the simulation is running at.
				}

				if (GUILayout.Button ("Main Menu")) 
				{
					GameManager.manager.ParticleSize = 1.0f;
					Application.LoadLevel(0);							// Returns the user to the Main Menu.
				}
			}
			GUILayout.EndHorizontal();

			//	Horizontal Slider which allows for a user to select the number of Particles to be displayed in the scene
			GUILayout.BeginVertical("box");
			{
				GUILayout.Label("Number of Particles:"+ GameManager.manager.ParticleResolution * GameManager.manager.ParticleResolution);
				GameManager.manager.ParticleResolution = (int)GUILayout.HorizontalSlider(GameManager.manager.ParticleResolution, 10, 22);
			}
			GUILayout.EndVertical();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Reload Particles")) 
				{
					Application.LoadLevel(Application.loadedLevel);		// Reloads the current scene.
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();

	}
}
