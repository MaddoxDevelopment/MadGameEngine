using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameEngine.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine
{
	public class Game : GameWindow
	{
		public static readonly int GridSize = 48;
		public readonly View View;

		public List<ICollisionable> Collisionables;

		public Game(float scale = 2.0f) : base(1280, 720)
		{
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			View = new View(Vector2.Zero, scale);
		}

		public IPlayer LocalPlayer { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			LocalPlayer = new Player(this, "Local Player", new Vector2(), true);
			Collisionables = new List<ICollisionable>
			{
				LocalPlayer,
				new Npc("Alien", new Vector2(50f, 50f))
			};
			base.OnLoad(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);
			Collisionables.ForEach(r => r.CheckCollision());
			Collisionables.OfType<IMoveable>().ToList().ForEach(w => w.Move());
			View.SetPosition(LocalPlayer.Position.Current);
			View.Update();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.ClearColor(Color.FromArgb(28, 28, 28));

			Sprite.Begin(this);
			View.ApplyMatrix();

			Collisionables.ForEach(r => r.Render());

			SwapBuffers();
		}
	}
}