using UnityEngine;
using System.Collections;
using System;

public class FluidParticle  
{
	public float Mass;				// Mass Of Particle.
	public float Size;				// Size of Particle
	public Vector3 Position;		// Current Position of Particle.
	public Vector3 PositionOld;		// Previous Position of Particle.
	public Vector3 Velocity;		// Current Velocity of Particle in X,Y,Z direction.
	public Vector3 Force;			// Current Force being applied to Particle in X,Y,Z direction.
	public float Density;			// Density Of Particle.
	public float Pressure;			// Pressure of Particle.
	public float Viscosity;			// Viscosity of Fluid Particle.
	public Solver Solver;			// Integration Solver for Resolving particle position per time stamp.

	public FluidParticle()
	{
		Mass		= 1.0f;
		Size 		= 1.0f;
		Viscosity 	= 1.002f;
		Position 	= Vector3.zero;
		PositionOld = this.Position;
		Velocity	= Vector3.zero;
		Force 		= Vector3.zero;
		Density 	= 1000.0f;
		Solver = new Solver();
		Solver.Dampening = Mathf.Epsilon;
	}
	
	public void UpdatePressure()
	{
		Pressure = 2.3f * (Density - 100.0f);
	}

	public void Update(float dTime)
	{
		Solver.Solve (ref Position, ref PositionOld, ref Velocity, Force, Mass, dTime);
	}
}
