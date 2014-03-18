using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SPH
{
	public List<FluidParticle> particleList;
	
	public SmoothingKernel SKPoly6;
	public SmoothingKernel SKPressure;
	public SmoothingKernel SKViscosity;
	
	public FluidParticle fp;
	public Vector3 f;
	public Vector3 dist;

	public float increment;
	public float minDist;
	public float minDistSq;
	public float diff;
	public float scalar;
	public float distLenSq;
	public float distLen;

	public SPH () 
	{

		particleList = new List<FluidParticle> ();
		fp = new FluidParticle ();
		SKPoly6 = new Poly6 (fp.Size);
		SKPressure = new Spiky (fp.Size);
		SKViscosity = new Viscosity (fp.Size);
	
		minDist = Menu.Size * 1.0f;
		minDistSq = minDist * minDist;
	}

	public void ConceptualFluidParticle(Vector3 pointPosition)
	{
		fp = new FluidParticle();
		fp.Position = pointPosition;
		particleList.Add(fp);
	}
	
	public void CalculateDensities(int index)
	{
		particleList[index].Density = 0.0f;
		particleList[index].Density += particleList[index].Mass * (float)this.SKPoly6.Calculate(ref dist);
	}
	
	public void CalculateSPHForces(int indexPart, int indexOther)
	{
		particleList [indexPart].Force = Vector3.zero;
		particleList [indexPart].Velocity = Vector3.zero;
		if(particleList[indexOther].Density > Mathf.Epsilon && particleList[indexPart] != particleList[indexOther])
		{
			distLen = Vector3.Distance(particleList[indexOther].Position,particleList[indexPart].Position);
			if(distLen < (Menu.Size * 1.0f))
			{
				// Calculate pressure forces
				scalar = particleList[indexOther].Mass * (particleList[indexPart].Pressure + particleList[indexOther].Pressure) / (2.0f * particleList[indexOther].Density);
				f = SKPressure.CalculateGradient(ref dist);
				f = f * scalar;
				particleList[indexPart].Force -= f;
				particleList[indexOther].Force += f;
				
				// Calculate Viscosity based forces
				scalar   = particleList[indexOther].Mass * (float)this.SKViscosity.CalculateLaplacian(ref dist) * Menu.Viscosity * 1.0f / particleList[indexOther].Density;
				f = particleList[indexOther].Velocity - particleList[indexPart].Velocity;
				f = f * scalar;
				particleList[indexPart].Force += f;
				particleList[indexOther].Force -= f;
			}
		}
	}
	
	public void CalculateExternalForces(int indexPart, int indexOther)
	{
		if(particleList[indexPart] != particleList[indexOther])
		{
			dist = particleList[indexOther].Position - particleList[indexPart].Position;
			distLenSq = dist.sqrMagnitude;
			
			if(distLenSq < minDistSq)
			{
				if(distLenSq > Mathf.Epsilon)
				{
					distLen = (float)Math.Sqrt((double)distLenSq);
					dist = dist * 0.5f * (distLen - minDist)/distLen;

					particleList[indexOther].Position = particleList[indexOther].Position - dist;
					particleList[indexOther].PositionOld = particleList[indexOther].PositionOld - dist;

					particleList[indexPart].Position = particleList[indexPart].Position + dist;
					particleList[indexPart].PositionOld = particleList[indexPart].PositionOld + dist;
				}
				else
				{
					diff = 0.5f * minDist;
					particleList[indexOther].Position.x -= diff;
					particleList[indexOther].Position.y -= diff;
					particleList[indexOther].Position.z -= diff;

					particleList[indexPart].Position.x  += diff;
					particleList[indexPart].PositionOld.y += diff;
					particleList[indexPart].PositionOld.z += diff;
				}
			}
		}
	}	
}
