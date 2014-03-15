using UnityEngine;
using System.Collections;
using System;

public class Solver 
{
	
	public float Dampening;
	
	public Solver()
	{
		Dampening = 1.0f;
	}
	
	public void Solve(ref Vector3 position, ref Vector3 positionOld, ref Vector3 velocity, Vector3 force, float mass, float timeStep) 
	{
		Vector3 t;
		Vector3 oldPos = position;
		Vector3 acceleration = force / mass;

		acceleration = acceleration * (timeStep * timeStep);
		t = position - positionOld;
		t = t * (1.0f - Dampening);
		t = t + acceleration;
		position = position + t;
		positionOld = oldPos;
		t = position - positionOld;
		velocity = t / timeStep;
	}
}
