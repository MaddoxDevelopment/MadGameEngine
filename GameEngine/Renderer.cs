using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine
{
	public class Renderer
	{
		private readonly Dictionary<int, Entity> _entities;
		private readonly int entityCount;
		private readonly Vector2[][] _vectors;
		private readonly int[] _vbo;
		private readonly GameWindow window;

		protected virtual void OnEntityRendered(int index, Entity entity)
		{
			
		}
		
		public virtual void OnFrameUpdate(FrameEventArgs e)
		{
			
		}
		
		public Dictionary<int, Entity> EntityMap => _entities;

		public Renderer(GameWindow window, IList<Entity> objects)
		{
			this.window = window;
			_entities = new Dictionary<int, Entity>();
			entityCount = objects.Count;
			_vbo = new int[entityCount];
			_vectors = new Vector2[entityCount][];
			for (var i = 0; i < objects.Count; i++)
			{
				var entity = objects[i];
				_entities[i] = entity;
				_vectors[i] = entity.Vectors.ToArray();
				entity.Vectors = new List<Vector2>();
			}
		}

		public Entity GetEntity(int index)
		{
			return _entities[index];
		}
		
		public void UpdatePosition(int index, Position position)
		{
			_entities[index].Position = position;
		}

		public void UpdateEntity(int index, Entity entity)
		{
			_entities[index] = entity;
		}

		public void Setup()
		{
			GL.GenBuffers(entityCount, _vbo);
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
				var entity = _entities[i];
				world = Matrix4.CreateTranslation(entity.Position.X, entity.Position.Y, 0);
				var extracted = world.ExtractTranslation();
				entity.Position.Set(extracted.X, extracted.Y);
				UpdateEntity(i, entity);
				OnEntityRendered(i, entity);
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