using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ParallelSteering
{
	public class RenderingSystem : Drawable
	{
		private readonly List<Boid> m_Boids;
		private readonly VertexArray m_VertArray;

		public RenderingSystem(List<Boid> boids)
		{
			m_Boids = boids;

			m_VertArray = new VertexArray(PrimitiveType.Triangles, (uint) m_Boids.Count * 3);
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			for (int i = 0; i < m_Boids.Count; i++)
			{
				Vector2f p0 = m_Boids[i].Shape.GetPoint(0);
				Vector2f p1 = m_Boids[i].Shape.GetPoint(1);
				Vector2f p2 = m_Boids[i].Shape.GetPoint(2);

				m_VertArray[(uint)(i * 3)] = new Vertex(m_Boids[i].Transform.TransformPoint(p0));
				m_VertArray[(uint)(i * 3) + 1] = new Vertex(m_Boids[i].Transform.TransformPoint(p1));
				m_VertArray[(uint)(i * 3) + 2] = new Vertex(m_Boids[i].Transform.TransformPoint(p2));
			}

			target.Draw(m_VertArray, states);
		}
	}
}
