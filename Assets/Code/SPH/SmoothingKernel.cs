using UnityEngine;
using System.Collections;
using System;

/** Object representative of SPH Smoothing Kernel
 * 	Contains an abstract suite of methods common to Smoothing Kernels.
 * 	Smoothing Kernel provides an implementation of the Smoothing Kernel function as well as the Gradient and Laplacian of that Function.
 * 	The Smoothing Kernel's implemented include the Poly6 and Viscosity Smoothing Kernel as described by Mathias Muller et al (2003) in the paper Particle-Based Fluid Simulation for Interactive Applications.
 *  The Derivation of the Gradient and Laplacian of the smoothing kernels are based off the work of Stefan Auer in the paper Realtime particle-based fluid simulation 
 */
public abstract class SmoothingKernel 
{
	protected double Scaling;				// Scalar value representative of Kernel multiplier.
	protected double scalar;				// Temporary scalar value used in the calculation of the Kernels multiplier.

	protected double SmoothingLength;		// Magnitude of the Distance Vector.(r).
	protected double SmoothingLengthSq;		// Magnitude to the power of 2.		(r^2).
	protected double SmoothingLengthCb; 	// Magnitude to the power of 3. 	(r^3).

	protected double lengthOfDistance;		// Smoothing Kernel Length.					  (h)
	protected double lengthOfDistanceSQ;	// Smoothing Kernel Length to the power of 2. (h^2)
	protected double lengthOfDistanceCB;	// Smoothing Kernel Length to the power of 2. (h^3)


	protected double h2minusr2;				// Smoothing Kernel - Magntitude of the Distance vector. Used for (h-r)^x and (h^2 - h^2)^x calculations.

	/* Smoothing Length H
	 * Calculates the Squared and Cubed value based on the assigned Smoothing value h.
	 */
	public double SmoothingLengthH
	{
		get 
		{
			return SmoothingLength;
		}

		set
		{
			SmoothingLength = value;
			SmoothingLengthSq = SmoothingLength * SmoothingLength;
			SmoothingLengthCb = SmoothingLength * SmoothingLength * SmoothingLength;
		}
	}
	
	public SmoothingKernel()
	{
		SmoothingLength = 1.0d;
	}
	
	public SmoothingKernel(float SmoothingLength)
	{
		this.SmoothingLengthH = SmoothingLength;
	}
	
	public abstract double Calculate(ref Vector3 distance);
	
	public abstract Vector3 CalculateGradient(ref Vector3 distance);
	
	public abstract double CalculateLaplacian(ref Vector3 distance);
}
