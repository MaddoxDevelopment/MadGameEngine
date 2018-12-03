namespace GameEngine.Base
{
	public interface IPlayer : ILocatable, ICollisionable, INameable, IMoveable
	{
		bool IsLocalPlayer { get; }
	}
}