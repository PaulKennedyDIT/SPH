using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Boundary
{
	private ParticleSystem ps;
	public List<FluidParticle> list;
	private GameObject Ground,ContainerXL,ContainerXR,ContainerZF,ContainerZN,Top;

	public Boundary()
	{
		list = new List<FluidParticle> ();
	}

	public void Update(ref List<FluidParticle> ParticleList)
	{
		this.list = ParticleList;
		ContainerZF = GameObject.FindGameObjectWithTag ("containerZFar");
		ContainerZN = GameObject.FindGameObjectWithTag ("containerZNear");
		ContainerXL = GameObject.FindGameObjectWithTag ("containerXLeft");
		ContainerXR = GameObject.FindGameObjectWithTag ("containerXRight");
		Ground 		= GameObject.FindGameObjectWithTag ("ground");
		Top 		= GameObject.FindGameObjectWithTag ("top");
	}

	public void CheckContainerCollision(int index,float particleSize)
	{
		if(this.list[index].Position.x <= (ContainerXR.transform.position.x + particleSize))
		{
			this.list[index].Position.x = ContainerXR.transform.position.x + (particleSize);
		}
		
		if (list[index].Position.x >= ContainerXL.transform.position.x - particleSize) 
		{
			this.list[index].Position.x = ContainerXL.transform.position.x - particleSize;
		}

		if(this.list[index].Position.z <= ContainerZF.transform.position.z + particleSize)
		{
			this.list[index].Position.z = ContainerZF.transform.position.z + particleSize;
		}
		
		if (this.list[index].Position.z >= ContainerZN.transform.position.z - particleSize) 
		{
			this.list[index].Position.z = ContainerZN.transform.position.z - particleSize;
		}

		if (this.list[index].Position.y <= Ground.transform.position.y + (particleSize)) 
		{
			this.list[index].Position.y = Ground.transform.position.y + (particleSize);
		}

		if (this.list[index].Position.y >= Top.transform.position.y - (particleSize)) 
		{
			this.list[index].Position.y = Top.transform.position.y - (particleSize);
		}
	}
}
