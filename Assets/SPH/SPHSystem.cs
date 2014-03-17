using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class SPHSystem : MonoBehaviour 
{
	public List<GameObject> spherelist = new List<GameObject> ();
	private GameObject sphere;
	private SPH sph;
	private FluidParticle fp;
	private float UpdateTime = 0.05f;

	public SPHSystem()
	{
		fp = new FluidParticle ();
		sph = new SPH ();
	}

	void Start () 
	{
		fp.Size = Menu.Size;
		fp.Viscosity = Menu.Viscosity;
		CreatePoints ();
		StartCoroutine ("DoCalculate");
	}

	private void CreatePoints () 
	{
		for (int x = 0; x < Menu.ParticleResolution; x++) 
		{
			for (int z = 0; z < Menu.ParticleResolution; z++) 
			{
				sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphere.AddComponent<Rigidbody>();
				sphere.transform.position = new Vector3((x + (transform.position.x - fp.Size * 2.0f)),transform.position.y, (z + (transform.position.z - fp.Size * 2.0f)));
				sphere.transform.localScale = new Vector3(Menu.Size,Menu.Size,Menu.Size);
				//sphere.renderer.sharedMaterial= Resources.Load("water", typeof(Material)) as Material;
				sphere.rigidbody.mass = fp.Mass;
				sphere.tag = "fluid";
				spherelist.Add(sphere);
				sph.CreateFluids(sphere.transform.position);
			}
		}
	}
	
	public void Calculate()
	{
		for (int k = 0; k < sph.particleList.Count; k++) 
		{
			sph.particleList [k].Position= spherelist[k].transform.position;
			sph.particleList [k].Update (UpdateTime);

			sph.CalculateDensities (k);
			sph.particleList [k].UpdatePressure ();
			for (int n =0; n < sph.particleList.Count; n++) 
			{
				sph.particleList [n].Position= spherelist[n].transform.position;
				sph.dist = sph.particleList[k].Position - sph.particleList[n].Position;
				sph.distLen = Vector3.Distance(sph.particleList[k].Position,sph.particleList[n].Position);

				if(sph.distLen < sph.particleList[k].Size)
				{
					sph.CheckParticleDistance (k, n);
					sph.CalculateForces (k, n);

					spherelist[k].rigidbody.AddForce(sph.particleList[k].Force);
					spherelist[n].rigidbody.AddForce(sph.particleList[n].Force);
					spherelist[n].transform.position = sph.particleList [n].Position;
				}
			}
			spherelist[k].transform.position = sph.particleList [k].Position;
		}
	}

	IEnumerator DoCalculate()
	{
		for(;;)
		{
			Calculate();
			yield return new WaitForSeconds(UpdateTime);
		}
	}	
}
