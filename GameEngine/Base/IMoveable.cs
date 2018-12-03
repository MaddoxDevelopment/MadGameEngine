namespace GameEngine.Base
{
	public interface IMoveable : ILocatable
	{
		Direction Direction { get; set; }
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