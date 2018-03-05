using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ParallelSteering.Rendering
{
	public class BoidRenderingJob : IRenderJob
	{
		private readonly List<Boid> m_Boids;
		private readonly VertexArray m_VertexArray;

		public BoidRenderingJob(List<Boid> boids)
		{
			m_Boids = boids;
			m_VertexArray = new VertexArray(PrimitiveType.Triangles, (uint)m_Boids.Count * 3);
		}

		public void Prepare()
		{
			for (int i = 0; i < m_Boids.Count; i++)
			{
				Vector2f p0 = m_Boids[i].Shape.GetPoint(0);
				Vector2f p1 = m_Boids[i].Shape.GetPoint(1);
				Vector2f p2 = m_Boids[i].Shape.GetPoint(2);

				m_Boids[i].Rotation = (float)(m_Boids[i].Velocity.Angle() * (180f / Math.PI));

				m_VertexArray[(uint)(i * 3)] = new Vertex(m_Boids[i].Transform.TransformPoint(p0));
				m_VertexArray[(uint)(i * 3) + 1] = new Vertex(m_Boids[i].Transform.TransformPoint(p1));
				m_VertexArray[(uint)(i * 3) + 2] = new Vertex(m_Boids[i].Transform.TransformPoint(p2));
			}
		}

		public void Render(RenderTarget target, RenderStates states)
		{
			target.Draw(m_VertexArray, states);
		}
	}
}
