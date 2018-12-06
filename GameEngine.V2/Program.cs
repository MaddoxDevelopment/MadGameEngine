using System;
using GameEngine.V2.Queue;
using OpenTK.Graphics;

namespace GameEngine.V2
{
	class Program
	{
		static void Main(string[] args)
		{
			var game = new Game(1280, 720, GraphicsMode.Default, "MadGameEngine", 300, 100);
			game.Run(60, 60);
		}
	}
}