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

	internal class Test : GameWindow
	{
		private Vector2[][] buffer;
		private readonly Dictionary<int, Tuple<float, float>> location = new Dictionary<int, Tuple<float, float>>
		{
			{0, new Tuple<float, float>(100f, 100f)},
			{1, new Tuple<float, float>(500f, 500f)}
		};

		private int selectedIndex = -1;
		private readonly int[] _vbo = new int[2]; //vertex buffer objects

		public Test(int width, int height, GraphicsMode mode) : base(width, height, mode)
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			buffer = new[]
			{
				new[]
				{
					new Vector2(0, 0),
					new Vector2(200, 0),
					new Vector2(200, 200),
					new Vector2(0, 200)
				},
				new[]
				{
					new Vector2(0, 0),
					new Vector2(200, 0),
					new Vector2(200, 200),
					new Vector2(0, 200)
				}
			};

			SetBuffer();
			base.OnLoad(e);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			for (var i = 0; i < buffer.Length; i++)
			{
				var translation = location[i];
				var points = buffer[i].Select(w => new Point((int)((int)w.X + translation.Item1), (int)((int)w.Y + translation.Item2))).ToList();
				var minX = points.Min(p => p.X);
				var minY = points.Min(p => p.Y);
				var maxX = points.Max(p => p.X);
				var maxY = points.Max(p => p.Y);
				var rec = new Rectangle(new Point(minX, minY), new Size(maxX-minX, maxY-minY));
				Console.WriteLine("Rec: " + JsonConvert.SerializeObject(points));
				if (!rec.Contains(e.Position)) continue;
				selectedIndex = i;
				break;
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			if (selectedIndex == -1)
			{
				return;
			}
			location[selectedIndex] = new Tuple<float, float>(e.X, e.Y);
			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			selectedIndex = -1;
			base.OnMouseUp(e);
		}

		private void SetBuffer()
		{
			GL.GenBuffers(2, _vbo);
			for (var i = 0; i < _vbo.Length; i++)
			{
				Console.WriteLine(i + " " + _vbo[i]);
				var b = buffer[i];
				GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[i]);
				GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vector2.SizeInBytes * b.Length), b, BufferUsageHint.StaticDraw);
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
				var starting = location[i];
				world = Matrix4.CreateTranslation(starting.Item1, starting.Item2, 0);
				var extracted = world.ExtractTranslation();
				location[i] = new Tuple<float, float>(extracted.X, extracted.Y);
				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadMatrix(ref world);
				GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[i]);
				GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes * i, 0);
				GL.DrawArrays(PrimitiveType.Quads, 0, buffer[i].Length);	
			}
			this.SwapBuffers();
		}
	}
}