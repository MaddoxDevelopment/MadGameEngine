using System;
using System.Collections.Concurrent;
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

		public ConcurrentDictionary<ICollisionable, byte> Collisionables;

		public Game(float scale = 2.0f) : base(1650, 1050)
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
			Collisionables = new ConcurrentDictionary<ICollisionable, byte>();
			Collisionables[LocalPlayer] = 0;
			Collisionables[new Npc(this, "Alien", new Vector2(50, 50))] = 0;
			Collisionables[new Npc(this, "Alien", new Vector2(150, 50))] = 0;
			Collisionables[new Npc(this, "Alien", new Vector2(250, 50))] = 0;

			new KeyTest(this);
			base.OnLoad(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);
			foreach (var key in Collisionables.Keys)
			{
				key.CheckCollision();
			}
			Collisionables.Keys.OfType<IMoveable>().ToList().ForEach(w => w.Move());
			View.SetPosition(LocalPlayer.Position.Current);
			Collisionables.Keys.OfType<IUpdateable>().ToList().ForEach(w => w.Update());
			View.Update();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.ClearColor(Color.FromArgb(28, 28, 28));

			Sprite.Begin(this);
			View.ApplyMatrix();

			foreach (var key in Collisionables.Keys)
			{
				key.Render();
			}

			SwapBuffers();
		}
	}
}