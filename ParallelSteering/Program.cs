using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ParallelSteering
{
	class Program
	{
		private static bool Closed;

		static void Main(string[] args)
		{
			VideoMode mode = new VideoMode(1440, 960);
			RenderWindow window = new RenderWindow(mode, "Parallel Steering");
			
			window.SetFramerateLimit(60);
			window.Closed += OnWindowClose;

			Controller control = new Controller(400, window);
			Clock clock = new Clock();
			Clock updateClock = new Clock();
			FPSAverager ufpsAverage = new FPSAverager(10);

			float elapsedFrameTime = 0f;

			while (!Closed)
			{
				window.DispatchEvents();

				updateClock.Restart();
				control.Update(elapsedFrameTime);
				int fps = (int) Math.Floor(1f / updateClock.ElapsedTime.AsSeconds());
				ufpsAverage.PushFPS(fps);
				window.SetTitle($"Parallel Steering - {ufpsAverage.AverageFPS} UPS");

				elapsedFrameTime = clock.ElapsedTime.AsSeconds();
				clock.Restart();

				window.Clear(Color.Black);

				control.Render();

				window.Display();
			}
		}

		private static void OnWindowClose(object sender, EventArgs e)
		{
			Closed = true;
		}
	}
}
