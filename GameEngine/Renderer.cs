using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine
{
	public class Renderer
	{
		private readonly Dictionary<int, Entity> _entities;
		private readonly int entityCount;
		private readonly Dictionary<int, Vector2[]> _vectors;
		private readonly Dictionary<int, int> _vbo;
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
			_entities = new Dictionary<int, Entity>(entityCount);
			entityCount = objects.Count;
			_vbo = new Dictionary<int, int>(entityCount);
			_vectors = new Dictionary<int, Vector2[]>();
			for (var i = 0; i < objects.Count; i++)
			{
				var entity = objects[i];
				_entities[i] = entity;
				_vectors[i] = entity.Vectors.ToArray();
				entity.Vectors = new List<Vector2>();
			}
		}

		public Entity GetEntity(int index, bool includeVectors = false)
		{
			var entity = _entities.ContainsKey(index) ? _entities[index] : null;
			if (entity == null)
				return null;
			entity.Vectors = includeVectors ? _vectors[index].ToList() : new List<Vector2>();
			return entity;
		}

		public List<Vector2> GetVectors(int index)
		{
			return _vectors.ContainsKey(index) ? _vectors[index].ToList() : new List<Vector2>();

		}
		
		public void UpdatePosition(int index, Position position)
		{
			if (!_entities.ContainsKey(index))
				return;
			_entities[index].Position = position;
		}

		public void UpdateEntity(int index, Entity entity)
		{
			if (!_entities.ContainsKey(index))
				return;
			_entities[index] = entity;
			_vectors[index] = entity.Vectors.ToArray();
			SetEntityBuffer(index);
		}

		public void UpdateVectors(int index, List<Vector2> vectors)
		{
			if (!_vectors.ContainsKey(index))
				return;
			_vectors[index] = vectors.ToArray();
			SetEntityBuffer(index);
		}

		public void RemoveEntity(int index)
		{
			if (!_entities.ContainsKey(index))
				return;
			_entities.Remove(index);
			_vectors.Remove(index);
			ClearBuffer(index);
		}

		public void Setup()
		{
			var temp = new int[entityCount];
			GL.GenBuffers(entityCount, temp);
			for (var i = 0; i < temp.Length; i++)
			{
				_vbo[i] = temp[i];
				SetEntityBuffer(i);
			}
		}

		private void SetEntityBuffer(int index)
		{
			var buffer = _vectors[index];
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[index]);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vector2.SizeInBytes * buffer.Length), buffer, BufferUsageHint.DynamicDraw);
		}

		private void ClearBuffer(int index)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[index]);
			GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicDraw);
			_vbo.Remove(index);
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

			foreach (var i in _vbo.Keys)
			{
				if (!_entities.ContainsKey(i))
				{
					continue;
				}
				var entity = _entities[i];
				world = Matrix4.CreateTranslation(entity.Position.X, entity.Position.Y, 0);
				var extracted = world.ExtractTranslation();
				entity.Position.Set(extracted.X, extracted.Y);
				OnEntityRendered(i, entity);
				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadMatrix(ref world);
				GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[i]);
				GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes, 0);
				GL.DrawArrays(PrimitiveType.Polygon, 0, _vectors[i].Length);			
			}
	
			window.SwapBuffers();
		}
	}
}