using System;
using OpenTK;

namespace GameEngine
{
	public class Position
	{
		public Vector2 Current { get; set; }
		public Vector2 Destination { get; set; }

		public bool AtDestination => Current == Destination;

		public void Set(int x, int y)
		{
			Destination = new Vector2(x, y);
		}

		public void AddX(int x)
		{
			Current = new Vector2(Current.X + x, Current.Y);
		}

		public void SubtractX(int x)
		{
			Current = new Vector2(Current.X - x, Current.Y);
		}

		public void AddY(int y)
		{
			Current = new Vector2(Current.X, Current.Y + y);
		}

		public void SubtractY(int y)
		{
			Current = new Vector2(Current.X, Current.Y - y);
		}

		public Position ToWorld(View view)
		{
			Current /= view.Zoom;
			var dX = new Vector2((float)Math.Cos(view.Rotation), (float)Math.Sin(view.Rotation));
			var dY = new Vector2(
				(float)Math.Cos(view.Rotation + Math.PI / 2.0),
				(float)Math.Sin(view.Rotation + Math.PI / 2.0));
			return new Position
			{
				Current = Current + (dX * Current.X + dY * Current.Y)
			};
		}

		public static Position FromPosition(Position position)
		{
			return new Position { Current = position.Current, Destination = position.Destination };
		}

		public override string ToString()
		{
			return Current.ToString();
		}
	}
}