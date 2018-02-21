using System;
using System.Collections.Generic;
using System.Threading;
using SFML.System;

namespace ParallelSteering
{
	public class SteeringController
	{
		private readonly List<Boid> m_Boids;
		private readonly List<Vector2f> m_NextVelocities;
		private readonly ManualResetEvent m_StartSignal;
		private readonly List<AutoResetEvent> m_EndSignals;
		private readonly List<Thread> m_Threads;

		public SteeringController(List<Boid> boids)
		{
			m_Boids = boids;
			m_NextVelocities = new List<Vector2f>(boids.Count);
			for (int i = 0; i < boids.Count; i++)
			{
				m_NextVelocities.Add(new Vector2f());
			}

			m_Threads = new List<Thread>();
			m_StartSignal = new ManualResetEvent(false);
			m_EndSignals = new List<AutoResetEvent>();

			int start = 0;
			for (int i = 0; i < Config.THREAD_COUNT; i++)
			{
				int iLocal = i;
				int localStart = start;
				int count = (int) Math.Ceiling((m_Boids.Count - (i + 0.0)) / Config.THREAD_COUNT);

				m_EndSignals.Add(new AutoResetEvent(false));
				m_Threads.Add(new Thread(() => ProcessBoidSteering(localStart, count, m_StartSignal, m_EndSignals[iLocal])));
				m_Threads[i].Start();

				start += count;
			}
		}

		public void Update()
		{
			// Signal all threads to execute
			m_StartSignal.Set();
			m_StartSignal.Reset();

			// Wait for all threads to signal they are done
			for (int i = 0; i < m_Threads.Count; i++)
			{
				m_EndSignals[i].WaitOne();
			}
			
			for (int i = 0; i < m_Boids.Count; i++)
			{
				m_Boids[i].Velocity = m_NextVelocities[i];
			}
		}

		private void ProcessBoidSteering(int start, int count, EventWaitHandle startSignal, EventWaitHandle endSignal)
		{
			while (true)
			{
				startSignal.WaitOne();

				for (int i = start; i < start + count; i++)
				{
					Vector2f vel = new Vector2f();
					vel += Steering.Wander(m_Boids[i]) * 50;
					vel += Steering.Align(m_Boids[i], m_Boids) * 5;
					vel += Steering.Cohesion(m_Boids[i], m_Boids);
					vel += Steering.Separation(m_Boids[i], m_Boids);
					m_NextVelocities[i] = vel.Normalized() * m_Boids[i].MaxVelocity;
				}

				endSignal.Set();
			}
		}
	}
}
