namespace GameEngine
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var window = new Game();
			window.Run(60, 0);
		}
	}
}