using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameEngine.Base;
using GameEngine.Physics;
using OpenTK;
using OpenTK.Input;

namespace GameEngine
{
	public class Player : IPlayer
	{
		private readonly Game _game;

		private readonly HashSet<Key> _movementKeys = new HashSet<Key>
		{
			Key.W, Key.A, Key.S, Key.D
		};

		private readonly string _name;
		private readonly Vector2 _size;

		public Player(Game game, string name, Vector2 startPosition, bool isLocalPlayer)
		{
			_game = game;
			_name = name;
			Position = new Position { Current = startPosition, Destination = startPosition };
			Sprite = SpriteLoader.LoadTexture("slimeBlock.png");
			Console.WriteLine(Sprite.Height + " " + Sprite.Width);
			_size = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
			IsLocalPlayer = isLocalPlayer;
		}

		private RectangleF Rectangle =>
			new RectangleF(Position.Current.X - _size.X / 2f, Position.Current.Y - _size.Y / 2f, _size.X, _size.Y);

		public Texture2D Sprite { get; set; }
		public Position Position { get; set; }

		public void Render()
		{
			GameEngine.Sprite.Draw(Sprite, Rectangle);
		}

		public bool IsLocalPlayer { get; }

		public bool CheckCollision()
		{
			return CheckCollision(Rectangle);
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

		public void Move()
		{
			var state = Keyboard.GetState(0);

			if (!_movementKeys.Any(w => state.IsKeyDown(w))) return;

			var copy = Position.FromPosition(Position);

			if (state.IsKeyDown(Key.W))
				copy.SubtractY(10);
			if (state.IsKeyDown(Key.S))
				copy.AddY(10);
			if (state.IsKeyDown(Key.A))
				copy.SubtractX(10);
			if (state.IsKeyDown(Key.D))
				copy.AddX(10);

			if (CheckCollision(new RectangleF(copy.Current.X - _size.X / 2f, copy.Current.Y - _size.Y / 2f, _size.X,
				_size.Y))) return;

			Position = copy;
		}

		public Direction Direction { get; set; }

		public bool CheckCollision(RectangleF rectangle)
		{
			foreach (var entity in _game.Collisionables.Where(w => w != this))
			{
				var bounds = entity.GetBounds();
				if (!Collisions.IsColliding(rectangle, bounds))
					continue;
				CollidingWith = entity;
				return true;
			}

			return false;
		}
	}
}