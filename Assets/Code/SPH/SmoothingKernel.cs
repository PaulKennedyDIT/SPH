using UnityEngine;
using System.Collections;
using System;

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
			CalculateFactor();
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
	
	protected abstract void CalculateFactor();
	
	public abstract double Calculate(ref Vector3 distance);
	
	public abstract Vector3 CalculateGradient(ref Vector3 distance);
	
	public abstract double CalculateLaplacian(ref Vector3 distance);
}
