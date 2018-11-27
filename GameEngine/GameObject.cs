using System;
using OpenTK;

namespace GameEngine
{
	public class GameObjectPosition
	{
		public float X { get; private set; }
		public float Y { get; private set; }

		public GameObjectPosition()
		{
			X = 0;
			Y = 0;
		}

		public void Set(float x, float y)
		{
			X = x;
			Y = y;
		}
		public void Left(float x) => X -= x;
		public void Right(float x) => X += x;
		public void Up(float y) => Y -= y;
		public void Down(float y) => Y += y;
	}

	public class GameObject
	{
		public Guid Id { get; } = Guid.NewGuid();
		public Vector2[] Vectors { get; set; }
		public GameObjectPosition Position { get; }

		public GameObject()
		{
			Position = new GameObjectPosition();
			Vectors = new Vector2[0];
		}
	
	}
}