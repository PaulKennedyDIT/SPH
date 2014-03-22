using UnityEngine;
using System.Collections;
using System;

public class Spiky : SmoothingKernel 
{
	private double lengthOfDistanceSQ;
	private double kernelR;
	private double diff;
	private double f;
	private double fac;
	private double len;

	public Spiky()
	{
		
	}
	
	public Spiky(double SmoothingLength)
	{
		this.SmoothingLengthH = SmoothingLength;
	}
	
	protected override void CalculateFactor()
	{
		kernelR = Math.Pow(SmoothingLength,6.0d);
		this.Scaling = (15.0f/ (Math.PI * kernelR));
	}
	
	public override double Calculate(ref Vector3 distance)
	{
		// Mathf Sqrt(Vector3.Dot(v,v));
		lengthOfDistanceSQ = distance.sqrMagnitude;

		if (lengthOfDistanceSQ > SmoothingLengthSq) 
		{
			return 0.0f;
		}
		
		if(lengthOfDistanceSQ < Mathf.Epsilon)
		{
			lengthOfDistanceSQ = Mathf.Epsilon;
		}
		
		f = SmoothingLength - Math.Sqrt (lengthOfDistanceSQ);
		return Scaling * f * f * f;
	}
	
	public override Vector3 CalculateGradient (ref Vector3 distance)
	{
		// Mathf Sqrt(Vector3.Dot(v,v));
		double lengthOfDistanceSQ = distance.sqrMagnitude;
				
		if (lengthOfDistanceSQ > SmoothingLengthSq) 
		{
			return new Vector3 (0.0f, 0.0f, 0.0f);
		}
		
		if(lengthOfDistanceSQ < Mathf.Epsilon)
		{
			lengthOfDistanceSQ = Mathf.Epsilon;
		}
		
		len = Math.Sqrt (lengthOfDistanceSQ);
		fac = -this.Scaling * 3.0f * (this.SmoothingLength - len) * (this.SmoothingLength - len) / len;

		return new Vector3 (distance.x * (float)fac, distance.y * (float)fac , distance.z * (float)fac);
	}
	
	public override double CalculateLaplacian(ref Vector3 distance)
	{
		throw new NotImplementedException();
	}
}
