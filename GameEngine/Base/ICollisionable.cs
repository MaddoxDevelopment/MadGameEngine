using System.Drawing;

namespace GameEngine.Base
{
	public interface ICollisionable : IRenderable
	{
		ICollisionable CollidingWith { get; set; }
		bool CheckCollision();
		RectangleF GetBounds();
		bool CanCollideLocalPlayer();
		void OnCollision(ICollisionable source);
	}

	public interface ICollisionableLocatable : ICollisionable, ILocatable {}
}