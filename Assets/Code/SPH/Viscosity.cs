using UnityEngine;
using System.Collections;
using System;

public class Viscosity : SmoothingKernel 
{
	private double lengthOfDistanceSQ;
	private double kernelR;
	private double diff;
	private double f;
	private double fac;
	private double len;
	private double len3;

	public Viscosity()
	{
		
	}
	
	public Viscosity(double kernelSize)
	{
		this.SmoothingLengthH = kernelSize;
	}
	
	protected override void CalculateFactor()
	{
		this.Scaling = (15.0f/ (2.0f * Mathf.PI * SmoothingLengthCb));
	}
	
	public override double Calculate(ref Vector3 distance)
	{
		lengthOfDistanceSQ = distance.sqrMagnitude;
		
		
		if (lengthOfDistanceSQ > SmoothingLengthSq) 
		{
			return 0.0d;
		}
		
		if(lengthOfDistanceSQ < Mathf.Epsilon)
		{
			lengthOfDistanceSQ = Mathf.Epsilon;
		}
		
		len = Math.Sqrt (lengthOfDistanceSQ);
		len3 = len * len * len;
		return Scaling * (((-len3 / (2.0f * SmoothingLength)) + (lengthOfDistanceSQ / SmoothingLengthSq) + (SmoothingLength / 2.0f * len))) - 1.0f;
	}
	
	public override Vector3 CalculateGradient (ref Vector3 distance)
	{
		throw new NotImplementedException();
	}
	
	public override double CalculateLaplacian (ref Vector3 distance)
	{
		lengthOfDistanceSQ = distance.sqrMagnitude;
		
		
		if (lengthOfDistanceSQ > SmoothingLengthSq) 
		{
			return 0.0d;
		}
		
		if(lengthOfDistanceSQ < Mathf.Epsilon)
		{
			lengthOfDistanceSQ = Mathf.Epsilon;
		}
		
		len = Math.Sqrt (lengthOfDistanceSQ);
		
		return Scaling * (6.0f / SmoothingLengthCb) * (SmoothingLength - len);
	}
}
