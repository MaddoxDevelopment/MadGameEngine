using System;
using System.Drawing;
using GameEngine.V2.Camera;
using GameEngine.V2.Debug;
using GameEngine.V2.Networking;
using GameEngine.V2.Positioning;
using GameEngine.V2.Queue;
using GameEngine.V2.Sprite;
using GameEngine.V2.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GameEngine.V2
{
	public class Game : NetworkedGame
	{
		protected override void OnServerTick()
		{
			
		}

		protected override void OnTick()
		{
			
		}


		private readonly GameDebugger _debugger;
		private readonly TweenCamera _camera;
		private Texture2D _block;
		private readonly Texture2D _enemy;
		private string _text;

		private RectangleF _player => new RectangleF(_blockPosition.Current.X - _block.Width, _blockPosition.Current.Y - _block.Height, _block.Width, _block.Height);

		private readonly Position _blockPosition;
		
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.ClearColor(Color.FromArgb(28, 28, 28));
			RenderQueue.Instance.Execute();
			SwapBuffers();
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.Z)
			{
				_blockPosition.AddX(_block.Width);
				_block = new Texture2D(_block.Id, -_block.Width, _block.Height);
			}

			if (e.Key == Key.X)
			{
				Test();
			}

			if (e.Key == Key.C)
			{
				rotation = 0;
			}
			base.OnKeyDown(e);
		}

		

		private void Test()
		{
			RenderQueue.Instance.Enqueue(() => rotation == 90, () =>
			{
				if (rotation < 90)
				{
					rotation += 5;
				}
				if (rotation > 90)
				{
					rotation = 90;
				}
			}, () =>
			{
				
			});
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			UpdateQueue.Instance.Execute();
			base.OnUpdateFrame(e);
		}

		private int rotation = 0;
		
		public Game(int width, int height, GraphicsMode mode, string title, long tickRateMillis, long serverTickRateMillis) 
			: base(width, height, mode, title, tickRateMillis, serverTickRateMillis)
		{
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			_debugger = new GameDebugger(this);
			_debugger.Initialize();			
			_block = SpriteLoader.LoadTexture("slimeBlock.png");
			_blockPosition = new Position {Current = Vector2.Zero};
			_enemy = SpriteLoader.LoadTexture("alienBlue_front.png");
			_camera = new TweenCamera
			{
				Zoom = 1,
				Position = new Position { Current = Vector2.One }
			};
			
			UpdateQueue.Instance.Enqueue(-1, () =>
			{
				if (Focused)
				{
					var state = Keyboard.GetState(0);

					if (state.IsKeyDown(Key.W))
						_blockPosition.SubtractY(10);
					if (state.IsKeyDown(Key.S))
						_blockPosition.AddY(10);
					if (state.IsKeyDown(Key.A))
						_blockPosition.SubtractX(10);
					if (state.IsKeyDown(Key.D))
						_blockPosition.AddX(10);
					if (state.IsKeyDown(Key.X))
						rotation += 2;
				}

				_camera.SetPosition(_blockPosition.Current);
				_camera.Move();
			});
			
			_debugger.Run(() => 
				"Player: " + _blockPosition.Current + " Rotation: " + rotation);

			
			RenderQueue.Instance.Enqueue(-1, () =>
			{
				Sprite.Sprite.Begin(this);
				_camera.Render(this);
				Sprite.Sprite.Draw(_enemy, new Vector2(600, -90));
			});
			
			
			
			RenderQueue.Instance.Enqueue(-1, () =>
			{		
				Sprite.Sprite.DrawWithRotation(rotation, _block, _player);	
			});
		}
	}
}