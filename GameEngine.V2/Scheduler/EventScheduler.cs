using System;
using System.Collections.Generic;

namespace GameEngine.V2.Scheduler
{
	public class EventScheduler : IDisposable
	{
		private readonly Dictionary<string, GameEvent> _events;

		public EventScheduler()
		{
			_events = new Dictionary<string, GameEvent>();
		}
		
		private static long GetMillis => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;


		public void RemoveTask(string id)
		{
			if (!_events.ContainsKey(id))
				return;
			_events.Remove(id);
		}
		
		public void ExecuteRecurring(string id, long timeToWait, long initialDelay, Action action)
		{
			if (!_events.ContainsKey(id))
			{
				_events[id] = new GameEvent
				{
					timeToWait = timeToWait,
					action = action,
					dateAdded = GetMillis,
					initialDelay = initialDelay
				};
			}
			var e = _events[id];
			if (!e.passedInitialDelay && GetMillis - e.initialDelay > e.dateAdded)
			{
				e.passedInitialDelay = true;
				_events[id] = e;
			}
			if (!e.passedInitialDelay) 
				return;			
			if (GetMillis - e.dateAdded < e.timeToWait) 
				return;

			e.action?.Invoke();
			e.action = action;
			e.dateAdded = GetMillis;
			_events[id] = e;
		}

		public void Dispose()
		{
			_events.Clear();
		}
	}

	public class GameEvent
	{
		public long timeToWait { get; set; }
		public long initialDelay { get; set; }
		public long dateAdded { get; set; }
		public bool passedInitialDelay { get; set; }
		public Action action { get; set; }
	}
	
}