using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace GameEngine._3D.Shaders.Base
{
	public abstract class AbstractShaderProgram : IDisposable
	{
		public int ProgramId { get; set; }
		public int VertexShaderId { get; }
		public int FragmentShaderId { get; }

	
		public AbstractShaderProgram(string vertexProgramName, string fragmentProgramName)
		{
			VertexShaderId = LoadShader(vertexProgramName, ShaderType.VertexShader);
			FragmentShaderId = LoadShader(fragmentProgramName, ShaderType.FragmentShader);
			ProgramId = GL.CreateProgram();
		}

		public void Load()
		{
			GL.AttachShader(ProgramId, VertexShaderId);
			GL.AttachShader(ProgramId, FragmentShaderId);
			BindAttributes();
			GL.LinkProgram(ProgramId);
			GL.ValidateProgram(ProgramId);
		}

		public void Start()
		{
			GL.UseProgram(ProgramId);
		}

		public void Stop()
		{
			GL.UseProgram(0);
		}
		
		protected abstract void BindAttributes();

		protected void BindAttribute(int attribute, string name)
		{
			GL.BindAttribLocation(ProgramId, attribute, name);
		}

		protected int LoadShader(string name, ShaderType type)
		{
			var dir = Directory.GetCurrentDirectory();
			var path = Path.Combine(dir, "Shaders", name + ".glsl");
			var source = File.ReadAllText(path);
			var shader = GL.CreateShader(type);
			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);
			GL.GetShader(shader, ShaderParameter.CompileStatus, out var status);
			GL.GetShader(shader, ShaderParameter.InfoLogLength, out var log);
			Console.Write("Log length: " + log);
			Console.WriteLine("Status: " + status);
			return shader;
		}

		public void Dispose()
		{
			Stop();
			GL.DetachShader(ProgramId, VertexShaderId);
			GL.DetachShader(ProgramId, FragmentShaderId);
			GL.DeleteShader(VertexShaderId);
			GL.DeleteShader(FragmentShaderId);
			GL.DeleteProgram(ProgramId);
		}
	}

}