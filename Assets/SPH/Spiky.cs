using UnityEngine;
using System.Collections;
using System;

public class Spiky : SmoothingKernel 
{
	private double lengthOfDistanceSQ;
	private double epsilon;
	private double kernelR;
	private double diff;
	private double f;
	private double fac;
	private double len;

	public Spiky()
	{
		
	}
	
	public Spiky(double kernelSize)
	{
		this.kernelSize = kernelSize;
	}
	
	protected override void CalculateFactor()
	{
		kernelR = Math.Pow(kernelSize,6.0d);
		this.factor = (15.0f/ (Math.PI * kernelR));
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
		
		if(lengthOfDistanceSQ < epsilon)
		{
			lengthOfDistanceSQ = epsilon;
		}
		
		f = kernelSize - Math.Sqrt (lengthOfDistanceSQ);
		return factor * f * f * f;
	}
	
	public override Vector3 CalculateGradient (ref Vector3 distance)
	{
		// Mathf Sqrt(Vector3.Dot(v,v));
		double lengthOfDistanceSQ = distance.sqrMagnitude;
		double epsilon = Mathf.Epsilon;
				
		if (lengthOfDistanceSQ > kernelSizePow2) 
		{
			return new Vector3 (0.0f, 0.0f, 0.0f);
		}
		
		if(lengthOfDistanceSQ < epsilon)
		{
			lengthOfDistanceSQ = epsilon;
		}
		
		len = Math.Sqrt (lengthOfDistanceSQ);
		fac = -this.factor * 3.0f * (this.kernelSize - len) * (this.kernelSize - len) / len;

		return new Vector3 (distance.x * (float)fac, distance.y * (float)fac , distance.z * (float)fac);
	}
	
	public override double CalculateLaplacian(ref Vector3 distance)
	{
		throw new NotImplementedException();
	}
}
