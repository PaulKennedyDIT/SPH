using UnityEngine;
using System.Collections;
using System;

public class Spiky : SmoothingKernel 
{
	
	public Spiky(double SmoothingLength)
	{
		this.SmoothingLengthH = SmoothingLength;
	}
	
	public override double Calculate(ref Vector3 distance)
	{
		this.Scaling = (15.0f/ (Math.PI * Math.Pow(SmoothingLength,6.0d)));
		lengthOfDistance = distance.sqrMagnitude;

		if (lengthOfDistance > SmoothingLengthSq || lengthOfDistance < Mathf.Epsilon) 
		{
			return 0.0f;
		}

		scalar = SmoothingLength - lengthOfDistance;
		return (this.Scaling * (scalar * scalar * scalar));
	}
	
	public override Vector3 CalculateGradient (ref Vector3 distance)
	{
		lengthOfDistance = distance.sqrMagnitude;
		this.Scaling = (45.0f/(Math.PI * (float)Math.Pow(SmoothingLength,6.0d) * lengthOfDistance));
	
		if (lengthOfDistance > SmoothingLengthSq || lengthOfDistance < Mathf.Epsilon) 
		{
			return new Vector3 (0.0f, 0.0f, 0.0f);
		}

		h2minusr2 = SmoothingLengthSq - lengthOfDistance;
		scalar = this.Scaling * (h2minusr2 * h2minusr2);

		return new Vector3 (-distance.x * (float)scalar, -distance.y * (float)scalar , -distance.z * (float)scalar);
	}
	
	public override double CalculateLaplacian(ref Vector3 distance)
	{
		throw new NotImplementedException();
	}
}
