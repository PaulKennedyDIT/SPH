using UnityEngine;
using System.Collections;
using System;

/* Poly 6 Smoothing Kernel
 * Contains a definition of the Poly6 Smoothing kernel including the Gradient and Laplacian of the Poly6 function.
 */
public class Poly6 : SmoothingKernel
{
	public Poly6(double SmoothingLength)
	{
		this.SmoothingLengthH = SmoothingLength;
	}

	//	As defined by Mathias Muller et al (2003) - poly6(r,h) = if 0 ≤ r ≤ h  then 315/64*π*(h^9) otherwise 0.
	//	where r = Square Magnitude of Distance Vector.
	// 		  h = Smoothing Length
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

	// Gradient as derived by Stefan Auer in the paper Realtime particle-based fluid simulation
	// ∇Poly6(r,h) = -r * (945/ 32 * π * h^9) * (h^2 - r^2)^2
	//				 where r = Square Magnitude of Distance Vector.
	//					   h = Smoothing Length
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

	// Laplacian as derived by Stefan Auer in the paper Realtime particle-based fluid simulation
	//	∇^2 Poly6(r,h) = (945/ 8 * π * h^9) * (h^2 - r^2) * ( r^2 - 3/4 * (h^2 - r^2))
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
