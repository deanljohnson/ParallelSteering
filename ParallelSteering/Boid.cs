using SFML.Graphics;
using SFML.System;

namespace ParallelSteering
{
	public class Boid : Transformable
	{
		private const float HALF_SIZE = 1;

		public Vector2f WanderTarget { get; set; }
		public Vector2f Velocity { get; set; }
		public ConvexShape Shape { get; private set; }
		public float MaxVelocity { get; set; }

		public Boid()
		{
			Shape = new ConvexShape(3);

			Shape.SetPoint(0, new Vector2f(HALF_SIZE, 0));
			Shape.SetPoint(1, new Vector2f(-HALF_SIZE, HALF_SIZE));
			Shape.SetPoint(2, new Vector2f(-HALF_SIZE, -HALF_SIZE));

			Position = new Vector2f(50, 50);
		}
	}
}
