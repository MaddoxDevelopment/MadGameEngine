using System;
using System.Drawing;
using System.Linq;
using GameEngine.Base;
using GameEngine.Movement;
using GameEngine.Physics;
using OpenTK;

namespace GameEngine
{
	public class Projectile : ICollisionable, IMoveable, IRemovable
	{
		private readonly ProjectileOptions _options;
		private ICollisionable _source { get; set; }

		private readonly Vector2 _size;
		private readonly Game _game;
		private readonly TweenMovement _tweenMovement;

		public Projectile(Game game, ICollisionable source, ProjectileOptions options)
		{
			_game = game;
			_source = source;
			_options = options;
			Position = new Position { Current = options.StartPosition, Destination = options.StartPosition };
			_size = new Vector2(options.Sprite.Width / 2f, options.Sprite.Height / 2f);
			_tweenMovement = new TweenMovement(this);

			if (options.Collision.Type == ProjectileCollisionType.SpecificTarget)
			{
				_tweenMovement.SetPosition(options.Collision.SpecificTarget, options.MovementSpeed);
			}
		}

		public void Render()
		{
			Sprite.Draw(_options.Sprite, GetBounds());
		}

		public ICollisionable CollidingWith { get; set; }

		public bool CheckCollision()
		{
			ICollisionable result = null;

			if (_options.Collision.Type == ProjectileCollisionType.SpecificTarget)
			{
				var target = _options.Collision.SpecificTarget;
				if (target == null)
				{
					Remove();
					return false;
				}
				if (target.Position.Current != Position.Current)
				{
					return false;
				}

				result = target;
			}

			if (_options.Collision.Type == ProjectileCollisionType.FirstHit)
			{
				foreach (var entity in _game.Collisionables.Keys.Where(w => w != this && w != _source))
				{
					var bounds = entity.GetBounds();
					if (!Collisions.IsColliding(GetBounds(), bounds))
						continue;
					result = entity;
				}
			}

			if (result == null)
				return false;

			CollidingWith = result;
			result.OnCollision(this);
			OnCollision(result);
			Remove();
			return true;
		}

		public RectangleF GetBounds()
		{
			return new RectangleF(Position.Current.X - _size.X / 2f, Position.Current.Y - _size.Y / 2f, _size.X,
				_size.Y);
		}

		public bool CanCollideLocalPlayer()
		{
			return false;
		}

		public void OnCollision(ICollisionable source)
		{
			_options.Collision.OnCollision?.Invoke(source);
			Remove();
		}

		public Direction Direction { get; set; }

		public void Move()
		{
			if (Position.Current.X > 10000)
			{
				Remove();
				return;
			}

			switch (_options.Collision.Type)
			{
				case ProjectileCollisionType.SpecificTarget:
					_tweenMovement.Step(_options.Collision.SpecificTarget);
					break;
				case ProjectileCollisionType.FirstHit:
					Position.AddX(_options.MovementSpeed);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public Position Position { get; set; }

		public void Remove()
		{
			_game.Collisionables.TryRemove(this, out _);
		}
	}
}