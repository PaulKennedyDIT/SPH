using UnityEngine;
using System.Collections;
using System;

public class Poly6 : SmoothingKernel
{
	private double lengthOfDistanceSQ;
	private double h2minusr2;
	private double scalar;

	public Poly6(double SmoothingLength)
	{
		this.SmoothingLengthH = SmoothingLength;
	}
	
	public override double Calculate(ref Vector3 distance)
	{
		this.Scaling = (315.0f/ (64.0f * Math.PI * (float)Math.Pow(SmoothingLength,9.0f)));
		lengthOfDistanceSQ = (float)Mathf.Pow(distance.sqrMagnitude,2.0f);
		
		if (lengthOfDistanceSQ >= SmoothingLengthSq || lengthOfDistanceSQ <= Mathf.Epsilon) 
		{
			return 0.0f;
		}

		 h2minusr2 = SmoothingLengthSq - lengthOfDistanceSQ;

		return Scaling * (h2minusr2 * h2minusr2 * h2minusr2);
	}
	
	public override Vector3 CalculateGradient (ref Vector3 distance)
	{
		this.Scaling = (945.0f / (32.0f * Mathf.PI * (float)Math.Pow (SmoothingLength, 9.0f)));
		lengthOfDistanceSQ = (float)Mathf.Pow(distance.sqrMagnitude,2.0f);

		if (lengthOfDistanceSQ > SmoothingLengthSq || lengthOfDistanceSQ <= Mathf.Epsilon) 
		{
			return new Vector3 (0.0f, 0.0f, 0.0f);
		}

		h2minusr2 = SmoothingLengthSq - lengthOfDistanceSQ;
		scalar = this.Scaling * (h2minusr2 * h2minusr2);

		return new Vector3(-distance.x * (float)scalar, -distance.y * (float)scalar,-distance.z * (float)scalar);
	}
	
	public override double CalculateLaplacian(ref Vector3 distance)
	{
		this.Scaling = (945.0f)/(8.0f * Math.PI * (float)Math.Pow(SmoothingLength,9.0f));
		lengthOfDistanceSQ = (float)Mathf.Pow(distance.sqrMagnitude,2.0f);

		if (lengthOfDistanceSQ > SmoothingLengthSq || lengthOfDistanceSQ <= Mathf.Epsilon) 
		{
			return 0.0f;
		}

		h2minusr2 = SmoothingLengthSq - lengthOfDistanceSQ;

		distance = Vector3.Scale (distance, distance);
		return (this.Scaling * h2minusr2 * (distance.sqrMagnitude - ((3.0f * (h2minusr2))/4.0f)));
	}
}
