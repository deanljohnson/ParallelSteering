using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ParallelSteering
{
	public class Steering
	{
		private static readonly Random Random = new Random(Config.SEED);
		private static float m_WanderAngle;

		public const float ARRIVE_RADIUS = 10f;
		public const float WANDER_CIRCLE_RADIUS = 5f;
		public const float WANDER_CIRCLE_DISTANCE = 30f;
		public const float WANDER_ANGLE_CHANGE = 1.5f;
		public const float SEPARATION_RADIUS = 10f;
		public const float COHESION_RADIUS = 100f;

		public static Vector2f Seek(Boid self, Vector2f target)
		{
			return (target - self.Position).Normalized();
		}

		public static Vector2f Flee(Boid self, Vector2f target)
		{
			return -Seek(self, target);
		}

		public static Vector2f Arrive(Boid self, Vector2f target)
		{
			Vector2f dif = (target - self.Position);
			float dist = dif.Length();

			if (dist < ARRIVE_RADIUS)
			{
				return dif.Normalized() * self.MaxVelocity * (dist / ARRIVE_RADIUS);
			}

			return dif.Normalized() * self.MaxVelocity;
		}

		public static Vector2f Wander(Boid self)
		{
			Vector2f normalVel = self.Velocity.Normalized();
			Vector2f circleCenter = normalVel * WANDER_CIRCLE_DISTANCE;
			Vector2f displacement = new Vector2f(
				(float) Math.Cos(m_WanderAngle) * WANDER_CIRCLE_RADIUS, 
				(float) Math.Sin(m_WanderAngle) * WANDER_CIRCLE_RADIUS);

			m_WanderAngle += (float)(Random.NextDouble() * WANDER_ANGLE_CHANGE - WANDER_ANGLE_CHANGE * .5f);

			return (circleCenter + displacement).Normalized();
		}

		public static Vector2f Align(Boid self, IList<Boid> others)
		{
			float x = 0, y = 0;
			for (int i = 0; i < others.Count; i++)
			{
				x += others[i].Velocity.X;
				y += others[i].Velocity.Y;
			}

			return new Vector2f(x,y).Normalized();
		}

		public static Vector2f Cohesion<T>(Boid self, IList<T> others)
			where T : Transformable
		{
			int count = 0;
			float x = 0, y = 0;

			for (int i = 0; i < others.Count; i++)
			{
				if ((self.Position - others[i].Position).SquaredLength()
				    < COHESION_RADIUS * COHESION_RADIUS)
				{
					x += others[i].Position.X;
					y += others[i].Position.Y;
					count++;
				}
			}

			return Seek(self, new Vector2f(x / count, y / count));
		}

		public static Vector2f Separation<T>(Boid self, IList<T> others)
			where T : Transformable
		{
			int count = 0;
			float x = 0, y = 0;

			for (int i = 0; i < others.Count; i++)
			{
				if ((self.Position - others[i].Position).SquaredLength() 
					< SEPARATION_RADIUS * SEPARATION_RADIUS)
				{
					x += others[i].Position.X;
					y += others[i].Position.Y;
					count++;
				}
			}

			return Flee(self, new Vector2f(x / count, y / count));
		}
	}
}
