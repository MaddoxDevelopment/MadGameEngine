using System.Drawing;

namespace GameEngine.Physics
{
	public class Collisions
	{
		public static bool IsColliding(RectangleF rect, RectangleF rect2)
		{
			return !(!(rect.X < rect2.X + rect2.Width) || !(rect.X + rect.Width > rect2.X) ||
			       !(rect.Y < rect2.Y + rect2.Height) || !(rect.Y + rect.Height > rect2.Y));
		}
	}
}