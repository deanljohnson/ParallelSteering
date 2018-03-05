using System.Collections.Generic;
using SFML.Graphics;

namespace ParallelSteering.Rendering
{
	public class RenderingSystem : Drawable
	{
		private readonly List<IRenderJob> m_RenderJobs = new List<IRenderJob>();

		public void AddRenderJob(IRenderJob job)
		{
			m_RenderJobs.Add(job);
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			foreach (var job in m_RenderJobs)
			{
				job.Prepare();
			}

			foreach (var job in m_RenderJobs)
			{
				job.Render(target, states);
			}
		}
	}
}
