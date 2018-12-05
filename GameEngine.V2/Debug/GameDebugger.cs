using System.Drawing;
using System.Globalization;
using OpenTK;

namespace GameEngine.V2.Debug
{
	public class GameDebugger
	{
		private static readonly Font _font = new Font(FontFamily.GenericMonospace, 32, FontStyle.Bold);
		private readonly Game _game;
		private readonly DelayedTextWriter _writer;
		private Point _mousePosition { get; set; }
		
		public GameDebugger(Game game, long textUpdateRateMillis = 500)
		{
			_game = game;
			_mousePosition = new Point();
			_writer = new DelayedTextWriter(_font, textUpdateRateMillis);
		}

		public void Initialize()
		{
			_game.MouseMove += (sender, args) => { _mousePosition = args.Position; };			
		}

		public void Run(string customText)
		{
			var x = 0;
			_writer.WriteTextDelayed("mousePos", "Mouse: " + _mousePosition, Vector2.One);
			x += 50;
			_writer.WriteTextDelayed("fps", "FPS: " + (_game.RenderFrequency / 1)
			                              .ToString(CultureInfo.InvariantCulture), new Vector2(0, x));
			x += 50;
			_writer.WriteTextDelayed("ups", "UPS: " + (_game.UpdateFrequency / 1)
			                              .ToString(CultureInfo.InvariantCulture), new Vector2(0, x));
			x += 50;
			_writer.WriteTextDelayed("custom", customText, new Vector2(0, x));
		}
	
	}
}