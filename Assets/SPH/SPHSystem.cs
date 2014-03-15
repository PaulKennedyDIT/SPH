using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class SPHSystem : MonoBehaviour 
{
	private ParticleSystem.Particle[] points;
	private Boundary bounds;
	private SPH sph;
	private float increment;
	private FluidParticle fp;
	private float UpdateTime = 0.06f;
	private int i;

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

	void Update()
	{

	}
		
	private void CreatePoints () 
	{
		points = new ParticleSystem.Particle[Menu.ParticleResolution * Menu.ParticleResolution];
		increment = 1f / (Menu.ParticleResolution);
		i = 0;
		for (int x = 0; x < Menu.ParticleResolution; x++) 
		{
			for (int z = 0; z < Menu.ParticleResolution; z++) 
			{
				points[i].position = new Vector3(x * increment,transform.position.y, z * increment);
				points[i].size =Menu.Size;
				sph.CreateFluids(points[i].position);
				i++;
			}
		}
	}
	
	public void Calculate()
	{
		particleSystem.GetParticles (points);
		for (int k = 0; k < sph.particleList.Count; k++) 
		{
			sph.particleList [k].Update (UpdateTime);

			sph.CalculateDensities (k);
			sph.particleList [k].UpdatePressure ();

			for (int n =0; n < sph.particleList.Count; n++) 
			{
				sph.dist = sph.particleList[k].Position - sph.particleList[n].Position;
				sph.distLen = Vector3.Distance(sph.particleList[k].Position,sph.particleList[n].Position);

				if(sph.distLen < 0.25f)
				{
					sph.CheckParticleDistance (k, n);
					sph.CalculateForces (k, n);
					points[k].position = sph.particleList [k].Position;
					points[n].position = sph.particleList [n].Position;
				}
			}


			sph.bounds.Update(ref sph.particleList);
			sph.bounds.CheckContainerCollision (k, sph.particleList[k].Size);

			particleSystem.SetParticles (points, sph.particleList.Count);
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
