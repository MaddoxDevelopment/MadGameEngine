using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace GameEngine.V2
{
	public class DelayedTextWriter
	{
		private readonly Dictionary<string, Tuple<string, long, Vector2, List<Texture2D>>> _map;
		
		private long GetMillis => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

		private readonly Font _font;
		private readonly long _timeToWait;

		public DelayedTextWriter(Font font, long timeToWait)
		{
			_font = font;
			_timeToWait = timeToWait;
			_map = new Dictionary<string, Tuple<string, long, Vector2, List<Texture2D>>>();
		}

		public void WriteTextDelayed(string id, string text, Vector2 position)
		{
			if (!_map.ContainsKey(id))
			{
				_map[id] = new Tuple<string, long, Vector2, List<Texture2D>>
					(text, GetMillis, position, TextRender.LoadText(_font, text));
			}
			
			var values = _map[id];
			TextRender.PrintText(values.Item4, values.Item3);
			
			if (GetMillis - _map[id].Item2 > _timeToWait)
			{
				_map.Remove(id);
			}
		}
	}
}