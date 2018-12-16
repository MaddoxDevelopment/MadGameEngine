using System;
using GameEngine._3D.Shaders;
using GameEngine._3D.Shaders.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine._3D
{
	public class Game : GameWindow
	{
		private readonly ModelLoader _loader;
		private readonly Renderer _renderer;
		private readonly RawModel _model;
		private readonly AbstractShaderProgram _shader;

		public Game(int width, int height) : base(width, height)
		{
			_loader = new ModelLoader();
			_renderer = new Renderer();			
			_shader = new StaticShader();

			float[] vertices = {
				-0.5f, 0.5f, 0f, //v0
				-0.5f, -0.5f, 0f, //v1
			
				0.5f, -0.5f, 0f, //v2
				0.5f, 0.5f, 0f //v3
				
			};
			int[] indices = {
				0, 3, 1,
				3, 1, 2
			};
			
			_shader.Load();
			_model = _loader.LoadToVao(vertices, indices);
		}

		protected override void OnUnload(EventArgs e)
		{
			_shader.Dispose();
			_loader.Dispose();
			base.OnUnload(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			_renderer.Prepare();
			//_shader.Start();
			_renderer.Render(_model);
			//_shader.Stop();
			GL.Flush();
			SwapBuffers();
		}
	}
}