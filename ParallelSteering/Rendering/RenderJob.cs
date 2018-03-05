using SFML.Graphics;

namespace ParallelSteering.Rendering
{
	public interface IRenderJob
	{
		void Prepare();
		void Render(RenderTarget target, RenderStates states);
	}
}
