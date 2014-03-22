using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager manager;

	public int ParticleResolution =22;

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
