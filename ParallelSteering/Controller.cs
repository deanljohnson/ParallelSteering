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

		public Controller(int boidCount)
		{
			Random r = new Random();

			for (int i = 0; i < boidCount; i++)
			{
				Boid b = new Boid();
				b.Position = new Vector2f(i * 5,0) + b.Position;
				b.Velocity = new Vector2f((float)r.NextDouble(), (float)r.NextDouble());
				m_Boids.Add(b);
			}

			m_Renderer = new RenderingSystem(m_Boids);
		}

		public void Update(float deltaTime)
		{
			for (int i = 0; i < m_Boids.Count; i++)
			{
				Boid b = m_Boids[i];
				b.Position = b.Position + (b.Velocity * deltaTime * Config.PIXELS_PER_METER);
			}
		}

		public void Render(RenderTarget target, RenderStates states)
		{
			m_Renderer.Draw(target, states);
		}
	}
}
