using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SPH
{
	public List<FluidParticle> particleList;					// Collection of Conceptual Fluid Particles representative of each Fluid Particle visually represented in the scene..
	
	public SmoothingKernel Poly6;								// Object representative of a Smoothing Kernel. Implementation encompasses the Poly6 smoothing Kernel suggested by Muller et all (2003)
	public SmoothingKernel Spiky;								// Object representative of a Smoothing Kernel. Implementation encompasses the Spiky smoothing kernel devised by Hoover, et al. (1994)
	public SmoothingKernel Viscosity;							// Object representative of a Smoothing Kernel. Implementation encompasses the Viscosity smoothing Kernel suggested by Muller et all (2003)
	
	public FluidParticle fp;									// Instantiation of a Fluid Particle Object. Conceptual representation for a fluid Particle in the system. Used for computation of SPH equations.

	public Vector3 SPHForce;									// Vector representative of the repulsive Pressure and Viscosity forces computed as part of the Smooth Particle Hydrodynamics rule.
	public Vector3 dist;										// Vector which represents the distance between two Fluid particles. Position of Fluid particle i minus the position of Fluid Particle j.  		
	
	public float maxDist;										// Floating point value representative of the maximum distance between Particle i and j at which the calculation of Pressure and Viscosity repulisive and attractive forces are calculated.
	public float maxDistSq;										// Squared maximum distance value.
	public float diff;
	public float scalar;
	public float distLenSq;										
	public float distLen;										// Represents the length of the distance vector. E.g the Magnitude of the vector dist.

	public SPH () 
	{

		particleList = new List<FluidParticle> ();
		fp = new FluidParticle ();
		Poly6 = new Poly6 (fp.Size);
		Spiky = new Spiky (fp.Size);
		Viscosity = new Viscosity (fp.Size);
	
		maxDist = Menu.Size * 1.0f;
		maxDistSq = maxDist * maxDist;
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
		particleList[index].Density += particleList[index].Mass * (float)this.Poly6.Calculate(ref dist);
	}
	
	public void CalculateSPHForces(int indexPart, int indexOther)
	{
		particleList [indexPart].Force = Vector3.zero;
		particleList [indexPart].Velocity = Vector3.zero;
		if(particleList[indexOther].Density > Mathf.Epsilon && particleList[indexPart] != particleList[indexOther])
		{
			distLen = Vector3.Distance(particleList[indexOther].Position,particleList[indexPart].Position);
			if(distLen < maxDist)
			{
				// Calculate pressure forces
				scalar = particleList[indexOther].Mass * (particleList[indexPart].Pressure + particleList[indexOther].Pressure) / (2.0f * particleList[indexOther].Density);
				SPHForce = Spiky.CalculateGradient(ref dist);
				SPHForce = SPHForce * scalar;
				particleList[indexPart].Force -= SPHForce;
				particleList[indexOther].Force += SPHForce;
				
				// Calculate Viscosity based forces
				scalar   = particleList[indexOther].Mass * (float)this.Viscosity.CalculateLaplacian(ref dist) * Menu.Viscosity * 1.0f / particleList[indexOther].Density;
				SPHForce = particleList[indexOther].Velocity - particleList[indexPart].Velocity;
				SPHForce = SPHForce * scalar;
				particleList[indexPart].Force += SPHForce;
				particleList[indexOther].Force -= SPHForce;
			}
		}
	}
	
	public void CalculateExternalForces(int indexPart, int indexOther)
	{
		if(particleList[indexPart] != particleList[indexOther])
		{
			dist = particleList[indexOther].Position - particleList[indexPart].Position;
			distLenSq = dist.sqrMagnitude;
			
			if(distLenSq < maxDistSq)
			{
				if(distLenSq > Mathf.Epsilon)
				{
					distLen = (float)Math.Sqrt((double)distLenSq);
					dist = dist * 0.5f * (distLen - maxDist)/distLen;

					particleList[indexOther].Position = particleList[indexOther].Position - dist;
					particleList[indexOther].PositionOld = particleList[indexOther].PositionOld - dist;

					particleList[indexPart].Position = particleList[indexPart].Position + dist;
					particleList[indexPart].PositionOld = particleList[indexPart].PositionOld + dist;
				}
				else
				{
					diff = 0.5f * maxDist;
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
