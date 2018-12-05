using System;
using OpenTK.Graphics;

namespace GameEngine.V2
{
	class Program
	{
		static void Main(string[] args)
		{
			var game = new Game(1650, 1050, GraphicsMode.Default, "MadGameEngine", 300, 100);
			game.Run(60, 0);
		}
	}
}