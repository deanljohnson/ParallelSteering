using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using QuadTree;
using SFML.Graphics;
using SFML.System;
using SFQuadTree;

namespace ParallelSteering
{
	public class SteeringController : IDisposable
	{
		private readonly List<Boid> m_Boids;
		private readonly List<Vector2f> m_NextVelocities;
		private readonly ManualResetEvent m_StartSignal;
		private readonly List<AutoResetEvent> m_EndSignals;
		private readonly List<Thread> m_Threads;
		private readonly QuadTree<Boid> m_QuadTree;

		public SteeringController(List<Boid> boids, QuadTree<Boid> tree)
		{
			m_Boids = boids;

			m_QuadTree = tree;

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

				Random random = new Random(Config.SEED);

				MaxPriorityQueue<Boid> inRangeBoids = new MaxPriorityQueue<Boid>();
				for (int i = start; i < start + count; i++)
				{
					m_QuadTree.GetKClosestObjects(m_Boids[i].Position, 30, Steering.COHESION_RADIUS, inRangeBoids);

					Vector2f vel = new Vector2f();
					vel += Steering.Wander(m_Boids[i], random) * 50;
					vel += Steering.Align(m_Boids[i], inRangeBoids.ToList()) * 5;
					vel += Steering.Cohesion(m_Boids[i], inRangeBoids.ToList());
					vel += Steering.Separation(m_Boids[i], inRangeBoids.ToList());
					m_NextVelocities[i] = vel.Normalized() * m_Boids[i].MaxVelocity;

					inRangeBoids.Clear();
				}

				endSignal.Set();
			}
		}

		public void Dispose()
		{
			foreach (var thread in m_Threads)
			{
				thread.Abort();
			}
		}
	}
}
