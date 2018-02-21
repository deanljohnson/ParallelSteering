using System;
using SFML.System;

namespace ParallelSteering
{
	public static class VectorExtensions
	{
		public static float Length(this Vector2f v)
		{
			return (float) Math.Sqrt((v.X * v.X) + (v.Y * v.Y));
		}

		public static float SquaredLength(this Vector2f v)
		{
			return v.X * v.X + v.Y * v.Y;
		}

		public static Vector2f Normalized(this Vector2f v)
		{
			float l = v.Length();
			if (float.IsNaN(l) || l == 0f)
				return new Vector2f(1,0);
			return v / l;
		}

		public static float Angle(this Vector2f v)
		{
			return (float) Math.Atan2(v.Y, v.X);
		}
	}
}
