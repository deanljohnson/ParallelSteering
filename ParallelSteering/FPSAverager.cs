namespace ParallelSteering
{
	public class FPSAverager
	{
		private readonly int[] m_PastFPS;
		private int m_NextIndex;

		public int AverageFPS { get; set; }

		public FPSAverager(int averageLength)
		{
			m_PastFPS = new int[averageLength];
			for (int i = 0; i < averageLength; i++)
			{
				m_PastFPS[i] = -1;
			}
		}

		public void PushFPS(int fps)
		{
			m_PastFPS[m_NextIndex] = fps;
			m_NextIndex = (m_NextIndex + 1) % m_PastFPS.Length;

			int count = 0;
			int sum = 0;
			for (int i = 0; i < m_PastFPS.Length; i++)
			{
				if (m_PastFPS[i] == -1) continue;
				count++;
				sum += m_PastFPS[i];
			}

			AverageFPS = sum / count;
		}
	}
}
