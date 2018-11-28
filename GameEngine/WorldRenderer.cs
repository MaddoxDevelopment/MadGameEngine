using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace GameEngine
{
	public class WorldRenderer : Renderer
	{
		private bool animating;
		private int lastNumber;
		private bool isDropping;
		private bool clearedAnimTimer;
		private double animTimer;
		private Position original;
		
		public WorldRenderer(GameWindow window, IList<Entity> entities) : base(window, entities)
		{
			window.KeyDown += (sender, args) =>
			{
				if (args.Key == Key.K)
				{
					animating = true;
				}
			};
		}

		public override void OnFrameUpdate(FrameEventArgs e)
		{
			if (animating && !clearedAnimTimer)
			{
				var obj = GetEntity(0);
				original = (Position)obj.Position.Clone();
				animTimer = 0;
				lastNumber = -1;
				clearedAnimTimer = true;
			}
			if (animating)
			{
				var obj = GetEntity(0);
				if (!isDropping)
				{
					obj.Position.Up(60);
				}
				if (original.Y - obj.Position.Y >= 400)
				{
					isDropping = true;
				}
				if (isDropping)
				{
					obj.Position.Down(60);					
					if (obj.Position.Equals(original))
					{
						isDropping = false;
						clearedAnimTimer = false;
						animating = false;
						return;
					}
				}
				
			}
	
			base.OnFrameUpdate(e);
		}
	}
}