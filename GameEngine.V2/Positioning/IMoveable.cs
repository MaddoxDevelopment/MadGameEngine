namespace GameEngine.V2.Positioning
{
	public interface IMoveable : ILocatable
	{
		void Move();
	}

	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
		UpLeft,
		UpRight,
		DownLeft,
		DownRight,
		None
	}
}