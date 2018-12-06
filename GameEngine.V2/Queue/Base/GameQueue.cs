using System;
using System.Collections.Concurrent;

namespace GameEngine.V2.Queue.Base
{
	public abstract class GameQueue
	{
		private readonly BlockingCollection<GameAction> _list = new BlockingCollection<GameAction>();

		public void Enqueue(long duration, Action action, Action onRemove = null)
		{
			_list.TryAdd(new GameAction(duration, action, onRemove));
		}

		public void Enqueue(Func<bool> condition, Action action, Action onRemove = null)
		{
			_list.TryAdd(new GameAction(condition, action, onRemove));
		}

		private int _count = -1;

		public int Count => _count;

		public void Execute()
		{
			if (_count != -1)
				return;
			_count = _list.Count;
			for (int i = 0; i < _count; i++)
			{
				if (!_list.TryTake(out var action))
					continue;
				action.Action.Invoke();
				if (action.Condition != null)
				{
					var expired = action.Condition.Invoke();
					if (expired)
					{
						action.OnRemove?.Invoke();
						continue;
					}
				}
				else
				{
					if (!action.IsForever && action.Expiration < DateTime.UtcNow)
					{
						action.OnRemove?.Invoke();
						continue;
					}
				}

				_list.TryAdd(action);
			}

			_count = -1;
		}
	}

	public class GameAction
	{
		public Func<bool> Condition { get; }
		public Action Action { get; }
		public Action OnRemove { get; }
		public DateTime Expiration { get; }
		public bool IsForever { get; }

		public GameAction(long expiration, Action action, Action onRemove = null)
		{
			IsForever = expiration == -1;
			if (!IsForever)
				Expiration = DateTime.UtcNow.AddMilliseconds(expiration);
			Action = action;
			OnRemove = onRemove;
		}

		public GameAction(Func<bool> condition, Action action, Action onRemove = null)
		{
			Condition = condition;
			IsForever = false;
			Action = action;
			OnRemove = onRemove;
		}
	}
}