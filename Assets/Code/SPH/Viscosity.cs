using UnityEngine;
using System.Collections;
using System;

public class Viscosity : SmoothingKernel 
{
	public Viscosity(double kernelSize)
	{
		this.SmoothingLengthH = kernelSize;
	}
	
	public override double Calculate(ref Vector3 distance)
	{
		this.Scaling = (15.0f/ (2.0f * Mathf.PI * SmoothingLengthCb));

		lengthOfDistance = distance.sqrMagnitude;
		lengthOfDistanceSQ = Math.Pow (distance.sqrMagnitude, 2.0d);
		lengthOfDistanceCB = Math.Pow (distance.sqrMagnitude, 3.0d);
		
		if (lengthOfDistance > SmoothingLength || lengthOfDistance < Mathf.Epsilon) 
		{
			return 0.0d;
		}

		scalar = -(lengthOfDistanceCB/(2 * SmoothingLengthCb)) + (lengthOfDistanceSQ/SmoothingLengthSq) + (SmoothingLength/ 2.0f * lengthOfDistance) - 1;
		return (this.Scaling * scalar);

	}
	
	public override Vector3 CalculateGradient (ref Vector3 distance)
	{
		this.Scaling = (15.0f/2.0f * Math.PI * SmoothingLengthCb);

		lengthOfDistance = distance.sqrMagnitude;
		lengthOfDistanceSQ = Math.Pow (distance.sqrMagnitude, 2.0d);
		lengthOfDistanceCB = Math.Pow (distance.sqrMagnitude, 3.0d);

		if (lengthOfDistance > SmoothingLength || lengthOfDistanceSQ <= Mathf.Epsilon) 
		{
			return new Vector3 (0.0f, 0.0f, 0.0f);
		}

		scalar = -((3 * lengthOfDistance)/(2.0f * SmoothingLengthCb)) + (2.0f/SmoothingLengthSq) - (SmoothingLength/(2.0f * lengthOfDistanceCB));
		this.Scaling = this.Scaling * scalar;

		return  new Vector3 (distance.x * (float)this.Scaling, distance.y * (float)this.Scaling, distance.z * (float)this.Scaling);
	}
	
	public override double CalculateLaplacian (ref Vector3 distance)
	{
		this.Scaling = (15.0f/(Math.PI * (float)Math.Pow(SmoothingLength,5.0d)));
		lengthOfDistance= distance.sqrMagnitude;

		if (lengthOfDistance > SmoothingLength || lengthOfDistance < Mathf.Epsilon) 
		{
			return 0.0d;
		}

		scalar = (1 - (lengthOfDistance/SmoothingLength));
		return (Scaling * scalar);
	}
}
