using System;
using System.Collections.Generic;
using System.Drawing;
using GameEngine.V2.Scheduler;
using OpenTK;

namespace GameEngine.V2.Text
{
	public class DelayedTextWriter : IDisposable
	{
		private readonly Dictionary<string, Tuple<string, long, Vector2, List<Texture2D>>> _map;
		
		private static long GetMillis => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

		private readonly Font _font;
		private readonly long _timeToWait;

		private readonly EventScheduler _scheduler;

		public DelayedTextWriter(Font font, long timeToWait)
		{
			_font = font;
			_timeToWait = timeToWait;
			_scheduler = new EventScheduler();
			_map = new Dictionary<string, Tuple<string, long, Vector2, List<Texture2D>>>();
		}

		public void WriteTextDelayed(string id, string text, Vector2 position)
		{
			_scheduler.ExecuteRecurring(id, _timeToWait, 0, () =>
			{
				_map[id] = new Tuple<string, long, Vector2, List<Texture2D>>
					(text, GetMillis, position, TextWriter.LoadText(_font, text));
			});
			if (!_map.ContainsKey(id)) return;
			var values = _map[id];
			TextWriter.PrintText(values.Item4, values.Item3);
		}

		public void Dispose()
		{
			_font?.Dispose();
			_scheduler?.Dispose();
		}
	}
}