using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine
{
	public class Renderer
	{
		private readonly Dictionary<int, GameObject> _objects;
		private readonly int objectCount;
		private Vector2[][] _vectors;
		private readonly int[] _vbo;
		private readonly GameWindow window;

		public Renderer(GameWindow window, IList<GameObject> objects)
		{
			this.window = window;
			_objects = new Dictionary<int, GameObject>();
			objectCount = objects.Count;
			_vbo = new int[objectCount];
			_vectors = new Vector2[objectCount][];
			for (var i = 0; i < objects.Count; i++)
			{
				var gameObject = objects[i];
				_objects[i] = gameObject;
				_vectors[i] = gameObject.Vectors;
				gameObject.Vectors = new Vector2[0];
			}
		}

		public void Setup()
		{
			GL.GenBuffers(objectCount, _vbo);
			for (var i = 0; i < _vbo.Length; i++)
			{
				var buffer = _vectors[i];
				GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[i]);
				GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vector2.SizeInBytes * buffer.Length), buffer, BufferUsageHint.StaticDraw);
			}
		}
		
		public void Execute()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.ClearColor(Color.Coral);

			var projection = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);

			var world = Matrix4.CreateTranslation(0f, 0f, 0);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref world);

			GL.EnableClientState(ArrayCap.VertexArray);
			GL.Color3(Color.Aquamarine);

			for (var i = 0; i < _vbo.Length; i++)
			{
				var starting = _objects[i];
				world = Matrix4.CreateTranslation(starting.Position.X, starting.Position.Y, 0);
				var extracted = world.ExtractTranslation();
				_objects[i].Position.Set(extracted.X, extracted.Y);
				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadMatrix(ref world);
				GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[i]);
				GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes, 0);
				GL.DrawArrays(PrimitiveType.Quads, 0, _vectors[i].Length);
			}
			window.SwapBuffers();
		}
	}
}