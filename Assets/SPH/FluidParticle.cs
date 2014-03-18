using UnityEngine;
using System.Collections;
using System;

public class FluidParticle  
{
	public float Mass;					// Mass Of Particle.
	public float Size;					// Size of Particle
	public Vector3 Position;			// Current Position of Particle.
	public Vector3 PositionOld;			// Previous Position of Particle.
	public Vector3 Velocity;			// Current Velocity of Particle in X,Y,Z direction.
	public Vector3 Force;				// Current Force being applied to Particle in X,Y,Z direction.
	public float Density;				// Density Of Particle.
	public float Pressure;				// Pressure of Particle.
	public float Viscosity;				// Viscosity of Fluid Particle.
	public Solver Solver;				// Integration Solver for Resolving particle position per time stamp.
	public float GasConstant;			// Gas Constant for the calculation of pressure.
	public float DensityOffSet;			// Rest Density as described by Desbrun.

	public FluidParticle()
	{
		Mass		= 1.0f;				// Default Mass of particle initialised.
		Size 		= 1.0f;				// Default Size of particle initialised.
		Viscosity 	= 1.002f;			// Default Viscosity Initialised.
		Position 	= Vector3.zero;		// Default Position of Fluid Particle Initalised to 0,0,0 origin.
		PositionOld = this.Position;	
		Velocity	= Vector3.zero;		// Default Velocity of Fluid Particle initialised to 0,0,0.
		Force 		= Vector3.zero;		// Default Force of Fluid Particle initialised to 0,0,0.
		Density 	= 1000.0f;			// Default Density of Fluid Particle initialised to 1000
		GasConstant = 8.3145f;			// Universal Gas Law Constant
		DensityOffSet = 100.0f;			// Rest Density


		/**	Fluid Particle Solver Initialised.
		* 	Solver integrates the position of a Fluid Particle based on the velocity and Force.
		* 	Integration integrates based on the current Position, Previous Position, Mass, and velocity of a Fluid particle per time delta.
		*/
		Solver = new Solver();			
		Solver.Dampening = Mathf.Epsilon;
	}

	/** Update Pressure Method
	 * Based on the Pressure calculation detailed by Desbrun.
	 * Formulae is p=k(ρ−ρ0)
	 */
	public void UpdatePressure()
	{
		Pressure = GasConstant * (Density - DensityOffSet);
	}

	/** Update Method
	 *  Integration integrates relative to the current Position, Previous Position, Mass, and velocity of a Fluid particle per time delta.
	 */
	public void Update(float dTime)
	{
		Solver.Solve (ref Position, ref PositionOld, ref Velocity, Force, Mass, dTime);
	}
}
