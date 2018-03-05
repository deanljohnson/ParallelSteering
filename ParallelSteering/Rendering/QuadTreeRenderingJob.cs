using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFQuadTree;

namespace ParallelSteering.Rendering
{
	public class QuadTreeRenderingJob<T> : IRenderJob 
		where T : Transformable
	{
		private readonly QuadTree<T> m_Tree;
		private readonly VertexArray m_VertexArray;

		public QuadTreeRenderingJob(QuadTree<T> tree)
		{
			m_Tree = tree;
			m_VertexArray = new VertexArray(PrimitiveType.Lines);

		}

		public void Prepare()
		{
			List<FloatRect> treeRegions = new List<FloatRect>();
			m_Tree.GetAllRegions(treeRegions);

			m_VertexArray.Resize((uint) (treeRegions.Count * 8));

			for (uint i = 0; i < treeRegions.Count; i++)
			{
				FloatRect rect = treeRegions[(int) i];
				uint j = i * 8;
				m_VertexArray[j] = new Vertex(new Vector2f(rect.Left, rect.Top), Color.Red);
				m_VertexArray[j+1] = new Vertex(new Vector2f(rect.Left + rect.Width, rect.Top), Color.Red);

				m_VertexArray[j+2] = new Vertex(new Vector2f(rect.Left + rect.Width, rect.Top), Color.Red);
				m_VertexArray[j+3] = new Vertex(new Vector2f(rect.Left + rect.Width, rect.Top + rect.Height), Color.Red);

				m_VertexArray[j+4] = new Vertex(new Vector2f(rect.Left + rect.Width, rect.Top + rect.Height), Color.Red);
				m_VertexArray[j+5] = new Vertex(new Vector2f(rect.Left, rect.Top + rect.Height), Color.Red);

				m_VertexArray[j+6] = new Vertex(new Vector2f(rect.Left, rect.Top + rect.Height), Color.Red);
				m_VertexArray[j+7] = new Vertex(new Vector2f(rect.Left, rect.Top), Color.Red);
			}
		}

		public void Render(RenderTarget target, RenderStates states)
		{
			target.Draw(m_VertexArray, states);
		}
	}
}
