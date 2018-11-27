using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace GameEngine
{
	public class WorldRenderer : Renderer
	{
		private bool animating;
		private int upFrames;
		private int downFrames;
		
		public WorldRenderer(GameWindow window, IList<GameObject> objects) : base(window, objects)
		{
			window.KeyDown += (sender, args) =>
			{
				if (args.Key == Key.K)
				{
					animating = true;
				}
			};
		}

		protected override void OnObjectRendered(int index, GameObject obj)
		{
		}

		public override void OnFrameUpdate(FrameEventArgs e)
		{
			if (animating)
			{
				var obj = ObjectMap[0];
				if (upFrames < 10)
				{
					obj.Position.Up(upFrames * 10);
					UpdateGameObject(0, obj);
					upFrames++;
				}
				else
				{
					obj.Position.Down(downFrames * 10);
					UpdateGameObject(0, obj);
					downFrames++;
				}
				if (upFrames == 10 && downFrames == 10)
				{
					upFrames = 0;
					downFrames = 0;
					animating = false;
				}
			}
			base.OnFrameUpdate(e);
		}
	}
}