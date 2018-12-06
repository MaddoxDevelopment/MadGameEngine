using GameEngine.V2.Queue.Base;

namespace GameEngine.V2.Queue
{
	public class UpdateQueue : GameQueue
	{
		private static UpdateQueue instance;
		
		public static UpdateQueue Instance => instance ?? (instance = new UpdateQueue());
	}
}