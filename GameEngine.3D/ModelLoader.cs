using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GameEngine._3D
{
	public class ModelLoader : IDisposable
	{
		private readonly HashSet<int> _vaos = new HashSet<int>();
		private readonly HashSet<int> _vbos = new HashSet<int>();
		
		public RawModel LoadToVao(float[] positions, int[] indices)
		{
			var vao = CreateVao();
			var ibo = BindIndicesBuffer(indices);
			var vbo = StoreInAttributeList(0, positions);
			UnbindVao();
			return new RawModel(vao, vbo,ibo, indices.Length, indices.Length);
		}

		private int CreateVao()
		{
			var id = GL.GenVertexArray();
			GL.BindVertexArray(id);
			_vaos.Add(id);
			return id;
		}

		private void UnbindVao()
		{
			GL.BindVertexArray(0);
		}

		private int StoreInAttributeList(int index, float[] data)
		{
			var vbo = GL.GenBuffer();
			_vbos.Add(vbo);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			return vbo;
		}

		private int BindIndicesBuffer(int[] indices)
		{
			var vbo = GL.GenBuffer();
			_vbos.Add(vbo);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
			return vbo;
		}

		public void Dispose()
		{
			foreach (var vao in _vaos)
			{
				GL.DeleteVertexArray(vao);
			}
			foreach (var vbo in _vbos)
			{
				GL.DeleteBuffer(vbo);	
			}
		}
	}
}