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
	public Vector3 SPHVelocity;									// Vector which is contains the Subtraction of Velocity values for the calculation of the the Viscosity SPH Rule.

	public float maxDist;										// Floating point value representative of the maximum distance between Particle i and j at which the calculation of Pressure and Viscosity repulisive and attractive forces are calculated.
	public float maxDistSq;										// Squared maximum distance value.
	public float scalar;										// Value containing the scalar multiplier for the SPHForce vector.
	public float SPHPressureSum;								// Contains the Summation of Pressure values from the SPH rules.
	public float distLen;										// Represents the length of the distance vector. E.g the Magnitude of the vector dist.

	public SPH () 
	{

		particleList = new List<FluidParticle> ();				// Instantiation of the FluidParticle list. Contains all of the Conceptual Fluid Particles within the scene.
		fp = new FluidParticle ();								// Object representative of each instance of a Fluid Particle.
		Poly6 = new Poly6 (1.0);								// Instantiation of a Poly6 smoothing Kernel with a smoothing distance of 1.0
		Spiky = new Spiky (1.0);								// Instantiation of a Spiky smoothing Kernel with a smoothing distance of 1.0
		Viscosity = new Viscosity (1.0);						// Instantiation of a Viscosity smoothing Kernel with a smoothing distance 1.0.
	
		maxDist = fp.Size * 0.5f;								// Maximum distance set to Half the size of a Fluid Particle.
		maxDistSq = maxDist * maxDist;							// Maximum distance squared.
	}

	/**	Conceptual Fluid Particle Method
	 * 	Creates a Conceptual Fluid Particle.
	 * 	Method creates a Conceptual Fluid Particle at a specified Position Vector location.
	 */ 
	public void ConceptualFluidParticle(Vector3 Position)
	{
		fp = new FluidParticle();
		fp.Position = Position;
		particleList.Add(fp);
	}


	/** Calculate Densities Method.
	 * Computes Density through the summation of the Mass * Poly6 Smoothing Kernel.
	 */ 
	public void CalculateDensities(int index)
	{
		particleList[index].Density = 0.0f;
		particleList[index].Density += particleList[index].Mass * (float)this.Poly6.Calculate(ref dist);
	}

	/** Calculate SPH Forces Method
	 * 	Calculates the Pressure and Viscosity forces which apply attractive and repulsive forces on each Fluid Particle.
	 *  SPH Equations Utilised:
	 * f(pressure)i =−∑j(mj * (pi+pj)/(2ρj) * ∇W(ri−rj,h)).
	 * 
	 * f(viscosity)i =µ∑j(mj * (vj−vi)/ρj * ∇2 *W(ri−rj,h).
	 * 
	 * Algorithm Fundamentals:
	 * Check if the Density of the fluid particle i is greater than Episilon. 
	 * And
	 * Check if the Fluid Particle i is not equal to the Fluid Particle J
	 * 
	 * Calculate the length of the Distance Vector between the Fluid Particle I's Position and the Fluid Particle J's Position.
	 * If the Length of the Distance vector is less than an assigned Max Distance Calculate the SPH Repulsive and attractive forces through the Pressure and Viscosity Rules.
	 */
	public void CalculateSPHForces(int indexPart, int indexOther)
	{
		if(particleList[indexOther].Density > Mathf.Epsilon && particleList[indexPart] != particleList[indexOther])
		{
			dist = particleList[indexPart].Position - particleList[indexOther].Position;
			distLen = dist.sqrMagnitude;
			if(distLen < maxDist)
			{
				// Calculate pressure forces
				SPHForce = CalculateSPHPressure(particleList[indexOther].Mass,particleList[indexPart].Pressure,particleList[indexOther].Pressure,particleList[indexOther].Density,ref dist);
				particleList[indexPart].Force -= SPHForce;
				particleList[indexOther].Force -= SPHForce;
				
				// Calculate Viscosity based forces
				SPHForce = CalculateSPHViscosity(particleList[indexOther].Mass,particleList[indexPart].Velocity,particleList[indexOther].Velocity,particleList[indexOther].Density,ref dist);
				particleList[indexPart].Force += SPHForce;
				particleList[indexOther].Force += SPHForce;
			}
		}
	}

	/**	CalculateSPHPressure Method
	 *  Implementation of the SPH Pressure Rule as defined by Mathias Muller in the paper Particle-Based Fluid Simulation for Interactive Applications (2003).
	 * 	f(pressure)i =−∑j(mj * (pi+pj)/(2ρj) * ∇W(ri−rj,h)).
	 * 					  Mass * PressureSum * Smoothing Kernel Gradient
	 */ 
	public Vector3 CalculateSPHPressure(float Mass, float IPressure,float JPressure,float Density, ref Vector3 distance)
	{
		SPHPressureSum = (IPressure + JPressure)/ (2.0f * Density);
		SPHForce = Spiky.CalculateGradient (ref distance);
		scalar = Mass * SPHPressureSum;
		SPHForce = SPHForce * scalar;

		return SPHForce;
	}

	/** CalculateSPHViscosity Method
	 * 
	 * f(viscosity)i =µ∑j(mj * (vj−vi)/ρj * ∇2 W(ri−rj,h)
	 * 						Mass * Velocity/Density * Smoothing Kernel Laplacian
	 */ 
	public Vector3 CalculateSPHViscosity(float Mass,Vector3 IVelocity, Vector3 JVelocity,float Density, ref Vector3 distance)
	{
		SPHVelocity = (JVelocity - IVelocity)/ (Density);
		SPHVelocity = SPHVelocity * Mass * (float)Viscosity.CalculateLaplacian (ref distance);
		SPHForce = Vector3.Scale(SPHForce,SPHVelocity);
		
		return SPHForce;
	}
}
