using System;
using System.Collections.Concurrent;

namespace GameEngine.V2.Queue
{
	public abstract class GameQueue
	{
		private readonly BlockingCollection<GameAction> list = new BlockingCollection<GameAction>();

		public void Enqueue(long duration, Action action, Action onRemove = null)
		{
			list.TryAdd(new GameAction(duration, action, onRemove));
		}

		private int _count = -1;

		public int Count => _count;
		
		public void Execute()
		{
			if (_count != -1)
				return;
			_count = list.Count;
			for (int i = 0; i < _count; i++)
			{
				if(!list.TryTake(out var action)) 
					continue;
				action.Action.Invoke();
				if (!action.IsForever && action.Expiration < DateTime.UtcNow)
				{
					action.OnRemove?.Invoke();
					continue;
				}
				list.TryAdd(action);
			}
			_count = -1;
		}
	}
	
	public class GameAction
	{
		public Action Action { get; }
		public Action OnRemove { get; }
		public DateTime Expiration { get; }
		public bool IsForever { get; }

		public GameAction(long expiration, Action action, Action onRemove = null)
		{
			IsForever = expiration == -1;
			if(!IsForever)
				Expiration = DateTime.UtcNow.AddMilliseconds(expiration);
			Action = action;
			OnRemove = onRemove;
		}
		
	}
}