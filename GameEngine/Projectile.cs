using System;
using System.Drawing;
using System.Linq;
using GameEngine.Base;
using GameEngine.Physics;
using OpenTK;

namespace GameEngine
{
	public class Bullet : ICollisionable, IMoveable, IRemovable
	{
		public Texture2D Sprite { get; set; }
		private ICollisionable _source { get; set; }
		private readonly Vector2 _size;
		private readonly Game _game;

		public Bullet(Game game, ICollisionable source, Vector2 startPosition)
		{
			_game = game;
			_source = source;
			Sprite = SpriteLoader.LoadTexture("onfire_0001.png");
			Position = new Position { Current = startPosition, Destination = startPosition };
			_size = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
		}

		public void Render()
		{
			GameEngine.Sprite.Draw(Sprite, GetBounds());
		}

		public ICollisionable CollidingWith { get; set; }
		
		public bool CheckCollision()
		{
			foreach (var entity in _game.Collisionables.Keys.Where(w => w != this && w != _source))
			{
				var bounds = entity.GetBounds();
				if (!Collisions.IsColliding(GetBounds(), bounds))
					continue;
				CollidingWith = entity;
				entity.OnCollision(this);
				Remove();
				return true;
			}
			return false;
		}

		public RectangleF GetBounds()
		{
			return new RectangleF(Position.Current.X - _size.X / 2f, Position.Current.Y - _size.Y / 2f, _size.X, _size.Y);
		}

		public bool IsCollidableWithLocalPlayer()
		{
			return false;
		}

		public void OnCollision(ICollisionable source)
		{
			
		}

		public Direction Direction { get; set; }
		
		public void Move()
		{
			if (Position.Current.X > 1000)
			{
				Remove();
				return;
			}
			Position.AddX(20);
		}

		public Position Position { get; set; }
		
		public void Remove()
		{
			_game.Collisionables.TryRemove(this, out _);
		}
	}
}