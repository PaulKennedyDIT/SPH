using UnityEngine;
using System.Collections;

/* GameManager implemented as a Singleton instance.
*  Contains globally accessible variables including the Particle Resolution (Particle Number) and the Particle Size.
*  Retains the assignment of the GameManagers fields throughout all of the scenes loaded at run time.
*/
public class GameManager : MonoBehaviour
{
	public static GameManager manager;

	public int ParticleResolution = 15;
	public float ParticleSize = 1.0f;

	void Awake()
	{
		if(manager != null)
		{
			GameObject.Destroy(manager);
		}
		else
		{
			manager = this;
		}
		DontDestroyOnLoad (this);
	}
}
