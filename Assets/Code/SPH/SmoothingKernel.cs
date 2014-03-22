using UnityEngine;
using System.Collections;
using System;

/** Object representative of SPH Smoothing Kernel
 * Contains an abstract suite of methods common to Smoothing Kernels.
 * 
 */
public abstract class SmoothingKernel 
{
	protected double Scaling;
	protected double SmoothingLength;
	protected double SmoothingLengthSq;
	protected double SmoothingLengthCb;
	
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
		Scaling = 1.0d;
		SmoothingLength = 1.0d;
	}
	
	public SmoothingKernel(float SmoothingLength)
	{
		Scaling = 1.0d;
		this.SmoothingLengthH = SmoothingLength;
	}
	
	public abstract double Calculate(ref Vector3 distance);
	
	public abstract Vector3 CalculateGradient(ref Vector3 distance);
	
	public abstract double CalculateLaplacian(ref Vector3 distance);
}
