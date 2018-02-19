using SFML.Graphics;
using SFML.System;

namespace ParallelSteering
{
	public class Boid : Transformable, Drawable
	{
		private const float HALF_SIZE = 5f;

		public Vector2f Velocity { get; set; }
		public ConvexShape Shape { get; private set; }

		public Boid()
		{
			Shape = new ConvexShape(3);

			Shape.SetPoint(0, new Vector2f(0, -HALF_SIZE));
			Shape.SetPoint(1, new Vector2f(-HALF_SIZE, HALF_SIZE));
			Shape.SetPoint(2, new Vector2f(HALF_SIZE, HALF_SIZE));

			Position = new Vector2f(50, 50);
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;
			Shape.Draw(target, states);
		}
	}
}
