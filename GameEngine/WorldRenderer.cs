using System.Collections.Generic;
using OpenTK;

namespace GameEngine
{
	public class WorldRenderer : Renderer
	{
		public WorldRenderer(GameWindow window, IList<GameObject> objects) : base(window, objects)
		{
		
		}

		protected override void OnObjectRendered(int index, GameObject obj)
		{
		}
	}
}