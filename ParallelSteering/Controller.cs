using System;
using System.Collections.Generic;
using ParallelSteering.Rendering;
using SFML.Graphics;
using SFML.System;
using SFQuadTree;

namespace ParallelSteering
{
	public class Controller : IDisposable
	{
		private readonly List<Boid> m_Boids = new List<Boid>();

		private readonly RenderTarget m_RenderTarget;
		private readonly RenderingSystem m_Renderer;
		private readonly SteeringController m_Steering;
		private readonly QuadTree<Boid> m_QuadTree;

		public Controller(int boidCount, RenderWindow target)
		{
			m_RenderTarget = target;

			float width = (m_RenderTarget.DefaultView.Size.X / Config.PIXELS_PER_METER);
			float height = (m_RenderTarget.DefaultView.Size.Y / Config.PIXELS_PER_METER);

			float maxD = Math.Max(width, height);

			m_QuadTree = new QuadTree<Boid>(new FloatRect(0, 0,
				maxD,
				maxD));

			Random r = new Random(Config.SEED);
			for (int i = 0; i < boidCount; i++)
			{
				Boid b = new Boid();
				b.Position = new Vector2f((float) (r.NextDouble() * width), (float) (r.NextDouble() * height));
				b.Velocity = new Vector2f((float) (r.NextDouble() - r.NextDouble()), (float) (r.NextDouble() - r.NextDouble()));
				b.MaxVelocity = 20f;
				m_Boids.Add(b);

				m_QuadTree.Add(b);
			}

			m_Renderer = new RenderingSystem();
			m_Renderer.AddRenderJob(new QuadTreeRenderingJob<Boid>(m_QuadTree));
			m_Renderer.AddRenderJob(new BoidRenderingJob(m_Boids));
			m_Steering = new SteeringController(m_Boids, m_QuadTree);
		}

		public void Update(float deltaTime)
		{
			m_QuadTree.Update();
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
