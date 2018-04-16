using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using QuadTree;
using SFML.System;
using SFQuadTree;

namespace ParallelSteering
{
	public class SteeringController
	{
		private readonly List<Boid> m_Boids;
		private readonly List<Vector2f> m_NextVelocities;
		private readonly QuadTree<Boid> m_QuadTree;
		private readonly List<WaitCallback> m_WorkItems = new List<WaitCallback>();
		private readonly AutoResetEvent[] m_EndEvents;

		public SteeringController(List<Boid> boids, QuadTree<Boid> tree)
		{
			m_Boids = boids;

			m_QuadTree = tree;

			m_NextVelocities = new List<Vector2f>(boids.Count);
			for (int i = 0; i < boids.Count; i++)
			{
				m_NextVelocities.Add(new Vector2f());
			}

			m_EndEvents = new AutoResetEvent[Config.THREAD_COUNT];

			int start = 0;
			for (int i = 0; i < Config.THREAD_COUNT; i++)
			{
				int locali = i;
				int localStart = start;
				int count = (int) Math.Ceiling((m_Boids.Count - (i + 0.0)) / Config.THREAD_COUNT);

				m_EndEvents[i] = new AutoResetEvent(false);
				m_WorkItems.Add((a) => ProcessBoidSteering(localStart, count, m_EndEvents[locali]));

				start += count;
			}
		}

		public void Update()
		{
			for (int i = 0; i < m_WorkItems.Count; i++)
			{
				ThreadPool.QueueUserWorkItem(m_WorkItems[i]);
			}

			// Wait for all threads to signal they are done
			WaitHandle.WaitAll(m_EndEvents);

			for (int i = 0; i < m_Boids.Count; i++)
			{
				m_Boids[i].Velocity = m_NextVelocities[i];
			}
		}

		private void ProcessBoidSteering(int start, int count, EventWaitHandle endHandle)
		{
			Random random = new Random(Config.SEED);

			for (int i = start; i < start + count; i++)
			{
				MaxPriorityQueue<Boid> inRangeBoids = new MaxPriorityQueue<Boid>();
				m_QuadTree.GetKClosestObjects(m_Boids[i].Position, 30, Steering.COHESION_RADIUS, inRangeBoids);

				Vector2f vel = new Vector2f();
				vel += Steering.Wander(m_Boids[i], random) * 50;
				vel += Steering.Align(m_Boids[i], inRangeBoids.ToList()) * 5;
				vel += Steering.Cohesion(m_Boids[i], inRangeBoids.ToList());
				vel += Steering.Separation(m_Boids[i], inRangeBoids.ToList());
				m_NextVelocities[i] = vel.Normalized() * m_Boids[i].MaxVelocity;

				inRangeBoids.Clear();
			}

			endHandle.Set();
		}
	}
}
