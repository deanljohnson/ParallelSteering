using System.Collections.Generic;
using SFML.System;

namespace ParallelSteering
{
	public class SteeringController
	{
		private readonly List<Boid> m_Boids;

		public SteeringController(List<Boid> boids)
		{
			m_Boids = boids;
		}

		public void Update()
		{
			for (int i = 0; i < m_Boids.Count; i++)
			{
				m_Boids[i].Velocity = Steering.Wander(m_Boids[i]) * m_Boids[i].MaxVelocity;
				//m_Boids[i].Velocity = Steering.Seek(m_Boids[i], new Vector2f(0,0));
			}
		}
	}
}
