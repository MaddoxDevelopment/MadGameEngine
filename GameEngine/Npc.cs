using System;
using System.Diagnostics;
using System.Drawing;
using GameEngine.Base;
using OpenTK;

namespace GameEngine
{
	public class Npc : INpc
	{
		private readonly string _name;
		private Vector2 _size;
		private Texture2D _sprite;
		private Stopwatch animationWatch;

		public Npc(string name, Vector2 startPos)
		{
			_name = name;
			Position = new Position { Current = startPos, Destination = startPos };
			Sprite = SpriteLoader.LoadTexture("alienBlue_front.png");
			SpriteDuck = SpriteLoader.LoadTexture("alienBlue_duck.png");
			_size = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
			_sprite = Sprite;
			Console.WriteLine(Sprite.Height + " " + Sprite.Width);
		}

		private RectangleF Rectangle =>
			new RectangleF(Position.Current.X - _size.X / 2f, Position.Current.Y - _size.Y / 2f, _size.X, _size.Y);

		public Texture2D Sprite { get; set; }
		public Texture2D SpriteDuck { get; set; }

		public void Render()
		{
			GameEngine.Sprite.Draw(_sprite, Rectangle);
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

		public bool IsCollidableWithLocalPlayer()
		{
			return true;
		}

		public void OnCollision(ICollisionable source)
		{
			animationWatch = animationWatch ?? Stopwatch.StartNew();
			animationWatch.Reset();
			animationWatch.Start();
			_sprite = SpriteDuck;
			_size = new Vector2(SpriteDuck.Width / 2f, SpriteDuck.Height / 2f);
		}

		public string Name()
		{
			return _name;
		}

		public Position Position { get; set; }
		
		public void Update()
		{
			if (animationWatch == null || animationWatch.ElapsedMilliseconds < 1000) return;
			_sprite = Sprite;
			_size = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
			animationWatch.Stop();
		}
	}
}