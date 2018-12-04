using System;
using System.Linq;
using GameEngine.Base;
using OpenTK;
using OpenTK.Input;

namespace GameEngine
{
	public class KeyTest
	{
		private Game _game { get; set; }
		static Random rnd = new Random();


		public KeyTest(Game game)
		{
			_game = game;
			_game.KeyDown += (sender, args) =>
			{
		
				if (args.Key == Key.X)
				{
					var toRemove = _game.Collisionables.Where(w => w.Key is INpc npc && npc.Name() == "Alien");
					foreach (var pair in toRemove)
					{
						_game.Collisionables.TryRemove(pair.Key, out _);
					}
				}

				if (args.Key == Key.Z)
				{
					_game.Collisionables[new Npc(_game, "Alien", new Vector2(50, 50))] = 0;
					_game.Collisionables[new Npc(_game, "Alien", new Vector2(150, 50))] = 0;
					_game.Collisionables[new Npc(_game, "Alien", new Vector2(250, 50))] = 0;
				}
				if (args.Key == Key.Space)
				{
					var aliens = _game.Collisionables.Keys
						.Where(w => w is INameable nameable && nameable.Name() == "Alien").ToList();

					var alien = aliens[rnd.Next(aliens.Count - 1)];
					
					var copy = Position.FromPosition(_game.LocalPlayer.Position);
					var options = new ProjectileOptions
					{
						Collision = new ProjectileCollisionOptions
						{
							Type = ProjectileCollisionType.SpecificTarget,
							SpecificTarget = (ICollisionableLocatable)alien,
							OnCollision = collisionable =>
							{	
								Console.WriteLine("Collided.");
							}
						},
						MovementSpeed = 50,
						Sprite = SpriteLoader.LoadTexture("onfire_0001.png"),
						StartPosition = copy.Current
					};
					_game.Collisionables[new Projectile(_game, _game.LocalPlayer, options)] = 0;
				}
			};
		}
	}
}