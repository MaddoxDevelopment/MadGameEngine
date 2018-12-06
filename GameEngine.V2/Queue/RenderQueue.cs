using GameEngine.V2.Queue.Base;

namespace GameEngine.V2.Queue
{
	public class RenderQueue : GameQueue
	{
		private static RenderQueue instance;

		public static RenderQueue Instance => instance ?? (instance = new RenderQueue());
	}
}