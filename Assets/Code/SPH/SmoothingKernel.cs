using UnityEngine;
using System.Collections;
using System;

/** Object representative of SPH Smoothing Kernel
 * Contains an abstract suite of methods common to Smoothing Kernels.
 */
public abstract class SmoothingKernel 
{
	protected double Scaling;				//Scalar value representative of 
	protected double SmoothingLength;
	protected double SmoothingLengthSq;
	protected double SmoothingLengthCb;

	protected double lengthOfDistance;
	protected double lengthOfDistanceSQ;
	protected double lengthOfDistanceCB;

	protected double scalar;
	protected double h2minusr2;

	/* Smoothing Length H
	 * Calculates the Squared and Cubed value based on the assigned Smoothing value h.
	 * 
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
