using UnityEngine;
using System.Collections;
using System;

public abstract class SmoothingKernel 
{
	protected double factor;
	protected double kernelSize;
	protected double kernelSizePow2;
	protected double kernelSizePow3;
	
	public double KernelSize
	{
		get 
		{
			return kernelSize;
		}

		set
		{
			kernelSize = value;
			kernelSizePow2 = kernelSize * kernelSize;
			kernelSizePow3 = kernelSize * kernelSize * kernelSize;
			CalculateFactor();
		}
	}
	
	public SmoothingKernel()
	{
		factor = 1.0d;
		kernelSize = 1.0d;
	}
	
	public SmoothingKernel(float kernelSize)
	{
		factor = 1.0d;
		this.kernelSize = kernelSize;
	}
	
	protected abstract void CalculateFactor();
	
	public abstract double Calculate(ref Vector3 distance);
	
	public abstract Vector3 CalculateGradient(ref Vector3 distance);
	
	public abstract double CalculateLaplacian(ref Vector3 distance);
}
