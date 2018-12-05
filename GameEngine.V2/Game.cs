using System;
using System.Drawing;
using GameEngine.V2.Debug;
using GameEngine.V2.Scheduler;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.V2
{
	public class Game : NetworkedGame
	{
		protected override void OnServerTick()
		{
			
		}

		readonly EventScheduler scheduler = new EventScheduler();


		protected override void OnTick()
		{
			
		}

		private readonly GameDebugger _debugger;
		
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.ClearColor(Color.FromArgb(28, 28, 28));
			Sprite.Begin(this);
			_debugger.Run(DateTime.UtcNow.ToString());
			SwapBuffers();
		}

	
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			
			base.OnUpdateFrame(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		public Game(int width, int height, GraphicsMode mode, string title, long tickRateMillis, long serverTickRateMillis) 
			: base(width, height, mode, title, tickRateMillis, serverTickRateMillis)
		{
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			_debugger = new GameDebugger(this);
			_debugger.Initialize();
		}
	}
}