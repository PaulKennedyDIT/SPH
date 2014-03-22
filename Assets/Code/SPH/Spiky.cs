using UnityEngine;
using System.Collections;
using System;

/* Spiky Smoothing Kernel
 * Contains a definition of the Spiky Smoothing kernel including the Gradient and Laplacian of the Spiky function.
 */
public class Spiky : SmoothingKernel 
{
	
	public Spiky(double SmoothingLength)
	{
		this.SmoothingLengthH = SmoothingLength;
	}

	//	As defined by Debrun and utilised by Mathias Muller et al (2003) - Spiky(r,h) = 15/π*^h6 * (h - r)^3  if 0 <= r <= h then otherwise 0.
	//	where r = Square Magnitude of Distance Vector.
	// 		  h = Smoothing Length
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

	// Gradient as derived by Stefan Auer in the paper Realtime particle-based fluid simulation
	// ∇Spiky(r,h) = -r * (45/π * h^6 * r) * (h - r)^2
	//				 where r = Square Magnitude of Distance Vector.
	//					   h = Smoothing Length
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

	// Laplacian of Spiky function not implemented. Usage not required.
	public override double CalculateLaplacian(ref Vector3 distance)
	{
		throw new NotImplementedException();
	}
}
