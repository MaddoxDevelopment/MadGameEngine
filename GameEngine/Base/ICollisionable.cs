using System.Drawing;

namespace GameEngine.Base
{
	public interface ICollisionable : IRenderable
	{
		bool CheckCollision();
		ICollisionable CollidingWith { get; set; }
		RectangleF GetBounds();
	}
}