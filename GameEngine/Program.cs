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
			var window = new Test(1650, 1050, GraphicsMode.Default);
			window.Run();
		}
	}

	internal class GameObjectPosition
	{
		public float X { get; private set; }
		public float Y { get; private set; }

		public GameObjectPosition()
		{
			X = 0;
			Y = 0;
		}

		public void Set(float x, float y)
		{
			X = x;
			Y = y;
		}
		public void Left(float x) => X -= x;
		public void Right(float x) => X += x;
		public void Up(float y) => Y -= y;
		public void Down(float y) => Y += y;
	}

	internal class GameObject
	{
		public Guid Id { get; } = Guid.NewGuid();
		public GameObjectPosition Position { get; }

		public GameObject()
		{
			Position = new GameObjectPosition();
		}
	
	}

	internal class Test : GameWindow
	{
		private Vector2[][] objectVectors;

		private readonly Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();

		private Tuple<int, Rectangle> selectedTarget;
		private readonly int[] _vbo = new int[10];

		public Test(int width, int height, GraphicsMode mode) : base(width, height, mode)
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			var offset = 300;
			var vectors = new[]
			{
				new Vector2(0, 0),
				new Vector2(200, 0),
				new Vector2(200, 200),
				new Vector2(0, 200)
			};
			var temp = new List<Vector2[]>();
			for (var i = 0; i < 10; i++)
			{
				temp.Add(vectors);
				objects.Add(i, new GameObject());
				objects[i].Position.Down(100);
				var multiplier = i * 100;
				objects[i].Position.Right(multiplier + offset);
			}
			objectVectors = temp.ToArray();
			SetBuffer();
			base.OnLoad(e);
		}

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

		private void SetBuffer()
		{
			GL.GenBuffers(objects.Count, _vbo);
			for (var i = 0; i < _vbo.Length; i++)
			{
				var b = objectVectors[i];
				GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[i]);
				GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vector2.SizeInBytes * b.Length), b,
					BufferUsageHint.StaticDraw);
			}
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.ClearColor(Color.Coral);

			var projection = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, 0, 1);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);

			var world = Matrix4.CreateTranslation(0f, 0f, 0);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref world);

			GL.EnableClientState(ArrayCap.VertexArray);
			GL.Color3(Color.Aquamarine);

			for (var i = 0; i < _vbo.Length; i++)
			{
				var starting = objects[i];
				world = Matrix4.CreateTranslation(starting.Position.X, starting.Position.Y, 0);
				var extracted = world.ExtractTranslation();
				objects[i].Position.Set(extracted.X, extracted.Y);
				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadMatrix(ref world);
				GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[i]);
				GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes, 0);
				GL.DrawArrays(PrimitiveType.Quads, 0, objectVectors[i].Length);
			}

			this.SwapBuffers();
		}
	}
}