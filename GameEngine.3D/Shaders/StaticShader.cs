using GameEngine._3D.Shaders.Base;

namespace GameEngine._3D.Shaders
{
	public class StaticShader : AbstractShaderProgram
	{
		public StaticShader() 
			: base("vertexShader", "fragmentShader")
		{
		}

		protected override void BindAttributes()
		{
			BindAttribute(0, "position");
		}
	}
}