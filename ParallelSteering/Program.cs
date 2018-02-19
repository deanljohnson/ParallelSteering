using System;
using System.Timers;
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

			Controller control = new Controller(100);
			Clock clock = new Clock();

			while (!Closed)
			{
				window.DispatchEvents();

				control.Update(clock.ElapsedTime.AsSeconds());
				clock.Restart();

				window.Clear(Color.Black);

				control.Render(window, RenderStates.Default);

				window.Display();
			}
		}

		private static void OnWindowClose(object sender, EventArgs e)
		{
			Closed = true;
		}
	}
}
