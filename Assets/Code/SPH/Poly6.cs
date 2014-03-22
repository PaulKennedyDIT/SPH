using UnityEngine;
using System.Collections;
using System;

public class Poly6 : SmoothingKernel
{
	private double lengthOfDistanceSQ;
	private double kernelR;
	private double diff;
	private double fac;

	public Poly6()
	{
	}
	
	public Poly6(double SmoothingLength)
	{
		this.SmoothingLengthH = SmoothingLength;
	}
	
	protected override void CalculateFactor()
	{
		kernelR = Math.Pow(SmoothingLength,9.0d);
		this.Scaling = (315.0f/ (64.0f * Math.PI * kernelR));
	}
	
	public override double Calculate(ref Vector3 distance)
	{
		// Mathf Sqrt(Vector3.Dot(v,v));
		lengthOfDistanceSQ = distance.sqrMagnitude;
		
		if (lengthOfDistanceSQ > SmoothingLengthSq) 
		{
			return 0.0f;
		}
		
		if (lengthOfDistanceSQ < Mathf.Epsilon) 
		{
			lengthOfDistanceSQ = Mathf.Epsilon;
		}
		
		double diff = SmoothingLengthSq - lengthOfDistanceSQ;

		return Scaling * diff * diff * diff;
	}
	
	public override Vector3 CalculateGradient (ref Vector3 distance)
	{
		// Mathf Sqrt(Vector3.Dot(v,v));
		lengthOfDistanceSQ = distance.sqrMagnitude;

		if (lengthOfDistanceSQ > SmoothingLengthSq) 
		{
			return new Vector3 (0.0f, 0.0f, 0.0f);
		}
		
		if(lengthOfDistanceSQ < Mathf.Epsilon)
		{
			lengthOfDistanceSQ = Mathf.Epsilon;
		}
		
		diff = SmoothingLengthSq - lengthOfDistanceSQ;
		fac = -this.Scaling * 6.0d * diff * diff;

		return new Vector3(distance.x * (float)fac, distance.y * (float)fac,distance.z * (float)fac);
	}
	
	public override double CalculateLaplacian(ref Vector3 distance)
	{
		throw new NotImplementedException();
	}
}
