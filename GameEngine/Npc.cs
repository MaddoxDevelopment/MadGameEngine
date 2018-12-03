using System;
using System.Drawing;
using GameEngine.Base;
using OpenTK;

namespace GameEngine
{
	public class Npc : INpc
	{
		private readonly string _name;
		private readonly Vector2 _size;

		public Npc(string name, Vector2 startPos)
		{
			_name = name;
			Position = new Position { Current = startPos, Destination = startPos };
			Sprite = SpriteLoader.LoadTexture("alienBlue_front.png");
			_size = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
			Console.WriteLine(Sprite.Height + " " + Sprite.Width);
		}

		private RectangleF Rectangle =>
			new RectangleF(Position.Current.X - _size.X / 2f, Position.Current.Y - _size.Y / 2f, _size.X, _size.Y);

		public Texture2D Sprite { get; set; }

		public void Render()
		{
			GameEngine.Sprite.Draw(Sprite, Rectangle);
		}

		public bool CheckCollision()
		{
			return false;
		}

		public ICollisionable CollidingWith { get; set; }

		public RectangleF GetBounds()
		{
			return Rectangle;
		}

		public string Name()
		{
			return _name;
		}

		public Position Position { get; set; }
	}
}