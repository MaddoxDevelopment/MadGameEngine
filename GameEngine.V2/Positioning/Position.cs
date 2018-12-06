using OpenTK;

namespace GameEngine.V2.Positioning
{
	public class Position
	{
		public Vector2 Current { get; set; }
		public Vector2 Destination { get; set; }

		public bool AtDestination => Current == Destination;

		public void Set(int x, int y)
		{
			Current = new Vector2(x, y);
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