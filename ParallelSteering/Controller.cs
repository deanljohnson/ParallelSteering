using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ParallelSteering
{
	public class Controller
	{
		private readonly List<Boid> m_Boids = new List<Boid>();

		private readonly RenderingSystem m_Renderer;
		private readonly SteeringController m_Steering;

		public Controller(int boidCount)
		{
			Random r = new Random();
			for (int i = 0; i < boidCount; i++)
			{
				Boid b = new Boid();
				b.Position = new Vector2f(i * 5,0) + b.Position;
				b.Velocity = new Vector2f((float) (r.NextDouble() - r.NextDouble()), (float) (r.NextDouble() - r.NextDouble()));
				b.MaxVelocity = 5f;
				m_Boids.Add(b);
			}

			m_Renderer = new RenderingSystem(m_Boids);
			m_Steering = new SteeringController(m_Boids);
		}

		public void Update(float deltaTime)
		{
			m_Steering.Update();

			for (int i = 0; i < m_Boids.Count; i++)
			{
				Boid b = m_Boids[i];
				b.Position = b.Position + (b.Velocity * deltaTime);
			}
		}

		public void Render(RenderTarget target, RenderStates states)
		{
			states.Transform.Scale(Config.PIXELS_PER_METER, Config.PIXELS_PER_METER);
			m_Renderer.Draw(target, states);
		}
	}
}
