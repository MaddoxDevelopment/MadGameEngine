using System.Drawing;

namespace GameEngine.Base
{
	public interface ICollisionable : IRenderable
	{
		ICollisionable CollidingWith { get; set; }
		bool CheckCollision();
		RectangleF GetBounds();
	}
}