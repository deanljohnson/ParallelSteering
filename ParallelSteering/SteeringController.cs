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
				Vector2f vel = new Vector2f();
				vel += Steering.Wander(m_Boids[i]) * 50;
				vel += Steering.Align(m_Boids[i], m_Boids) * 5;
				vel += Steering.Cohesion(m_Boids[i], m_Boids);
				vel += Steering.Separation(m_Boids[i], m_Boids);
				m_Boids[i].Velocity = vel.Normalized() * m_Boids[i].MaxVelocity;
			}
		}
	}
}
