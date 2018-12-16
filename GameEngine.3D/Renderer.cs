using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace GameEngine._3D
{
	public class Renderer
	{
		public void Prepare()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.Color3(Color.Coral);
		}

		public void Render(RawModel model)
		{
			GL.BindVertexArray(model.VaoId);
			GL.EnableVertexAttribArray(0);
			//GL.BindBuffer(BufferTarget.ArrayBuffer, model.VboId);
			//GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

			
			//GL.BindBuffer(BufferTarget.ElementArrayBuffer, model.IboId);
			GL.DrawElements(PrimitiveType.Triangles, model.VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}