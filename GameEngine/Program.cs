using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GameEngine
{
	class Program
	{
		static void Main(string[] args)
		{
			var window = new Game(1650, 1050, GraphicsMode.Default);
			window.Run(60, 60);
		}
	}

	internal class Game : GameWindow
	{
		private Tuple<int, Rectangle> selectedTarget;
		private Renderer _renderer;

		public Game(int width, int height, GraphicsMode mode) : base(width, height, mode)
		{
		}

		protected override void OnLoad(EventArgs e)
		{		
			base.OnLoad(e);
			_renderer = new WorldRenderer(this, Initializer.BuildGameObjects(this));
			_renderer.Setup();
		}

		/*
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			for (var i = 0; i < objectVectors.Length; i++)
			{
				var translation = objects[i];
				var points = objectVectors[i].Select(w =>
					new Point((int)((int)w.X + translation.Position.X), (int)((int)w.Y + translation.Position.Y))).ToList();
				var minX = points.Min(p => p.X);
				var minY = points.Min(p => p.Y);
				var maxX = points.Max(p => p.X);
				var maxY = points.Max(p => p.Y);
				var rec = new Rectangle(new Point(minX, minY), new Size(maxX - minX, maxY - minY));
				if (!rec.Contains(e.Position)) continue;
				selectedTarget = new Tuple<int, Rectangle>(i, rec);
				break;
			}
			base.OnMouseDown(e);
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			var state = e.Keyboard;
			if (state.IsKeyDown(Key.Number1) && state.IsKeyDown(Key.Number2))
			{
				objects[0].Position.Left(10);
				objects[0].Position.Up(10);
			}
			if (e.Key == Key.Number1)
			{
				objects[0].Position.Right(10);
				objects[0].Position.Down(10);
			}
			base.OnKeyDown(e);
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			if (selectedTarget == null)
			{
				return;
			}
			var rect = selectedTarget.Item2;
			var halfWidth = rect.Width / 2;
			var halfHeight = rect.Height / 2;
			objects[selectedTarget.Item1].Position.Set(e.X - halfHeight, e.Y - halfWidth);
			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			selectedTarget = null;
			base.OnMouseUp(e);
		}
		*/

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);
			_renderer.OnFrameUpdate(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			_renderer.Execute();	
		}
	}
}