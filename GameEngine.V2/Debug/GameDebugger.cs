using System;
using System.Drawing;
using GameEngine.V2.Queue;
using GameEngine.V2.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.V2.Debug
{
	public class GameDebugger : IDisposable
	{
		private static readonly Font Font = new Font(FontFamily.GenericMonospace, 32, FontStyle.Bold);
		private readonly Game _game;
		private bool _shouldRun;
		private Point _mousePosition;
		
		public GameDebugger(Game game)
		{
			_game = game;
			_shouldRun = true;
			_mousePosition = new Point();
		}

		public void Initialize()
		{
			_game.MouseMove += (sender, args) => { _mousePosition = args.Position; };
		}

		public void Stop()
		{
			_shouldRun = false;
		}

		public void Run(Func<string> customText)
		{
			var texturesMouse = TextWriter.LoadText(Font, "Mouse: " + _mousePosition);
			var rQueueSize = TextWriter.LoadText(Font, "RenderQ: " + RenderQueue.Instance.Count);
			var uQueueSize = TextWriter.LoadText(Font, "UpdateQ: " + UpdateQueue.Instance.Count);
			var textures = TextWriter.LoadText(Font, customText?.Invoke());
			RenderQueue.Instance.Enqueue(500, () =>
			{
				
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Rotate(0, Vector3d.UnitZ);			
				GL.Ortho(0, _game.Width, _game.Height, 0, -1, 1);
				TextWriter.PrintText(textures, Vector2.One);
				TextWriter.PrintText(texturesMouse, new Vector2(0, 50));
				TextWriter.PrintText(rQueueSize, new Vector2(0, 100));
				TextWriter.PrintText(uQueueSize, new Vector2(0, 150));
			}, () =>
			{
				Run(customText);
			});
		}

		public void Dispose()
		{
			Stop();
			_game?.Dispose();
		}
	}
}