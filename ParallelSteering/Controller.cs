using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ParallelSteering
{
	public class Controller : IDisposable
	{
		private readonly List<Boid> m_Boids = new List<Boid>();

		private readonly RenderTarget m_RenderTarget;
		private readonly RenderingSystem m_Renderer;
		private readonly SteeringController m_Steering;

		public Controller(int boidCount, RenderTarget target)
		{
			Random r = new Random();
			for (int i = 0; i < boidCount; i++)
			{
				Boid b = new Boid();
				b.Position = new Vector2f(i * 5,0) + b.Position;
				b.Velocity = new Vector2f((float) (r.NextDouble() - r.NextDouble()), (float) (r.NextDouble() - r.NextDouble()));
				b.MaxVelocity = 20f;
				m_Boids.Add(b);
			}

			m_Renderer = new RenderingSystem(m_Boids);
			m_Steering = new SteeringController(m_Boids);
			m_RenderTarget = target;
		}

		public void Update(float deltaTime)
		{
			m_Steering.Update();

			for (int i = 0; i < m_Boids.Count; i++)
			{
				Boid b = m_Boids[i];
				b.Position = b.Position + (b.Velocity * deltaTime);

				if (b.Position.X < 0)
					b.Position = new Vector2f(b.Position.X + (m_RenderTarget.DefaultView.Size.X / Config.PIXELS_PER_METER), b.Position.Y);
				if (b.Position.X > m_RenderTarget.DefaultView.Size.X / Config.PIXELS_PER_METER)
					b.Position = new Vector2f(b.Position.X - (m_RenderTarget.DefaultView.Size.X / Config.PIXELS_PER_METER), b.Position.Y);
				if (b.Position.Y < 0)
					b.Position = new Vector2f(b.Position.X, b.Position.Y + (m_RenderTarget.DefaultView.Size.Y / Config.PIXELS_PER_METER));
				if (b.Position.Y > m_RenderTarget.DefaultView.Size.Y / Config.PIXELS_PER_METER)
					b.Position = new Vector2f(b.Position.X, b.Position.Y - (m_RenderTarget.DefaultView.Size.Y / Config.PIXELS_PER_METER));
			}
		}

		public void Render()
		{
			RenderStates states = RenderStates.Default;
			states.Transform.Scale(Config.PIXELS_PER_METER, Config.PIXELS_PER_METER);
			m_Renderer.Draw(m_RenderTarget, states);
		}

		public void Dispose()
		{
			m_Steering?.Dispose();
		}
	}
}
