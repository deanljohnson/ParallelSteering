using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ParallelSteering
{
	class Program
	{
		private static bool Closed;
		private static readonly List<double> Timings = new List<double>();
		private static string m_OutputTextFile = "out.txt";
		private static double m_MinutesToExecute = -1;

		static void Main(string[] args)
		{
			ParseCommandLineArguments(args);

			VideoMode mode = new VideoMode(920, 920);
			RenderWindow window = new RenderWindow(mode, "Parallel Steering");
			
			window.SetFramerateLimit(60);
			window.Closed += OnWindowClose;

			Controller control = new Controller(Config.BOID_COUNT, window);
			Clock clock = new Clock();
			Clock updateClock = new Clock();
			FPSAverager ufpsAverage = new FPSAverager(30);

			float elapsedFrameTime = 0f;
			float totalElapsedTime = 0f;

			while (!Closed)
			{
				window.DispatchEvents();

				if (Closed ||
					(m_MinutesToExecute > 0 && totalElapsedTime > m_MinutesToExecute * 60))
				{
					string output = string.Join(",", Timings.Select(t => t.ToString("N3")));
					File.WriteAllText(m_OutputTextFile, output);
					break;
				}

				updateClock.Restart();
				control.Update(elapsedFrameTime);
				double fps = 1f / updateClock.ElapsedTime.AsSeconds();
				Timings.Add(fps);
				ufpsAverage.PushFPS((int)Math.Floor(fps));
				window.SetTitle($"Parallel Steering - {ufpsAverage.AverageFPS} UPS");

				elapsedFrameTime = clock.ElapsedTime.AsSeconds();
				totalElapsedTime += elapsedFrameTime;
				clock.Restart();

				window.Clear(Color.Black);

				control.Render();

				window.Display();

				OutputStatsToConsole();
			}
		}

		private static void OutputStatsToConsole()
		{
			if (Timings.Count <= 1)
				return;

			Timings.Sort();
			Console.WriteLine(GetStatisticsOutput());
		}

		private static string GetStatisticsOutput()
		{
			Timings.Sort();
			return $"Threads: {Config.THREAD_COUNT}" +
				   $"\tMin: {Timings[1]:N3}" +
				   $"\t50%: {Statistics.Percentile(Timings, .5, true):N3}" +
				   $"\t75%: {Statistics.Percentile(Timings, .75, true):N3}" +
				   $"\t90%: {Statistics.Percentile(Timings, .9, true):N3}" +
				   $"\t99%: {Statistics.Percentile(Timings, .99, true):N3}" +
				   $"\tMax: {Timings[Timings.Count - 1]:N3}";
		}

		private static void ParseCommandLineArguments(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "-t")
				{
					m_MinutesToExecute = float.Parse(args[i + 1]);
					i++;
				}
				else if (args[i] == "-o")
				{
					m_OutputTextFile = args[i + 1];
					i++;
				}
				else if (args[i] == "-numThreads")
				{
					Config.THREAD_COUNT = int.Parse(args[i + 1]);
					i++;
				}
			}
		}

		private static void OnWindowClose(object sender, EventArgs e)
		{
			Closed = true;
		}
	}
}
