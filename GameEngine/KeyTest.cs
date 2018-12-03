using OpenTK.Input;

namespace GameEngine
{
	public class KeyTest
	{
		private Game _game { get; set; }

		public KeyTest(Game game)
		{
			_game = game;
			_game.KeyDown += (sender, args) =>
			{
				if (args.Key == Key.Space)
				{
					var copy = Position.FromPosition(_game.LocalPlayer.Position);

					_game.Collisionables[new Bullet(_game, _game.LocalPlayer, copy.Current)] = 0;
				}
			};
		}
	}
}