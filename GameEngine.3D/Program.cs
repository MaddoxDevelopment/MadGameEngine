using System;

namespace GameEngine._3D
{
	class Program
	{
		static void Main(string[] args)
		{
			
			try
			{
				var game = new Game(1280, 720);
				game.Run(60);
			}
			catch (Exception e)
			{
				Console.Write(e);
			}
			
		}
	}
}