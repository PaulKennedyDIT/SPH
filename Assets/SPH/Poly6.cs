using UnityEngine;
using System.Collections;
using System;

public class Poly6 : SmoothingKernel
{
	private double lengthOfDistanceSQ;
	private double epsilon;
	private double kernelR;
	private double diff;
	private double fac;

	public Poly6()
	{
	}
	
	public Poly6(double kernelSize)
	{
		this.kernelSize = kernelSize;
	}
	
	protected override void CalculateFactor()
	{
		kernelR = Math.Pow(kernelSize,9.0d);
		this.factor = (315.0f/ (64.0f * Math.PI * kernelR));
	}
	
	public override double Calculate(ref Vector3 distance)
	{
		// Mathf Sqrt(Vector3.Dot(v,v));
		lengthOfDistanceSQ = distance.sqrMagnitude;
		epsilon = Mathf.Epsilon;
		
		if (lengthOfDistanceSQ > kernelSizePow2) 
		{
			return 0.0f;
		}
		
		if (lengthOfDistanceSQ < epsilon) 
		{
			lengthOfDistanceSQ = epsilon;
		}
		
		double diff = kernelSizePow2 - lengthOfDistanceSQ;

		return factor * diff * diff * diff;
	}
	
	public override Vector3 CalculateGradient (ref Vector3 distance)
	{
		// Mathf Sqrt(Vector3.Dot(v,v));
		lengthOfDistanceSQ = distance.sqrMagnitude;
		epsilon = Mathf.Epsilon;

		if (lengthOfDistanceSQ > kernelSizePow2) 
		{
			return new Vector3 (0.0f, 0.0f, 0.0f);
		}
		
		if(lengthOfDistanceSQ < epsilon)
		{
			lengthOfDistanceSQ = epsilon;
		}
		
		diff = kernelSizePow2 - lengthOfDistanceSQ;
		fac = -this.factor * 6.0d * diff * diff;

		return new Vector3(distance.x * (float)fac, distance.y * (float)fac,distance.z * (float)fac);
	}
	
	public override double CalculateLaplacian(ref Vector3 distance)
	{
		throw new NotImplementedException();
	}
}
