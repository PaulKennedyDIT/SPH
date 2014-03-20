using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class SPHSystem : MonoBehaviour 
{
	public List<GameObject> drawParticleList = new List<GameObject> ();		// Collection of GameObjects used to visually represent each Fluid Particle.

	private GameObject drawParticle;										// GameObject which visually represents each individual Fluid Particle.	
	private SPH sph;														// Instantiation of Smooth Particle Hydrodynamic object. Performs the SPH calculations.
	private FluidParticle tempFluidParticle;								// Instantiation of a temporary Fluid Particle Object.
																			// Used to conceptually represent each Fluid Particle. 
																			// Used to determine assign the default values of each Fluid Particle to the Rigid Body visual representation of a fluid particle.
	private static float UpdateTime = 0.05f;								// Instantiation of the Time Delta used for Updating each particle per time slice.


	
	public SPHSystem()
	{
		tempFluidParticle = new FluidParticle ();
		sph = new SPH ();
	}

	void Start () 
	{

		tempFluidParticle.Size = Menu.Size;									// Size of Fluid Particle is assigned based on a value specified from the Menu object.
		tempFluidParticle.Viscosity = Menu.Viscosity;						// Default Viscosity is defined as 1.002 which is the Viscosity of water at a temperature of 20 degrees celcius.
		CreateFluidParticle ();												// Method which instansiates a Fluid Particles and assigns the conceptual location of each fluid particle to the visual layer concept.
		StartCoroutine ("CalculateSPH");									// Initialises a Coroutine to begin the calculation of the SPH forces and distance checking on a set time slice. Used as a performance optimisation.

	}

	/** Create Fluid Particle Method
	 * Method used to instansiate both the visual and concetual level objects which encapsulate a fluid particle.
	 * Creates a Sphere Primitive using the Unity Engine and positions each particle relative the transform position of the SPHSystem script.
	 * Scale and Mass of the rigid body is assigned from the Temporary Fluid Particle Object created.
	 */
	private void CreateFluidParticle() 
	{
		for (int x = 0; x < Menu.ParticleResolution; x++) 
		{
			for (int z = 0; z < Menu.ParticleResolution; z++) 
			{
				drawParticle = VisualFluidParticle(x,z);
				drawParticleList.Add(drawParticle);
				sph.ConceptualFluidParticle(drawParticle.transform.position);
			}
		}
	}

	private GameObject VisualFluidParticle(int loopindex1,int loopindex2)
	{
		drawParticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		drawParticle.AddComponent<Rigidbody>();
		drawParticle.transform.position = new Vector3((loopindex1 + (transform.position.x - tempFluidParticle.Size * 2.0f)),transform.position.y, (loopindex2 + (transform.position.z - tempFluidParticle.Size * 2.0f)));
		drawParticle.transform.localScale = new Vector3(Menu.Size,Menu.Size,Menu.Size);
		drawParticle.rigidbody.mass = tempFluidParticle.Mass;
		drawParticle.name = "Fluid Particle";
		drawParticle.tag = "fluid";

		return drawParticle;
	}

	/** Calculate Method
	 *  Directly used as a means of calculating the Smooth Particle Hydrodynamic equations as described by Mathias Muller.
	 * 	Algorithm fundamentals:
	 * 	Iterate over every particle (i) in a list of Fluid Particles.
	 *  Set the Visual Position of each Fluid Particle to be the Conceptal Position for each Particle i
	 *  Integrate the Particle (i) position to calculate specify the Previous Position of the Particle, the Velocity and Acceleration per time slice.
	 * 	Update the Pressure of the Particle (i).
	 *  Calculate the Density of the Particle(i).
	 * 
	 * 		Iterate over all of the other Particles in the Particle list (j).
	 * 		Set the Visual Position of each Fluid Particle (j) to be the Conceptual Position for each Particle (j).
	 * 		Calculate the distance between the position vectors of particle (j) and the particle (i).
	 * 		If the Distance between the two position vectors is less than or equal to the Size of a Particle Check
	 * 			Calculate SPH forces using by calculating Pressure and Viscosity based forces as described by Muller
	 * 		
	 * 			Add the applied to each Conceptual Fluid Particle i and j to the Visual Fluid Particle component.
	 * 			Adjust the Transform position of the fluid Particle i and j accordingly.
	 */ 
	public void Calculate()
	{
		for (int i = 0; i < sph.particleList.Count; i++) 
		{
			sph.particleList [i].Position= drawParticleList[i].transform.position;
			sph.particleList [i].Update (UpdateTime);

			sph.particleList [i].UpdatePressure ();
			sph.CalculateDensities (i);

			for (int j =0; j < sph.particleList.Count; j++) 
			{
				sph.particleList [j].Position= drawParticleList[j].transform.position;
				sph.distLen = Vector3.Distance(sph.particleList[i].Position,sph.particleList[j].Position);

				if(sph.distLen <= sph.particleList[i].Size)
				{
					sph.CalculateSPHForces (i, j);

					drawParticleList[i].rigidbody.AddForce(sph.particleList[i].Force);
					drawParticleList[j].rigidbody.AddForce(sph.particleList[j].Force);
					drawParticleList[j].transform.position = sph.particleList [j].Position;
				}
			}
			drawParticleList[i].transform.position = sph.particleList [i].Position;
		}
	}

	/** CalculateSPH Method
	 *  Coroutine which every X seconds where X is the defined constant UpdateTime.
	 *  In the current implementation, the Coroutine runs over 0.05 of a second.
	 *  Implemented as an optimisation method to avoid runing the Calculate Method every CPU cycle.
	 */ 
	IEnumerator CalculateSPH()
	{
		for(;;)
		{
			Calculate();
			yield return new WaitForSeconds(UpdateTime);
		}
	}	
}
