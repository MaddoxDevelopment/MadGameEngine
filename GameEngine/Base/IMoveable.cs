namespace GameEngine.Base
{
	public interface IMoveable
	{
		void Move();
		Direction Direction { get; set; }
	}

	public enum Direction
	{
		Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, None
	}
}
