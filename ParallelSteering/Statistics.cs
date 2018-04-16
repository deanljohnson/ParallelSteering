using System.Collections.Generic;

namespace ParallelSteering
{
	public static class Statistics
	{
		public static double Percentile(List<double> sequence, double percentile, bool sorted = false)
		{
			if (!sorted)
				sequence.Sort();

			int N = sequence.Count;
			double n = (N - 1) * percentile + 1;

			if (n == 1d) return sequence[0];
			if (n == N) return sequence[N - 1];

			int k = (int)n;
			double d = n - k;
			return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
		}
	}
}
