using GameEngine.V2.Positioning;
using OpenTK;

namespace GameEngine.Movement
{
	public class TweenMovement
	{
		private readonly IMoveable _moveable;
		private float QuarticOut(float t) => -((t - 1) * (t - 1) * (t - 1) * (t - 1)) + 1;
		private float _currentStep;
		private float _tweenSteps;

		public TweenMovement(IMoveable moveable)
		{
			_moveable = moveable;
		}

		public void SetPosition(Vector2 position, int steps = 15)
		{
			if (_moveable.Position.Current == position)
				return;
			_moveable.Position.Destination = position;
			_currentStep = 0;
			_tweenSteps = steps;				
		}

		public void SetPosition(ILocatable locatable, int steps = 15)
		{
			if (locatable == null)
			{
				return;
			}
			SetPosition(locatable.Position.Current, steps);
		}

		public void Step(ILocatable locatable)
		{
			if (locatable == null)
			{
				return;
			}
			if (locatable.Position.Destination != _moveable.Position.Destination)
			{
				_moveable.Position.Destination = locatable.Position.Current;
			}
			Step();
		}
		
		public void Step()
		{
			if (_moveable.Position.AtDestination)
			{
				return;
			}
			if (_currentStep >= _tweenSteps)
			{
				_moveable.Position.Current = _moveable.Position.Destination;
				return;
			}
			_currentStep++;
			_moveable.Position.Current = _moveable.Position.Current + (_moveable.Position.Destination - _moveable.Position.Current) *
			                   QuarticOut(_currentStep / _tweenSteps);
		}
	}
}